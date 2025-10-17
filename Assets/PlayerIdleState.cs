using UnityEngine;

public class PlayerIdleState : EntityState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        // |EN| Transition to Move state if there is movement input |TR| Hareket girdisi varsa Move state'ine geçiş yap
        if (player.movementInput != Vector2.zero)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }
}
