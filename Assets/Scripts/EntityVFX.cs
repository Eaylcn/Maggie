using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial; // |EN| Material to use when entity takes damage |TR| Varlık hasar aldığında kullanılacak malzeme
    [SerializeField] private float onDamageVFXDuration = 0.2f; // |EN| Duration to show the damage VFX |TR| Hasar VFX'sini gösterme süresi
    private Material originalMaterial; // |EN| Original material of the entity |TR| Varlığın orijinal malzemesi
    private Coroutine onDamageVfxCoroutine; // |EN| Coroutine reference for damage VFX |TR| Hasar VFX'si için Coroutine referansı

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>(); // |EN| Get reference to SpriteRenderer in children |TR| Çocuklardaki SpriteRenderer'a referans al
        originalMaterial = sr.material; // |EN| Get the original material of the entity |TR| Varlığın orijinal malzemesini al
    }

    public void PlayOnDamageVFX()
    {
        if (onDamageVfxCoroutine != null) // |EN| If a damage VFX is already playing, stop it |TR| Zaten bir hasar VFX'si oynuyorsa, durdur
            StopCoroutine(onDamageVfxCoroutine); 

        onDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo()); // |EN| Start the damage VFX coroutine |TR| Hasar VFX'si coroutine'ini başlat
    }

    private IEnumerator OnDamageVfxCo()
    {
        sr.material = onDamageMaterial; // |EN| Change to damage material |TR| Hasar malzemesine değiştir
        yield return new WaitForSeconds(onDamageVFXDuration); // |EN| Wait for the specified duration |TR| Belirtilen süreyi bekle
        sr.material = originalMaterial; // |EN| Revert to original material |TR| Orijinal malzemeye geri dön
    }
}
