using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputSet input;
    public StateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public Vector2 movementInput { get; private set; }

    // |EN| Awake is called when the script instance is being loaded |TR| Awake, script örneği yüklendiğinde çağrılır
    private void Awake()
    {
        stateMachine = new StateMachine();
        input = new PlayerInputSet();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
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
        stateMachine.currentState.Update();
    }
}
