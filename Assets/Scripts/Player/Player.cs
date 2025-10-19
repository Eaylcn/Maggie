using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }
    
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
    public Vector2[] attackMovement;             // |EN| Movement force vectors applied during each attack combo sequence |TR| Her saldırı kombo sekansı sırasında uygulanan hareket kuvveti vektörleri
    public Vector2 jumpAttackMovement;           // |EN| Movement force vector applied specifically during jump attack |TR| Zıplama saldırısı sırasında özel olarak uygulanan hareket kuvveti vektörü
    public float attackMovementDuration = 0.1f;  // |EN| Duration in seconds for how long attack movement force is applied |TR| Saldırı hareket kuvvetinin ne kadar süre uygulandığının saniye cinsinden süresi
    public float comboResetTime = 1.0f;          // |EN| Time in seconds before attack combo counter resets to zero |TR| Saldırı kombo sayacının sıfıra sıfırlanmadan önceki saniye cinsinden süre
    private Coroutine queuedAttackCoroutine;     // |EN| Coroutine reference for managing delayed attack state transitions |TR| Gecikmeli saldırı durum geçişlerini yönetmek için Coroutine referansı

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public Vector2 wallJumpForce;
    
    [Range(0f, 1f)]
    public float airMoveMultiplier = 0.8f;                  // |EN| Movement speed multiplier when in air (0-1 range for reduced air control) |TR| Havadayken hareket hızı çarpanı (azaltılmış hava kontrolü için 0-1 aralığı)
    [Range(0f, 1f)]
    public float wallSlideSlowdownFactor = 0.3f;            // |EN| Factor to reduce falling speed during wall slide (0-1 range) |TR| Duvar kayması sırasında düşme hızını azaltma faktörü (0-1 aralığı)
    [Space]
    public float dashDuration = 0.25f;                      // |EN| Total duration of dash movement in seconds |TR| Dash hareketinin saniye cinsinden toplam süresi
    public float dashSpeed = 20f;                           // |EN| Movement speed during dash execution |TR| Dash yürütme sırasındaki hareket hızı
    public Vector2 movementInput { get; private set; }      // |EN| Current player input for horizontal and vertical movement |TR| Yatay ve dikey hareket için mevcut oyuncu girdisi

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet(); // |EN| Create new instance of player input system |TR| Oyuncu girdi sisteminin yeni örneğini oluştur

        // |EN| Initialize all player state instances with references to this player and state machine |TR| Tüm oyuncu durum örneklerini bu oyuncu ve durum makinesi referanslarıyla başlat
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

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }
    
    // |EN| Initiates attack state transition with coroutine queue system for smooth input handling |TR| Pürüzsüz girdi işleme için coroutine kuyruk sistemi ile saldırı durumu geçişini başlatır
    public void EnterAttackStateWithQueue()
    {
        // |EN| Cancel any existing queued attack to prevent multiple simultaneous attack commands |TR| Birden fazla eşzamanlı saldırı komutunu önlemek için mevcut sıraya alınan saldırıyı iptal et
        if (queuedAttackCoroutine != null)
            StopCoroutine(queuedAttackCoroutine);

        queuedAttackCoroutine = StartCoroutine(EnterAttackStateAfterDelay()); // |EN| Start delayed attack state transition coroutine |TR| Gecikmeli saldırı durumu geçiş coroutine'ini başlat
    }

    private IEnumerator EnterAttackStateAfterDelay()
    {
        yield return new WaitForEndOfFrame(); // |EN| Wait one frame to ensure all input processing is complete |TR| Tüm girdi işlemesinin tamamlandığından emin olmak için bir kare bekle
        stateMachine.ChangeState(basicAttackState);
        queuedAttackCoroutine = null; // |EN| Clear coroutine reference to indicate completion |TR| Tamamlanmayı belirtmek için coroutine referansını temizle
    }

    // |EN| Called when GameObject becomes active, enables input system and sets up input callbacks |TR| GameObject aktif olduğunda çağrılır, girdi sistemini etkinleştirir ve girdi geri çağrılarını kurar
    private void OnEnable()
    {
        input.Enable();

        input.Player.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => movementInput = Vector2.zero;
    }
    
    // |EN| Called when GameObject becomes inactive, disables input system to prevent memory leaks |TR| GameObject pasif olduğunda çağrılır, bellek sızıntılarını önlemek için girdi sistemini devre dışı bırakır
    private void OnDisable()
    {
        input.Disable();
    }
}
