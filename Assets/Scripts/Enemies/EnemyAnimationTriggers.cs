using UnityEngine;

public class EnemyAnimationTriggers : EntityAnimationTriggers
{
    private Enemy enemy;
    private EnemyVFX enemyVFX;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<EnemyVFX>();
    }

    private void EnableCounterWindow()
    {
        enemyVFX.EnableAttackAlertVFX(true); // |EN| Enable the attack alert VFX |TR| Saldırı uyarı VFX'sini etkinleştir
        enemy.EnableCounterWindow(true); // |EN| Enable the counter window for the enemy |TR| Düşman için karşı saldırı penceresini etkinleştir
    }

    private void DisableCounterWindow()
    {
        enemyVFX.EnableAttackAlertVFX(false); // |EN| Disable the attack alert VFX |TR| Saldırı uyarı VFX'sini devre dışı bırak
        enemy.EnableCounterWindow(false); // |EN| Disable the counter window for the enemy |TR| Düşman için karşı saldırı penceresini devre dışı bırak
    }
}
