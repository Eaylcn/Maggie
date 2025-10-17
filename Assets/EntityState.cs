using System;
using UnityEngine;

public abstract class EntityState
{
    protected Player player;
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected PlayerInputSet input;

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

    public virtual void Enter()
    {
        // |EN| Eveytime state will be changed this method will be called |TR| Her state değiştiğinde bu method çağrılacak
        anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        // |EN| We are going to run logic of the state here |TR| State'in mantığını burada çalıştıracağız

        anim.SetFloat("yVelocity", rb.linearVelocity.y); // |EN| Update yVelocity parameter for animations |TR| Animasyonlar için yVelocity parametresini güncelle
    }
    
    public virtual void Exit()
    {
        // |EN| This method will be called when exiting the state |TR| Bu method state'den çıkarken çağrılacak
        anim.SetBool(animBoolName, false);
    }
}
