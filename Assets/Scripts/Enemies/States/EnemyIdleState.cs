using UnityEngine;

public class EnemyIdleState : EnemyGroundedState
{
    public EnemyIdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleDuration; // |EN| Set timer for idle duration |TR| Boşta kalma süresi için zamanlayıcıyı ayarla
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0f)
        {
            stateMachine.ChangeState(enemy.moveState); // |EN| Transition to move state after idle duration |TR| Boşta kalma süresinden sonra hareket durumuna geçiş yap
        }
    }
}
