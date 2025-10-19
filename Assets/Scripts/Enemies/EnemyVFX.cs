using UnityEngine;

public class EnemyVFX : EntityVFX
{
    [Header("Counter Attack Window VFX")]
    [SerializeField] private GameObject attackAlertVFX; // |EN| VFX to indicate the enemy is vulnerable to counter-attacks |TR| Düşmanın karşı saldırılara karşı savunmasız olduğunu gösteren VFX

    public void EnableAttackAlertVFX(bool enable)
    {
        if (attackAlertVFX == null) return;

        attackAlertVFX.SetActive(enable); // |EN| Enables or disables the attack alert VFX |TR| Saldırı uyarı VFX'sini etkinleştirir veya devre dışı bırakır
    } 
        
}
