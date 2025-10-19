using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, damageDealer); // |EN| Call base method to apply damage |TR| Hasarı uygulamak için temel yöntemi çağır

        if (!wasHit) 
            return false; // |EN| If base method indicates no damage was taken, return false |TR| Temel yöntem hasar alınmadığını gösteriyorsa false döndür

        if (damageDealer.GetComponent<Player>() != null) // |EN| Check if the damage dealer is the player |TR| Hasar verenin oyuncu olup olmadığını kontrol et
        {
            // |EN| Try to enter battle state when taking damage |TR| Hasar alırken savaş durumuna girmeyi dene
            enemy.TryEnterBattleState(damageDealer);
        }

        return true; // |EN| Damage was successfully applied |TR| Hasar başarıyla uygulandı
    }
}
