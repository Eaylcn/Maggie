using System;
using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    // |EN| Cached commonly used components |TR| Önbelleğe alınmış sık kullanılan bileşenler
    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

    protected float stateTimer; // |EN| Timer to track duration in the state |TR| State'deki süreyi takip etmek için zamanlayıcı
    protected bool triggerCalled; // |EN| Flag to ensure triggers are called on animation events |TR| Animasyon olaylarında tetikleyicilerin çağrıldığından emin olmak için bayrak

    // |EN| Constructor to initialize the state with player, state machine and state name |TR| State'i player, state machine ve state adı ile başlatmak için constructor
    public EntityState(Player player, StateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;

        // |EN| Cache commonly used components |TR| Sık kullanılan bileşenleri önbelleğe al
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    // |EN| Eveytime state will be changed this method will be called |TR| Her state değiştiğinde bu method çağrılacak
    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false; // |EN| Reset trigger flag on state enter |TR| State girişinde tetikleyici bayrağını sıfırla
    }

    // |EN| Update method to be called every frame while in this state |TR| Bu state'deyken her karede çağrılacak Update methodu
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime; // |EN| Decrease state timer |TR| State zamanlayıcısını azalt

        anim.SetFloat("yVelocity", rb.linearVelocity.y); // |EN| Update yVelocity parameter for animations |TR| Animasyonlar için yVelocity parametresini güncelle

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    // |EN| This method will be called when exiting the state |TR| Bu method state'den çıkarken çağrılacak
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    // |EN| Method to be called by animation events to notify the state of triggers |TR| State'e tetikleyiciler hakkında bildirmek için animasyon olayları tarafından çağrılacak method
    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    
    private bool CanDash()
    {
        if (player.wallDetected)
            return false;

        if (stateMachine.currentState == player.dashState)
            return false;

        return true; // |EN| Can dash if not already dashing and not against a wall |TR| Zaten dash yapmıyorsa ve duvara karşı değilse dash yapılabilir
    }
}
