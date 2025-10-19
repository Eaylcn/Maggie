using UnityEngine;

public class EnemyGroundedState : EnemyState
{
    public EnemyGroundedState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        // |EN| Transition to battle state if player is detected |TR| Oyuncu algılanırsa savaş durumuna geçiş yap
        if (enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.battleState);
    }
}
