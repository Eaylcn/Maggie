using UnityEngine;

/*
 * Player state for handling basic attack combos.
 * Supports queuing attacks for combo execution.
 * Manages attack movement and animation triggers.
 * Resets combo if time between attacks exceeds a threshold.
 * Designed to integrate with the existing Player and StateMachine architecture.
 * Assumes Player class has necessary properties like attackMovement, comboResetTime, etc.
 * Animation events should call CallAnimationTrigger to signal attack completion.
 * Ensure to set up animation parameters and transitions in the Animator for proper functioning.
 */

/*
 * Temel saldırı kombinasyonlarını yöneten oyuncu durumu.
 * Kombo yürütme için saldırıların sıraya alınmasını destekler.
 * Saldırı hareketini ve animasyon tetikleyicilerini yönetir.
 * Saldırılar arasındaki süre eşik değeri aşarsa komboyu sıfırlar.
 * Mevcut Player ve StateMachine mimarisi ile entegre olacak şekilde tasarlanmıştır.
 * Player sınıfının attackMovement, comboResetTime vb. gibi gerekli özelliklere sahip olduğunu varsayar.
 * Animasyon olayları, saldırı tamamlanmasını bildirmek için CallAnimationTrigger'ı çağırmalıdır.
 * Doğru çalışması için Animator'da animasyon parametrelerini ve geçişleri ayarlamayı unutmayın.
 */


public class PlayerBasicAttackState : EntityState
{
    private float attackMovementTimer;      // |EN| Timer to track duration of attack movement |TR| Saldırı hareketi süresini takip etmek için zamanlayıcı
    private float lastTimeAttacked;      // |EN| Time when the last attack was performed |TR| Son saldırının gerçekleştirildiği zaman

    private bool comboAttackQueued;   // |EN| Flag to check if a combo attack is queued |TR| Bir kombo saldırısının sıraya alınıp alınmadığını kontrol etmek için bayrak
    private int attackDirection;      // |EN| Direction of the attack based on player facing |TR| Oyuncunun baktığı yöne göre saldırı yönü  
    private int comboIndex = 1;             // |EN| To track attack combos and also it starts from 1 |TR| Saldırı kombinasyonlarını takip etmek için ve ayrıca 1'den başlar
    private int comboLimit = 3;       // |EN| Maximum number of combos |TR| Maksimum kombinasyon sayısı
    private const int FirstComboIndex = 1;  // |EN| First combo index that we use in the animator |TR| Animatörde kullandığımız ilk kombo indeksi

    public PlayerBasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackMovement.Length)
        {
            Debug.LogWarning("Combo limit does not match the length of attack movement array. Adjusting combo limit.");
            comboLimit = player.attackMovement.Length; // |EN| Adjust combo limit to match the length of attack movement array |TR| Saldırı hareketi dizisinin uzunluğuna göre kombo limitini ayarla
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false; // |EN| Reset combo attack queued flag |TR| Kombo saldırı sıraya alındı bayrağını sıfırla
        ResetComboIfNeeded();

        // |EN| Determine attack direction based on movement input or facing direction |TR| Hareket girdisine veya bakış yönüne göre saldırı yönünü belirle
        attackDirection = player.movementInput.x != 0 ? (int)player.movementInput.x : player.facingDirection;

        anim.SetInteger("BasicAttackIndex", comboIndex); // |EN| Set the attack combo index for animation |TR| Animasyon için saldırı kombinasyon indeksini ayarla
        ApplyAttackMovement();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackMovement();

        // |EN| Next Goal: Detect and hit enemies during attack
        // |TR| Sonraki Hedef: Saldırı sırasında düşmanları algıla ve vur

        // |EN| Queue combo attack if attack input is pressed during current attack |TR| Mevcut saldırı sırasında saldırı girişi yapılırsa kombo saldırısını sıraya al
        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        // |EN| If animation trigger has been called, handle state exit |TR| Animasyon tetikleyicisi çağrıldıysa, state çıkışını yönet
        if (triggerCalled)
            HandleStateExit();
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time; // |EN| Update the last attacked time |TR| Son saldırı zamanını güncelle

        comboIndex++; // |EN| Increment combo index for next attack |TR| Sonraki saldırı için kombinasyon indeksini artır
    }

    // |EN| Handle exiting the attack state and transitioning to next state |TR| Saldırı state'inden çıkmayı ve bir sonraki state'e geçişi yönet
    private void HandleStateExit()
    {
        // |EN| Transition to next state based on whether a combo attack is queued |TR| Bir kombo saldırısı sıraya alınıp alınmadığına bağlı olarak bir sonraki state'e geçiş yap
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false); // |EN| Reset current attack animation bool |TR| Mevcut saldırı animasyonu bool'unu sıfırla
            player.EnterAttackStateWithQueue(); // |EN| Enter attack state again for combo |TR| Kombo için tekrar saldırı state'ine gir
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    // |EN| Queue the next attack if within combo limits |TR| Kombo sınırları içindeyse bir sonraki saldırıyı sıraya al
    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    // |EN| Handle movement during the attack state |TR| Saldırı state'indeki hareketi yönet
    private void HandleAttackMovement()
    {
        attackMovementTimer -= Time.deltaTime;

        if (attackMovementTimer < 0f)
            player.SetVelocity(0f, rb.linearVelocity.y);
    }

    // |EN| Apply attack movement when entering attack state |TR| Saldırı state'ine girerken saldırı hareketini uygula
    private void ApplyAttackMovement()
    {
        Vector2 attackMove = player.attackMovement[comboIndex - 1]; // |EN| Get attack movement based on current combo index |TR| Mevcut kombinasyon indeksine göre saldırı hareketini al

        attackMovementTimer = player.attackMovementDuration; // |EN| Set timer for attack movement duration |TR| Saldırı hareketi süresi için zamanlayıcı ayarla
        player.SetVelocity(attackMove.x * attackDirection, attackMove.y); // |EN| Set player velocity based on attack movement and direction |TR| Saldırı hareketi ve yönüne göre player hızını ayarla
    }

    // |EN| Reset combo index if needed |TR| Gerekirse kombo indeksini sıfırla
    private void ResetComboIfNeeded()
    {
        // |EN| Reset combo if time since last attack exceeds combo reset time |TR| Son saldırıdan bu yana geçen süre kombo sıfırlama süresini aşarsa komboyu sıfırla
        if (Time.time - lastTimeAttacked > player.comboResetTime)
            comboIndex = FirstComboIndex;

        // |EN| Ensure combo index does not exceed limit |TR| Kombinasyon indeksinin sınırı aşmadığından emin ol
        if (comboIndex > comboLimit)
            comboIndex = FirstComboIndex;
    }
}
