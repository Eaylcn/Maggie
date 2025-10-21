using UnityEngine;

public class PlayerDeadState : PlayerState
{
    EntityVFX vfx;

    public PlayerDeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        vfx = player.GetComponent<EntityVFX>();
    }

    public override void Enter()
    {
        base.Enter();

        input.Disable(); // |EN| Disable player input upon death |TR| Ölüm üzerine oyuncu girdisini devre dışı bırak
        rb.simulated = false; // |EN| Disable physics simulation to prevent further movement |TR| Daha fazla hareketi önlemek için fizik simülasyonunu devre dışı bırak
        stateMachine.SwitchOffStateMachine(); // |EN| Disable further state changes upon death |TR| Ölüm üzerine daha fazla durum değişikliğini devre dışı bırak
    }

    public override void Update()
    {
        base.Update();
        
        if (vfx.isAnyStatusVfxPlaying) 
            vfx.StopAllStatusVfx(); // |EN| Stop all status VFX upon death |TR| Ölümde tüm durum VFX'lerini durdur
    }
}
