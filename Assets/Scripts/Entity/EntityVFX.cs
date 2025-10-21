using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity entity;
    public bool isAnyStatusVfxPlaying { get; private set; }

    [Header("On Taking Damage VFX")]
    [SerializeField] private Material onTakingDamageMaterial; // |EN| Material to use when entity takes damage |TR| Varlık hasar aldığında kullanılacak malzeme
    [SerializeField] private float onTakingDamageVFXDuration = 0.2f; // |EN| Duration to show the damage VFX |TR| Hasar VFX'sini gösterme süresi
    private Material originalMaterial; // |EN| Original material of the entity |TR| Varlığın orijinal malzemesi
    private Coroutine onTakingDamageVfxCoroutine; // |EN| Coroutine reference for damage VFX |TR| Hasar VFX'si için Coroutine referansı

    [Header("On Doing Damage VFX")]
    [SerializeField] private Color hitVfxColor = Color.white; // |EN| Color tint for hit VFX when entity deals damage |TR| Varlık hasar verdiğinde vurma VFX'si için renk tonu
    [SerializeField] private GameObject hitVfxPrefab; // |EN| Prefab for hit VFX when entity deals damage |TR| Varlık hasar verdiğinde vurma VFX'si için prefab
    [SerializeField] private GameObject criticalHitVfxPrefab; // |EN| Prefab for critical hit VFX when entity deals critical damage |TR| Varlık kritik hasar verdiğinde kritik vurma VFX'si için prefab

    [Header("Elemental Colors")]
    [SerializeField] private Color iceVfxColor = new Color(0f, 0.82f, 1f); // |EN| Color hex for ice elemental VFX |TR| Buz elementsel VFX'si için renk hex
    [SerializeField] private Color fireVfxColor = new Color(1f, 0.4f, 0f); // |EN| Color hex for fire elemental VFX |TR| Ateş elementsel VFX'si için renk hex
    [SerializeField] private Color lightningVfxColor = new Color(1f, 1f, 0.3f); // |EN| Color hex for lightning elemental VFX |TR| Yıldırım elementsel VFX'si için renk hex
    private Color originalHitVfxColor; // |EN| Default color for hit VFX |TR| Vurulan VFX için varsayılan renk

    private void Awake()
    {
        entity = GetComponent<Entity>(); // |EN| Get reference to Entity component |TR| Entity bileşenine referans al
        sr = GetComponentInChildren<SpriteRenderer>(); // |EN| Get reference to SpriteRenderer in children |TR| Çocuklardaki SpriteRenderer'a referans al
        originalMaterial = sr.material; // |EN| Get the original material of the entity |TR| Varlığın orijinal malzemesini al
        originalHitVfxColor = hitVfxColor; // |EN| Store the original hit VFX color |TR| Orijinal vurma VFX rengini sakla
        isAnyStatusVfxPlaying = false;
    }

    public void PlayStatusVfx(float duration, ElementType elementType)
    {
        if (elementType == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(iceVfxColor, duration));

        if (elementType == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(fireVfxColor, duration));

        if (elementType == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(lightningVfxColor, duration));
    }
    
    public void StopAllStatusVfx()
    {
        StopAllCoroutines();
        sr.color = Color.white; // |EN| Reset color to white |TR| Rengi beyaza sıfırla
        sr.material = originalMaterial; // |EN| Reset material to original |TR| Malzemeyi orijinaline sıfırla
        isAnyStatusVfxPlaying = false;
    }

    private IEnumerator PlayStatusVfxCo(Color vfxColor, float duration)
    {
        float tickInterval = 0.25f; // |EN| Interval between color changes |TR| Renk değişiklikleri arasındaki aralık
        float elapsedTime = 0f; // |EN| Time elapsed since start of VFX |TR| VFX başlangıcından itibaren geçen süre

        Color lightColor = vfxColor * 1.2f; // |EN| Lighter version of the VFX color |TR| VFX renginin daha açık versiyonu
        Color darkColor = vfxColor * 0.8f;  // |EN| Darker version of the VFX color |TR| VFX renginin daha koyu versiyonu

        isAnyStatusVfxPlaying = true;

        bool toggle = false;

        while (elapsedTime < duration) // |EN| Loop until the total duration is reached |TR| Toplam süreye ulaşılana kadar döngü
        {
            sr.color = toggle ? lightColor : darkColor; // |EN| Alternate between light and dark colors |TR| Açık ve koyu renkler arasında geçiş yap
            toggle = !toggle; // |EN| Toggle the color state |TR| Renk durumunu değiştir

            yield return new WaitForSeconds(tickInterval); // |EN| Wait for the tick interval |TR| Tick aralığını bekle
            elapsedTime += tickInterval; // |EN| Increment elapsed time |TR| Geçen süreyi artır
        }

        sr.color = Color.white; // |EN| Reset color to white after VFX ends |TR| VFX sona erdikten sonra rengi beyaza sıfırla
        isAnyStatusVfxPlaying = false;
    }

    public void CreateOnHitVFX(Transform target, bool isCritical)
    {
        GameObject hitPrefab = isCritical ? criticalHitVfxPrefab : hitVfxPrefab; // |EN| Choose prefab based on whether it's a critical hit |TR| Kritik vurma olup olmadığına göre prefab seç
        GameObject hitVfx = Instantiate(hitPrefab, target.position, Quaternion.identity); // |EN| Instantiate hit VFX at specified position |TR| Belirtilen pozisyonda vurma VFX'si oluştur

        hitVfx.GetComponentInChildren<SpriteRenderer>().color = hitVfxColor; // |EN| Apply color tint to hit VFX |TR| Vurulan VFX'ye renk tonu uygula

        if (entity.facingDirection == -1 && isCritical)
            hitVfx.transform.Rotate(0f, 180f, 0f); // |EN| Flip VFX for left-facing critical hits |TR| Sola bakan kritik vuruşlar için VFX'yi çevir
    }
    
    public void SetElementalVfxColor(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Ice:
                hitVfxColor = iceVfxColor;
                break;
            case ElementType.Fire:
                hitVfxColor = fireVfxColor;
                break;
            case ElementType.Lightning:
                hitVfxColor = lightningVfxColor;
                break;
            default:
                hitVfxColor = originalHitVfxColor; // |EN| Revert to original color for non-elemental or unhandled types |TR| Elemental olmayan veya işlenmeyen türler için orijinal renge geri dön
                break;
        }
    }

    public void PlayOnDamageVFX()
    {
        if (onTakingDamageVfxCoroutine != null) // |EN| If a damage VFX is already playing, stop it |TR| Zaten bir hasar VFX'si oynuyorsa, durdur
            StopCoroutine(onTakingDamageVfxCoroutine); 

        onTakingDamageVfxCoroutine = StartCoroutine(OnDamageVfxCo()); // |EN| Start the damage VFX coroutine |TR| Hasar VFX'si coroutine'ini başlat
    }

    private IEnumerator OnDamageVfxCo()
    {
        if (sr == null || onTakingDamageMaterial == null)
            yield break; // |EN| Exit if SpriteRenderer or material is not set |TR| SpriteRenderer veya malzeme ayarlanmadıysa çık

        sr.material = onTakingDamageMaterial; // |EN| Change to damage material |TR| Hasar malzemesine değiştir
        yield return new WaitForSeconds(onTakingDamageVFXDuration); // |EN| Wait for the specified duration |TR| Belirtilen süreyi bekle
        sr.material = originalMaterial; // |EN| Revert to original material |TR| Orijinal malzemeye geri dön
    }
}
