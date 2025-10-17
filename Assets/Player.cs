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

    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;

    [Range(0f, 1f)]
    public float airMoveMultiplier = 0.7f; // |EN| Should be between 0 and 1 |TR| 0 ile 1 arasında olmalı
    private bool facingRight = true;
    public Vector2 movementInput { get; private set; }

    [Header("Collision Detection Settings")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    public bool groundDetected { get; private set; }

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
    private void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }

    // |EN| Method to handle collision detection (e.g., ground check) |TR| Çarpışma algılama işlemini yönetmek (örneğin, zemin kontrolü)
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    // |EN| Gizmos to visualize ground check in the editor |TR| Editörde zemin kontrolünü görselleştirmek için Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
    }
}
