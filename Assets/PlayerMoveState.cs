using UnityEngine;

public class PlayerMoveState : EntityState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();

        // |EN| Transition to Idle state if there is no movement input |TR| Hareket girdisi yoksa Idle state'ine geçiş yap
        if (player.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
