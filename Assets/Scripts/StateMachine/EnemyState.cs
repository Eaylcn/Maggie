using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;

    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        // |EN| Cache frequently used enemy components for better performance |TR| Daha iyi performans için sık kullanılan enemy bileşenlerini önbelleğe al
        anim = enemy.anim;
        rb = enemy.rb;
    }

    override public void UpdateAnimationParameters()
    {
        float battleAnimSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed; // |EN| Calculate animation speed multiplier based on battle movement speed |TR| Savaş hareket hızına dayalı animasyon hızı çarpanını hesapla

        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier); // |EN| Update movement animation speed multiplier |TR| Hareket animasyonu hız çarpanını güncelle
        anim.SetFloat("battleAnimSpeedMultiplier", battleAnimSpeedMultiplier); // |EN| Update battle animation speed multiplier |TR| Savaş animasyonu hız çarpanını güncelle
        anim.SetFloat("xVelocity", rb.linearVelocityX); // |EN| Update horizontal velocity parameter for enemy animations |TR| Düşman animasyonları için yatay hız parametresini güncelle
    }
}
