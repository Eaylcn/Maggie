using UnityEngine;

public class PlayerGroundedState : EntityState
{
    public PlayerGroundedState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Fall state if vertical velocity is negative (falling) |TR| Düşme hızı negatifse (düşüyorsa) Fall state'ine geçiş yap
        if (rb.linearVelocityY < 0f)
            stateMachine.ChangeState(player.fallState);

        // |EN| Transition to Jump state if jump input is detected |TR| Zıplama girdisi algılanırsa Jump state'ine geçiş yap
        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);
    }
}
