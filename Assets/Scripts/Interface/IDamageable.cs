using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(float damage, Transform damageSource);
}
