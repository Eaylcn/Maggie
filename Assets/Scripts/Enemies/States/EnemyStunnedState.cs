using UnityEngine;

public class EnemyStunnedState : EnemyState
{
    private EnemyVFX enemyVFX;

    public EnemyStunnedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyVFX = enemy.GetComponent<EnemyVFX>();
    }

    public override void Enter()
    {
        base.Enter();

        enemyVFX.EnableAttackAlertVFX(false); // |EN| Disable the attack alert VFX when stunned |TR| Sersemletildiğinde saldırı uyarı VFX'sini devre dışı bırak
        enemy.EnableCounterWindow(false); // |EN| Disable the counter window for the enemy |TR| Düşman için karşı saldırı penceresini devre dışı bırak

        stateTimer = enemy.stunnedDuration; // |EN| Set timer for stunned duration |TR| Sersemletilmiş süre için zamanlayıcıyı ayarla

        // |EN| Apply knockback force when entering stunned state |TR| Sersemletilmiş duruma girerken geri tepme kuvveti uygula
        rb.linearVelocity = new Vector2(-enemy.facingDirection * enemy.stunnedKnockbackForce.x, enemy.stunnedKnockbackForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.idleState); // |EN| Transition to idle state after stunned duration ends |TR| Sersemletilmiş süre sona erdikten sonra boşta duruma geçiş yap
        }
    }
}
