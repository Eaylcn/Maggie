using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInputSet input { get; private set; }
    
    public StateMachine stateMachine;

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerBasicAttackState basicAttackState { get; private set; }
    public PlayerJumpAttackState jumpAttackState { get; private set; }

    [Header("Attack Settings")]
    public Vector2[] attackMovement;             // |EN| Attack movement vectors for each combo |TR| Her kombinasyon için saldırı hareket vektörleri
    public Vector2 jumpAttackMovement;           // |EN| Movement vector for jump attack |TR| Zıplama saldırısı için hareket vektörü
    public float attackMovementDuration = 0.1f;  // |EN| Duration for which attack movement is applied |TR| Saldırı hareketinin uygulandığı süre
    public float comboResetTime = 1.0f;          // |EN| Time after which the attack combo resets |TR| Saldırı kombosunun sıfırlandığı süre
    private Coroutine queuedAttackCoroutine;     // |EN| Coroutine to handle queued attacks |TR| Sıraya alınan saldırıları yönetmek için Coroutine

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public Vector2 wallJumpForce;
    
    [Range(0f, 1f)]
    public float airMoveMultiplier = 0.8f;                  // |EN| Should be between 0 and 1 |TR| 0 ile 1 arasında olmalı
    [Range(0f, 1f)]
    public float wallSlideSlowdownFactor = 0.3f;            // |EN| Factor to slow down fall speed during wall slide |TR| Duvar kayması sırasında düşme hızını yavaşlatma faktörü
    [Space]
    public float dashDuration = 0.25f;                      // |EN| Duration of the dash in seconds |TR| Dash süresi (saniye cinsinden)
    public float dashSpeed = 20f;                           // |EN| Speed during the dash |TR| Dash sırasında hız
    private bool facingRight = true;                        // |EN| True if facing right, false if facing left |TR| Sağ bakıyorsa true, sol bakıyorsa false
    public int facingDirection { get; private set; } = 1;   // |EN| 1 for right, -1 for left |TR| Sağ için 1, sol için -1
    public Vector2 movementInput { get; private set; }      // |EN| Player movement input vector |TR| Player hareket girdisi vektörü

    [Header("Collision Detection Settings")]
    [SerializeField] private float groundCheckDistance = 1.4f;  // |EN| Distance for ground detection raycast |TR| Zemin algılama ışın mesafesi
    [SerializeField] private float wallCheckDistance = 0.4f;    // |EN| Distance for wall detection raycast |TR| Duvar algılama ışın mesafesi
    [SerializeField] private LayerMask whatIsGround;            // |EN| LayerMask to define what is considered ground |TR| Hangi katmanın zemin olarak kabul edileceğini tanımlamak için LayerMask
    [SerializeField] Transform primaryWallCheck;                // |EN| Primary transform for wall detection |TR| Duvar algılama için birincil transform
    [SerializeField] Transform secondaryWallCheck;              // |EN| Secondary transform for wall detection |TR| Duvar algılama için ikincil transform
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    // |EN| Awake is called when the script instance is being loaded |TR| Awake, script örneği yüklendiğinde çağrılır
    private void Awake()
    {
        // |EN| Get references first because states might need them during initialization |TR| Referansları önce al çünkü state'ler başlatma sırasında onlara ihtiyaç duyabilir
        anim = GetComponentInChildren<Animator>(); 
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        // |EN| Initialize all player states |TR| Tüm player state'lerini başlat
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "JumpFall"); 
        fallState = new PlayerFallState(this, stateMachine, "JumpFall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "JumpFall");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        basicAttackState = new PlayerBasicAttackState(this, stateMachine, "BasicAttack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "JumpAttack");
    }

    // |EN| OnEnable is called when the object becomes enabled and active |TR| OnEnable, nesne etkinleştirildiğinde ve aktif olduğunda çağrılır
    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => movementInput = Vector2.zero;
    }
    
    // |EN| OnDisable is called when the behaviour becomes disabled or inactive |TR| OnDisable, davranış devre dışı bırakıldığında veya etkin olmadığında çağrılır
    private void OnDisable()
    {
        input.Disable();
    }

    // |EN| Start is called before the first frame update |TR| Start, ilk kare güncellemesinden önce çağrılır
    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    // |EN| Update is called once per frame |TR| Update, her karede bir çağrılır
    private void Update()
    {
        HandleCollisionDetection();        // |EN| Handle collision detection (e.g., ground check) |TR| Çarpışma algılama işlemini yönet (örneğin, zemin kontrolü)
        stateMachine.UpdateActiveState();  // |EN| Update the active state |TR| Aktif state'i güncelle
    }

    // |EN| Method to enter the attack state because of we need MonoBehaviour to start coroutine |TR| Coroutine başlatmak için MonoBehaviour'a ihtiyacımız olduğu için saldırı state'ine girmek için method 
    public void EnterAttackStateWithQueue()
    {
        // |EN| If there's an existing queued attack coroutine, stop it to avoid multiple queued attacks |TR| Mevcut bir sıraya alınan saldırı coroutine'i varsa, birden fazla sıraya alınan saldırıyı önlemek için durdur
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateAfterDelay()); // |EN| Start coroutine to enter attack state after a delay |TR| Gecikmeden sonra saldırı state'ine girmek için coroutine başlat
    }

    private IEnumerator EnterAttackStateAfterDelay()
    {
        yield return new WaitForEndOfFrame(); // |EN| Wait until the end of the frame to ensure input is registered |TR| Girdinin kaydedildiğinden emin olmak için kare sonuna kadar bekle
        stateMachine.ChangeState(basicAttackState);
        queuedAttackCoroutine = null; // |EN| Reset the coroutine reference after execution |TR| Yürütme sonrası coroutine referansını sıfırla
    }

    public void CallAnimationTrigger()
    {
        stateMachine.currentState.CallAnimationTrigger(); // |EN| Forward the call to the current state |TR| Çağrıyı mevcut state'e ilet
    }

    // |EN| Method to set the player's velocity |TR| Player'ın hızını ayarlamak için method
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // |EN| Set the Rigidbody2D's velocity |TR| Rigidbody2D'nin hızını ayarla
        HandleFlip(xVelocity); // |EN| Handle flipping the player based on movement direction |TR| Hareket yönüne göre player'ı çevirme işlemini yönet
    }

    // |EN| Method to handle flipping the player's facing direction based on x velocity |TR| X hızı temelinde player'ın bakış yönünü çevirme işlemini yönetmek için method
    private void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !facingRight)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    // |EN| Method to flip the player's facing direction |TR| Player'ın bakış yönünü çevirmek için method
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight; 
        facingDirection *= -1; // |EN| Invert facing direction |TR| Bakış yönünü tersine çevir
    }

    // |EN| Method to handle collision detection (e.g., ground check) |TR| Çarpışma algılama işlemini yönetmek (örneğin, zemin kontrolü)
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround); 

        // |EN| Check for wall detection using two raycasts for better accuracy |TR| Daha iyi doğruluk için iki ışın kullanarak duvar algılamasını kontrol et
        wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                    && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    // |EN| Method to visualize ground and wall check rays in the editor |TR| Editörde zemin ve duvar kontrol ışınlarını görselleştirmek için method
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0)); 
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
    }
}
