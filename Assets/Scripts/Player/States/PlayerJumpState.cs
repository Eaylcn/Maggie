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

        // |EN| Transition to fall state if the player is descending and not in jump attack state |TR| Oyuncu alçalıyorsa ve zıplama saldırısı durumunda değilse düşme durumuna geç
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}
