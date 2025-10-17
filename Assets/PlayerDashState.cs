using UnityEngine;

public class PlayerDashState : EntityState
{
    private float originalGravityScale;
    private int dashDirection; // |EN| Direction of the dash for safety checks |TR| Güvenlik kontrolleri için dash yönü

    public PlayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        dashDirection = player.facingDirection; // |EN| Store dash direction at the start |TR| Başlangıçta dash yönünü sakla
        stateTimer = player.dashDuration; // |EN| Initialize dash duration timer |TR| Dash süresi zamanlayıcısını başlat

        originalGravityScale = rb.gravityScale; // |EN| Store original gravity scale |TR| Orijinal yerçekimi ölçeğini sakla
        rb.gravityScale = 0f; // |EN| Disable gravity during dash |TR| Dash sırasında yerçekimini devre dışı bırak
    }

    public override void Update()
    {
        base.Update();
        CancelDashIfNeeded();
        player.SetVelocity(player.dashSpeed * dashDirection, 0f); // |EN| Set dash velocity |TR| Dash hızını ayarla

        // |EN| Check if dash duration is over |TR| Dash süresinin bitip bitmediğini kontrol et
        if (stateTimer <= 0f)
        {
            // |EN| Transition to appropriate state after dash |TR| Dash sonrası uygun state'e geçiş yap
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0f, 0f); // |EN| Stop dash movement |TR| Dash hareketini durdur
        rb.gravityScale = originalGravityScale; // |EN| Restore original gravity scale |TR| Orijinal yerçekimi ölçeğini geri yükle
    }

    private void CancelDashIfNeeded()
    {
        // |EN| If player collides with wall during dash, cancel dash |TR| Player dash sırasında duvara çarparsa dash'i iptal et
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.wallSlideState);
        }
    }
}
