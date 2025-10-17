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

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public Vector2 wallJumpForce;

    [Range(0f, 1f)]
    public float airMoveMultiplier = 0.8f;                  // |EN| Should be between 0 and 1 |TR| 0 ile 1 arasında olmalı
    
    [Range(0f, 1f)]
    public float wallSlideSlowdownFactor = 0.3f;            // |EN| Factor to slow down fall speed during wall slide |TR| Duvar kayması sırasında düşme hızını yavaşlatma faktörü
    private bool facingRight = true;                        // |EN| True if facing right, false if facing left |TR| Sağ bakıyorsa true, sol bakıyorsa false
    public int facingDirection { get; private set; } = 1;   // |EN| 1 for right, -1 for left |TR| Sağ için 1, sol için -1
    public Vector2 movementInput { get; private set; }

    [Header("Collision Detection Settings")]
    [SerializeField] private float groundCheckDistance = 1.4f;  // |EN| Distance for ground detection raycast |TR| Zemin algılama ışın mesafesi
    [SerializeField] private float wallCheckDistance = 0.4f;    // |EN| Distance for wall detection raycast |TR| Duvar algılama ışın mesafesi
    [SerializeField] private LayerMask whatIsGround;            // |EN| LayerMask to define what is considered ground |TR| Hangi katmanın zemin olarak kabul edileceğini tanımlamak için LayerMask
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

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "JumpFall"); 
        fallState = new PlayerFallState(this, stateMachine, "JumpFall");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "JumpFall");
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
        wallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }

    // |EN| Method to visualize ground and wall check rays in the editor |TR| Editörde zemin ve duvar kontrol ışınlarını görselleştirmek için method
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));                
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(wallCheckDistance * facingDirection, 0)); 
    }
}
