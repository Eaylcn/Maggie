using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    // |EN| Cached commonly used components |TR| Önbelleğe alınmış sık kullanılan bileşenler
    protected Animator anim;
    protected Rigidbody2D rb;
    protected EntityStats stats;

    protected float stateTimer; // |EN| Timer to track duration in the state |TR| State'deki süreyi takip etmek için zamanlayıcı
    protected bool triggerCalled; // |EN| Flag to ensure triggers are called on animation events |TR| Animasyon olaylarında tetikleyicilerin çağrıldığından emin olmak için bayrak

    // |EN| Constructor to initialize the state with player, state machine and state name |TR| State'i player, state machine ve state adı ile başlatmak için constructor
    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
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
        UpdateAnimationParameters();   // |EN| Update any state-specific animation parameters |TR| Herhangi bir state'e özgü animasyon parametrelerini güncelle
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

    public virtual void UpdateAnimationParameters()
    {
        // |EN| Override in derived states to update specific animation parameters |TR| Belirli animasyon parametrelerini güncellemek için türetilmiş state'lerde geçersiz kılın
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offensiveStats.attackSpeed.GetValue();
        anim.SetFloat("AttackSpeedMultiplier", attackSpeed); // |EN| Update attack speed parameter in animations |TR| Animasyonlarda saldırı hızı parametresini güncelle
    }
}
