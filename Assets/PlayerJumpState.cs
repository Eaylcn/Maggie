using UnityEngine;

public class PlayerJumpState : PlayerOnAirState
{
    public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // |EN| Set the player's vertical velocity to the jump force |TR| Player'ın dikey hızını zıplama gücüne ayarla
        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Fall state if the player is falling |TR| Player düşüyorsa Fall state'ine geçiş yap
        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}
