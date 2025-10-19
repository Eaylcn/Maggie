using UnityEngine;

public class EnemySkeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => canbeStunned; } // |EN| Property to check if enemy can be countered (stunned) |TR| Düşmanın karşı saldırıya (sersemletmeye) uğrayıp uğrayamayacağını kontrol etmek için özellik
    
    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        moveState = new EnemyMoveState(this, stateMachine, "Move");
        attackState = new EnemyAttackState(this, stateMachine, "Attack");
        battleState = new EnemyBattleState(this, stateMachine, "Battle");
        deadState = new EnemyDeadState(this, stateMachine, "Dead");
        stunnedState = new EnemyStunnedState(this, stateMachine, "Stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    // |EN| Implementation of ICounterable interface method to handle counter-attack behavior |TR| Karşı saldırı davranışını yönetmek için ICounterable arayüzü yönteminin uygulanması
    public void HandleCounterAttack()
    {
        if (!CanBeCountered) return; // |EN| If enemy cannot be stunned, exit method |TR| Düşman sersemleyemiyorsa, yöntemden çık

        stateMachine.ChangeState(stunnedState);
    }
}
