using UnityEngine;

public class PlayerOnAirState : EntityState
{
    public PlayerOnAirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // |EN| Allow horizontal movement while in the air |TR| Havada yatay hareket izni ver
        if (player.movementInput.x != 0)
            player.SetVelocity(player.movementInput.x * (player.moveSpeed * player.airMoveMultiplier), rb.linearVelocity.y);

        // |EN| Transition to jump attack state if attack input is pressed while in air |TR| Havadayken saldırı girişi yapılırsa jump attack state'ine geçiş yap
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}
