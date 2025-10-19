using UnityEngine;

public class EnemyMoveState : EnemyGroundedState
{
    public EnemyMoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // |EN| Flip enemy direction if no ground is detected or a wall is hit upon entering move state |TR| Hareket durumuna girerken zemin algılanmazsa veya bir duvara çarpılırsa düşman yönünü değiştir
        if (!enemy.groundDetected || enemy.wallDetected)
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        // |EN| Example movement logic for the enemy |TR| Düşman için örnek hareket mantığı
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDirection, rb.linearVelocityY);

        // |EN| Transition to idle state if no ground is detected ahead |TR| İleri doğru zemin algılanmazsa idle state'ine geçiş yap
        if (!enemy.groundDetected || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }
}
