using UnityEngine;

/*
 * This script handles animation events for the Player.
 * Attach this script to the same GameObject that has the Animator component.
 * Animation events can call the methods in this script to notify the Player script about animation states.
 * For example, when an attack animation finishes, it can call the AttackOver() method below.
 * Make sure to set up the animation events in the Animator window in Unity.
 */

/*
 * Bu script, Player için animasyon olaylarını yönetir.
 * Bu script'i Animator bileşenine sahip aynı GameObject'e ekleyin.
 * Animasyon olayları, bu script'teki yöntemleri çağırarak Player script'ine animasyon durumları hakkında bildirimde bulunabilir.
 * Örneğin, bir saldırı animasyonu bittiğinde, aşağıdaki AttackOver() yöntemini çağırabilir.
 * Unity'deki Animator penceresinde animasyon olaylarını ayarladığınızdan emin olun.
 */

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); // |EN| Get reference to the Player script in parent GameObject |TR| Üst GameObject'teki Player script'ine referans al
    }

    public void CurrentStateTrigger()
    {
        player.CallAnimationTrigger(); // |EN| Forward the call to the Player script |TR| Çağrıyı Player script'ine ilet
    }
}
