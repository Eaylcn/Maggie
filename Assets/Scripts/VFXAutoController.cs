using UnityEngine;

public class VFXAutoController : MonoBehaviour
{
    [SerializeField] private bool autoDestroy = true; // |EN| Should the VFX auto-destroy after playing? |TR| VFX oynatıldıktan sonra otomatik olarak yok edilsin mi?
    [SerializeField] private float destroyDelay = 1f; // |EN| Delay before auto-destruction |TR| Otomatik yok etme öncesi gecikme süresi
    [Space]
    [SerializeField] private bool applyRandomOffset = true; // |EN| Should a random position offset be applied? |TR| Rastgele bir pozisyon ofseti uygulanmalı mı?
    [SerializeField] private bool applyRandomRotation = true; // |EN| Should a random rotation be applied? |TR| Rastgele bir dönüş uygulanmalı mı?

    [Header("Random Rotation")]
    [SerializeField] private float rotationMinZ = 0f; // |EN| Minimum Z rotation for randomization |TR| Rastgeleleştirme için minimum Z dönüşü
    [SerializeField] private float rotationMaxZ = 360f; // |EN| Maximum Z rotation for randomization |TR| Rastgeleleştirme için maksimum Z dönüşü

    [Header("Random Position Offset")]
    [SerializeField] private float xMinOffset = -0.3f; // |EN| Minimum X offset for randomization |TR| Rastgeleleştirme için minimum X ofseti
    [SerializeField] private float xMaxOffset = 0.3f; // |EN| Maximum X offset for randomization |TR| Rastgeleleştirme için maksimum X ofseti
    [Space]
    [SerializeField] private float yMinOffset = -0.3f; // |EN| Minimum Y offset for randomization |TR| Rastgeleleştirme için minimum Y ofseti
    [SerializeField] private float yMaxOffset = 0.3f; // |EN| Maximum Y offset for randomization |TR| Rastgeleleştirme için maksimum Y ofseti

    private void Start()
    {
        ApplyRandomPositionOffset(); // |EN| Apply random position offset if enabled |TR| Etkinleştirilmişse rastgele pozisyon ofseti uygula
        ApplyRandomRotation();       // |EN| Apply random rotation if enabled |TR| Etkinleştirilmişse rastgele dönüş uygula

        if (autoDestroy)
            Destroy(gameObject, destroyDelay); // |EN| Schedule auto-destruction |TR| Otomatik yok etme zamanlaması
    }

    private void ApplyRandomPositionOffset()
    {
        if (!applyRandomOffset)
            return;

        float randomX = Random.Range(xMinOffset, xMaxOffset); // |EN| Calculate random X offset |TR| Rastgele X ofseti hesapla
        float randomY = Random.Range(yMinOffset, yMaxOffset); // |EN| Calculate random Y offset |TR| Rastgele Y ofseti hesapla

        transform.position += new Vector3(randomX, randomY, 0f); // |EN| Apply the random offset to the VFX position |TR| Rastgele ofseti VFX pozisyonuna uygula
    }

    private void ApplyRandomRotation()
    {
        if (!applyRandomRotation)
            return;

        float randomZRotation = Random.Range(rotationMinZ, rotationMaxZ); // |EN| Calculate random Z rotation |TR| Rastgele Z dönüşü hesapla
        transform.Rotate(0f, 0f, randomZRotation); // |EN| Apply the random rotation to the VFX |TR| Rastgele dönüşü VFX'ye uygula
    }
}
