using UnityEngine;

public class PlayerBasicAttackState : EntityState
{
    private float attackMovementTimer;      // |EN| Timer to track duration of attack movement |TR| Saldırı hareketi süresini takip etmek için zamanlayıcı

    private const int FirstComboIndex = 1;  // |EN| First combo index that we use in the animator |TR| Animatörde kullandığımız ilk kombo indeksi
    private int comboIndex = 1;             // |EN| To track attack combos and also it starts from 1 |TR| Saldırı kombinasyonlarını takip etmek için ve ayrıca 1'den başlar
    private int comboLimit = 3;       // |EN| Maximum number of combos |TR| Maksimum kombinasyon sayısı

    private float lastTimeAttacked;      // |EN| Time when the last attack was performed |TR| Son saldırının gerçekleştirildiği zaman

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
        
        ResetComboIfNeeded();

        anim.SetInteger("BasicAttackIndex", comboIndex); // |EN| Set the attack combo index for animation |TR| Animasyon için saldırı kombinasyon indeksini ayarla
        ApplyAttackMovement();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackMovement();

        // |EN| Next Goal: Detect and hit enemies during attack
        // |TR| Sonraki Hedef: Saldırı sırasında düşmanları algıla ve vur

        // |EN| Transition back to Idle state when attack animation is over |TR| Saldırı animasyonu bittiğinde Idle state'ine geri dön
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAttacked = Time.time; // |EN| Update the last attacked time |TR| Son saldırı zamanını güncelle

        comboIndex++; // |EN| Increment combo index for next attack |TR| Sonraki saldırı için kombinasyon indeksini artır
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
        player.SetVelocity(attackMove.x * player.facingDirection, attackMove.y); // |EN| Apply attack movement based on facing direction |TR| Bakış yönüne göre saldırı hareketini uygula
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
