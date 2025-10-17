using UnityEngine;

public class PlayerFallState : PlayerOnAirState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Idle state if the player is grounded |TR| Player yere temas ediyorsa Idle state'ine geçiş yap
        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);

        // |EN| Transition to Wall Slide state if the player is touching a wall |TR| Player bir duvara temas ediyorsa Wall Slide state'ine geçiş yap
        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
