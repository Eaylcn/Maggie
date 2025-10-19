using UnityEngine;

public class EntityCombat : MonoBehaviour
{
    public float damage = 10f; // |EN| Damage dealt per attack |TR| Her saldırıda verilen hasar

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck; // |EN| Transform to check for targets |TR| Hedefleri kontrol etmek için Transform
    [SerializeField] private float targetCheckRadius = 1f; // |EN| Radius to check for targets |TR| Hedefleri kontrol etmek için yarıçap
    [SerializeField] private LayerMask whatIsTarget; // |EN| Layer mask to identify targets |TR| Hedefleri tanımlamak için katman maskesi

    public void PerformAttack()
    {
        // |EN| Implement attack logic here |TR| Saldırı mantığını buraya uygulayın
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>(); // |EN| Get the IDamageable component of the target |TR| Hedefin IDamageable bileşenini al
            damageable?.TakeDamage(damage, transform); // |EN| Deal damage to IDamageable targets |TR| IDamageable hedeflere hasar ver
        }
    }

    private Collider2D[] GetDetectedColliders()
    {
        // |EN| Detect targets within the specified radius |TR| Belirtilen yarıçap içinde hedefleri tespit et
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
