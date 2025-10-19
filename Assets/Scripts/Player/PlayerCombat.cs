using UnityEngine;

public class PlayerCombat : EntityCombat
{
    [Header("Counter-Attack Settings")]
    public float counterResetDuration = .1f; // |EN| Duration of the counter-attack state |TR| Karşı saldırı durumu süresi

    public bool CounterAttackPerformed()
    {
        bool counterAttackExecuted = false;

        // |EN| Implement counter-attack logic here |TR| Karşı saldırı mantığını buraya uygulayın
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>(); // |EN| Get the ICounterable component of the target |TR| Hedefin ICounterable bileşenini al

            if (counterable == null)
                continue; // |EN| If target is not counterable, skip to next target |TR| Hedef karşı saldırıya uğrayamıyorsa, bir sonraki hedefe geç
                
            if (counterable.CanBeCountered)
            {
                counterable.HandleCounterAttack(); // |EN| Invoke the counter-attack handling method on the target |TR| Hedef üzerindeki karşı saldırı işleme yöntemini çağır
                counterAttackExecuted = true;
            }
        }

        return counterAttackExecuted;
    }

    public float GetCounterResetDuration() => counterResetDuration;
}
