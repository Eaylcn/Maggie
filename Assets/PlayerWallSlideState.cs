using UnityEngine;

public class PlayerWallSlideState : EntityState
{
    public PlayerWallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        HandleWallSlide();
        
        // |EN| Transition to Wall Jump state if jump input is detected |TR| Zıplama girdisi algılanırsa Wall Jump state'ine geçiş yap
        if (input.Player.Jump.WasPressedThisFrame())
            stateMachine.ChangeState(player.wallJumpState);

        // |EN| Transition to Fall state if the player is no longer touching the wall |TR| Player artık duvara temas etmiyorsa Fall state'ine geçiş yap
        if (!player.wallDetected)
            stateMachine.ChangeState(player.fallState);

        // |EN| Transition to Idle state if the player is grounded |TR| Player yere temas ediyorsa Idle state'ine geçiş yap
        if (player.groundDetected)
        {
            stateMachine.ChangeState(player.idleState);
            player.Flip(); // |EN| Flip the player to face away from the wall when landing |TR| Player yere indiğinde duvardan uzaklaşacak şekilde döndür
        }
    }

    // |EN| Method to handle wall slide mechanics (e.g., reduced fall speed) |TR| Duvar kayma mekaniğini yönetmek için method (örneğin, azalmış düşme hızı)
    private void HandleWallSlide()
    {
        if (player.movementInput.y < 0)
            player.SetVelocity(player.movementInput.x, rb.linearVelocity.y); // |EN| Maintain current fall speed when pressing down |TR| Aşağı basıldığında mevcut düşme hızını koru
        else
            player.SetVelocity(player.movementInput.x, rb.linearVelocity.y * player.wallSlideSlowdownFactor); // |EN| Slow down the fall speed when not pressing down |TR| Aşağı basılmadığında düşme hızını yavaşlat
    }
}
