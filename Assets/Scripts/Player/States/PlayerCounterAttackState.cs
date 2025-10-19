using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private PlayerCombat combat;
    private bool counteredSuccessfully;

    public PlayerCounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PlayerCombat>();
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = combat.GetCounterResetDuration(); // |EN| Set the state timer based on counter-attack duration |TR| Karşı saldırı süresine göre durum zamanlayıcısını ayarla
        counteredSuccessfully = combat.CounterAttackPerformed(); // |EN| Attempt to perform counter-attack and store success status |TR| Karşı saldırıyı gerçekleştirmeye çalış ve başarı durumunu sakla
        anim.SetBool("CounterAttackPerformed", counteredSuccessfully); // |EN| Set animation parameter based on whether counter-attack was successful |TR| Karşı saldırının başarılı olup olmadığına göre animasyon parametresini ayarla
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0f, rb.linearVelocityY); // |EN| Maintain vertical velocity while stopping horizontal movement |TR| Yatay hareketi durdururken dikey hızı koru

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState); // |EN| Transition to Idle state if animation trigger was called |TR| Animasyon tetikleyicisi çağrıldıysa Idle durumuna geçiş yap

        if (stateTimer <= 0f && !counteredSuccessfully)
            stateMachine.ChangeState(player.idleState);
    }
}
