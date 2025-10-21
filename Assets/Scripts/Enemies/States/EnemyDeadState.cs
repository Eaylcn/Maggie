using UnityEngine;

public class EnemyDeadState : EnemyState
{
    private Collider2D enemyCollider;
    private UI_MiniHealthBar miniHealthBar;
    private EnemyVFX vfx;

    public EnemyDeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyCollider = enemy.GetComponent<Collider2D>();
        miniHealthBar = enemy.GetComponentInChildren<UI_MiniHealthBar>();
        vfx = enemy.GetComponent<EnemyVFX>();
    }

    public override void Enter()
    {
        base.Enter();

        // |EN| Apply the "bounce then fall off map" death effect (refactored into a method for reuse) 
        // |TR| "Zıpla sonra haritadan düş" ölüm etkisini uygular (yeniden kullanılabilirlik için metoda ayrıldı)
        // ApplyDeathBounce();

        // |EN| Disable further state changes upon death |TR| Ölüm üzerine daha fazla durum değişikliğini devre dışı bırak
        
        miniHealthBar.gameObject.SetActive(false); // |EN| Hide mini health bar upon death |TR| Ölüm üzerine mini sağlık çubuğunu gizle
        stateMachine.SwitchOffStateMachine();
        vfx.EnableAttackAlertVFX(false); // |EN| Disable any active attack alert VFX |TR| Aktif saldırı uyarı VFX'sini devre dışı bırak
    }

    public override void Update()
    {
        base.Update();

        // |EN| Keep zeroing horizontal/vertical movement if not using a death animation.
        // If you play a dedicated death animation that moves the sprite, you can remove this call.
        // |TR| Ölüm animasyonu kullanılmıyorsa yatay/düşey hareketi sıfırlamaya devam et.
        // Eğer sprite'ı hareket ettiren özel bir ölüm animasyonu oynatıyorsanız bu çağrıyı kaldırabilirsiniz.
        enemy.SetVelocity(0, 0);
        
        if (vfx.isAnyStatusVfxPlaying) 
            vfx.StopAllStatusVfx(); // |EN| Stop all status VFX upon death |TR| Ölümde tüm durum VFX'lerini durdur

        if (triggerCalled)
            Object.Destroy(enemy.gameObject); // |EN| Destroy enemy object after death animation finishes |TR| Ölüm animasyonu bittikten sonra düşman nesnesini yok et
    }

    // |EN| Performs the Mario-like death effect: disable animator/collider, give an upward impulse, then increase gravity so the enemy falls faster.
    // |TR| Mario tarzı ölüm efektini gerçekleştirir: animatörü/çarpıştırıcıyı devre dışı bırakır, yukarı doğru kısa bir itki verir, ardından düşüşü hızlandırmak için yerçekimini artırır.
    private void ApplyDeathBounce()
    {
        anim.enabled = false;                 // |EN| Disable animator to stop regular animations on death |TR| Ölümde normal animasyonları durdurmak için animatörü devre dışı bırak
        enemyCollider.enabled = false;        // |EN| Disable collider so the corpse no longer interacts with the world |TR| Cesedin artık dünyayla etkileşime girmemesi için çarpıştırıcıyı devre dışı bırak

        rb.gravityScale = 12f;                // |EN| Increase gravity so the enemy accelerates downward after the bounce |TR| Zıplamadan sonra düşüş hızlanması için yerçekimi ölçeğini artır
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 15f); // |EN| Give an upward velocity for the bounce effect |TR| Zıplama etkisi için yukarı doğru bir hız uygula
    }
}
