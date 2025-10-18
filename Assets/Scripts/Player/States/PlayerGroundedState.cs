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
        if (rb.linearVelocityY < 0f && !player.groundDetected) // |EN| Ensure the player is not grounded |TR| Player'ın yere temas etmediğinden emin ol
            stateMachine.ChangeState(player.fallState);

        // |EN| Transition to Jump state if jump input is detected |TR| Zıplama girdisi algılanırsa Jump state'ine geçiş yap
        if (input.Player.Jump.WasPerformedThisFrame())
            stateMachine.ChangeState(player.jumpState);

        // |EN| Transition to Attack state if attack input is detected |TR| Saldırı girdisi algılanırsa Attack state'ine geçiş yap
        if (input.Player.Attack.WasPerformedThisFrame())
            stateMachine.ChangeState(player.basicAttackState);
    }
}
