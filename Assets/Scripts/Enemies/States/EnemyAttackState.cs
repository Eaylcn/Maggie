using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
