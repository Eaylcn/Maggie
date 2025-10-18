using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // |EN| Set horizontal velocity to zero when entering Idle state |TR| Idle state'ine girerken yatay hızı sıfırla
        player.SetVelocity(0f, rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();

        // |EN| Prevent transitioning to Move state if player is trying to move into a wall |TR| Oyuncu bir duvara doğru hareket etmeye çalışıyorsa Move state'ine geçişi engelle
        if (player.movementInput.x == player.facingDirection && player.wallDetected)
            return;

        // |EN| Transition to Move state if there is movement input |TR| Hareket girdisi varsa Move state'ine geçiş yap
        if (player.movementInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
