using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private EntityVFX fx => GetComponentInChildren<EntityVFX>();

    [Header("Chest Settings")]
    [SerializeField] private Vector2 openForce = new Vector2(0, 5f); // |EN| Upward force applied when chest is opened |TR| Sandık açıldığında uygulanan yukarı doğru kuvvet

    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageSource)
    {
        if (anim.GetBool("ChestOpen")) return false; // |EN| If chest is already open, ignore damage |TR| Sandık zaten açıksa hasarı yoksay

        fx?.PlayOnDamageVFX(); // |EN| Play damage VFX when chest takes damage |TR| Sandık hasar aldığında hasar VFX'sini oynat
        anim.SetBool("ChestOpen", true); // |EN| Open the chest animation upon taking damage |TR| Hasar alındığında sandık açma animasyonu
        rb.linearVelocity = openForce; // |EN| Apply upward force to chest when opened |TR| Sandık açıldığında yukarı doğru kuvvet uygula
        rb.angularVelocity = Random.Range(-100f, 100f); // |EN| Apply random rotation to chest when opened |TR| Sandık açıldığında rastgele dönüş uygula

        //|EN| Logic for dropping loot can be implemented here |TR| Eşya düşürme mantığı burada uygulanabilir

        return true; // |EN| Damage was successfully applied |TR| Hasar başarıyla uygulandı
    }
}
