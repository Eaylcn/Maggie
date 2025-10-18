using UnityEngine;

public class PlayerJumpAttackState : EntityState
{
    private bool touchedGround;

    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        touchedGround = false; // |EN| Reset the ground touch flag |TR| Yere temas bayrağını sıfırla

        player.SetVelocity(player.jumpAttackMovement.x * player.facingDirection, player.jumpAttackMovement.y); // |EN| Apply jump attack movement |TR| Zıplama saldırısı hareketini uygula
    }

    public override void Update()
    {
        base.Update();

        // |EN| Check if the player has touched the ground |TR| Oyuncunun yere değip değmediğini kontrol et
        if (player.groundDetected && !touchedGround)
        {
            touchedGround = true; // |EN| Mark that the player has touched the ground |TR| Oyuncunun yere değdiğini işaretle
            anim.SetTrigger("JumpAttackTrigger"); // |EN| Trigger the jump attack animation |TR| Zıplama saldırısı animasyonunu tetikle
            player.SetVelocity(0f, rb.linearVelocityY); // |EN| Maintain vertical velocity |TR| Dikey hızı koru
        }

        // |EN| Transition to idle state after the jump attack animation is triggered and the player is on the ground |TR| Zıplama saldırısı animasyonu tetiklendikten ve oyuncu yerdeyken boşta durma durumuna geçiş
        if (triggerCalled && player.groundDetected)
            stateMachine.ChangeState(player.idleState);

    }
}
