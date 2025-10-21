using UnityEngine;

public interface IDamageable
{
    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageSource);
}
