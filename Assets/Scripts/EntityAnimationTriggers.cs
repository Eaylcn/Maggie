using UnityEngine;

/*
 * This script handles animation events for the Entity.
 * Attach this script to the same GameObject that has the Animator component.
 * Animation events can call the methods in this script to notify the Entity script about animation states.
 * For example, when an attack animation finishes, it can call the AttackOver() method below.
 * Make sure to set up the animation events in the Animator window in Unity.
 */

/*
 * Bu script, Entity için animasyon olaylarını yönetir.
 * Bu script'i Animator bileşenine sahip aynı GameObject'e ekleyin.
 * Animasyon olayları, bu script'teki yöntemleri çağırarak Entity script'ine animasyon durumları hakkında bildirimde bulunabilir.
 * Örneğin, bir saldırı animasyonu bittiğinde, aşağıdaki AttackOver() yöntemini çağırabilir.
 * Unity'deki Animator penceresinde animasyon olaylarını ayarladığınızdan emin olun.
 */

public class EntityAnimationTriggers : MonoBehaviour
{
    private Entity entity;
    private EntityCombat entityCombat;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>(); // |EN| Get reference to the Entity script in parent GameObject |TR| Üst GameObject'teki Entity script'ine referans al
        entityCombat = GetComponentInParent<EntityCombat>(); // |EN| Get reference to the EntityCombat script in parent GameObject |TR| Üst GameObject'teki EntityCombat script'ine referans al
    }

    public void CurrentStateTrigger()
    {
        entity.TriggerCurrentStateAnimation(); // |EN| Forward the call to the Entity script |TR| Çağrıyı Entity script'ine ilet
    }

    private void AttackTrigger()
    {
        entityCombat.PerformAttack(); // |EN| Call the PerformAttack method in EntityCombat script |TR| EntityCombat script'inde PerformAttack yöntemini çağır
    }
}
