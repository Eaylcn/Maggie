using UnityEngine;

public class PlayerWallJumpState : EntityState
{
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // |EN| Apply wall jump velocity away from the wall |TR| Duvarın uzağına doğru wall jump hızı uygula
        player.SetVelocity(player.wallJumpForce.x * -player.facingDirection, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Fall state after wall jump is initiated |TR| Wall jump başlatıldıktan sonra Fall state'ine geçiş yap
        if (rb.linearVelocityY < 0f)
            stateMachine.ChangeState(player.fallState);

        // |EN| Transition to Wall Slide state if the player is touching a wall again |TR| Player tekrar bir duvara temas ediyorsa Wall Slide state'ine geçiş yap
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);

        // |EN| Transition to jump attack state if attack input is pressed while jumping off the wall |TR| Duvar zıplaması yaparken saldırı girişi yapılırsa jump attack state'ine geçiş yap
        if (input.Player.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.jumpAttackState);
    }
}
