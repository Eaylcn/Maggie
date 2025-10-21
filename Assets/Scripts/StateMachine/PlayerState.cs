using UnityEngine;

/*
 * An abstract base class that defines the structure and common logic for all player states.
 * Each derived player state (e.g., idle, move, attack) inherits from this class and overrides its methods.
 * Handles animation flags, timers, and references to core components such as Animator, Rigidbody2D, and PlayerInputSet.
 * Integrates with the StateMachine class to manage player state transitions.
 */

/*
 * Tüm player state'leri için yapı ve ortak mantığı tanımlayan soyut bir temel sınıf.
 * Her türetilmiş player state'i (örneğin idle, move, attack) bu sınıftan miras alır ve metodlarını geçersiz kılar.
 * Animasyon bayraklarını, zamanlayıcıları ve Animator, Rigidbody2D, PlayerInputSet gibi temel bileşenlere olan referansları yönetir.
 * StateMachine sınıfı ile entegre olarak player state geçişlerini yönetir.
 */

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;

    // |EN| Constructor to initialize the player state with player reference, state machine and animation name |TR| Player state'ini player referansı, state machine ve animasyon adı ile başlatmak için constructor
    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        // |EN| Cache frequently used player components for better performance |TR| Daha iyi performans için sık kullanılan player bileşenlerini önbelleğe al
        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        
        anim.SetFloat("yVelocity", rb.linearVelocity.y); // |EN| Update vertical velocity parameter for player animations |TR| Player animasyonları için dikey hız parametresini güncelle
    }

    private bool CanDash()
    {
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true; // |EN| Allow dash if player is not already dashing and not against a wall |TR| Player zaten dash yapmıyorsa ve duvara karşı değilse dash yapmasına izin ver
    }
}
