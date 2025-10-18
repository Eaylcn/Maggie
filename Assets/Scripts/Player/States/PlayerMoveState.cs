using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Idle state if there is no movement input or if moving into a wall |TR| Hareket girdisi yoksa veya bir duvara doğru hareket ediliyorsa Idle state'ine geçiş yap
        if (player.movementInput.x == 0 || player.wallDetected)
        {
            stateMachine.ChangeState(player.idleState);
        }

        // |EN| Set the player's velocity based on movement input and move speed |TR| Hareket girdisi ve hareket hızına göre player'ın hızını ayarla
        player.SetVelocity(player.movementInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
