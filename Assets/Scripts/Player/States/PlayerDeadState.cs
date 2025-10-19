using UnityEngine;

public class PlayerDeadState : PlayerState
{
    EnemyVFX enemyVFX;

    public PlayerDeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        enemyVFX = player.GetComponentInChildren<EnemyVFX>();
    }

    public override void Enter()
    {
        base.Enter();

        input.Disable(); // |EN| Disable player input upon death |TR| Ölüm üzerine oyuncu girdisini devre dışı bırak
        rb.simulated = false; // |EN| Disable physics simulation to prevent further movement |TR| Daha fazla hareketi önlemek için fizik simülasyonunu devre dışı bırak
        stateMachine.SwitchOffStateMachine(); // |EN| Disable further state changes upon death |TR| Ölüm üzerine daha fazla durum değişikliğini devre dışı bırak
        enemyVFX.EnableAttackAlertVFX(false); // |EN| Disable any active attack alert VFX |TR| Aktif saldırı uyarı VFX'sini devre dışı bırak
    }
}
