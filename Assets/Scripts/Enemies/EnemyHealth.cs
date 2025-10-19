using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();

    public override void TakeDamage(float damage, Transform damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        if (isDead) return; // |EN| If already dead, do nothing further |TR| Zaten ölü ise, daha fazla bir şey yapma

        if (damageDealer.GetComponent<Player>() != null) // |EN| Check if the damage dealer is the player |TR| Hasar verenin oyuncu olup olmadığını kontrol et
        {
            // |EN| Try to enter battle state when taking damage |TR| Hasar alırken savaş durumuna girmeyi dene
            enemy.TryEnterBattleState(damageDealer);
        }
    }
}
