using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        entity.OnFlipped += HandleFlip; // |EN| Subscribe to entity flip event |TR| Varlık çevirme olayına abone ol
    }

    private void OnDisable()
    {
        entity.OnFlipped -= HandleFlip; // |EN| Unsubscribe from entity flip event |TR| Varlık çevirme olayından aboneliği kaldır
    }

    private void HandleFlip() => transform.rotation = Quaternion.identity; // |EN| Reset mini health bar rotation to default |TR| Mini sağlık çubuğu dönüşünü varsayılan olarak sıfırla
}
