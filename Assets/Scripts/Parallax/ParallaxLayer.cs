using UnityEngine;

/*
 * A class representing a single layer in a parallax background system.
 * Each layer stores its own Transform reference and parallax factor,
 * determining how much it moves relative to the camera’s motion.
 * Intended to be managed by the ParallaxBackground class.
 */

/*
 * Parallax arka plan sisteminde tek bir katmanı temsil eden bir sınıf.
 * Her katman kendi Transform referansını ve parallax faktörünü tutar,
 * kameranın hareketine göre ne kadar kayacağını belirler.
 * ParallaxBackground sınıfı tarafından yönetilmek üzere tasarlanmıştır.
 */

[System.Serializable] // |EN| Makes the class visible in the Unity Inspector. |TR| Sınıfın Unity Inspector'da görünmesini sağlar.
public class ParallaxLayer
{
    [SerializeField] private Transform background; // |EN| Reference to the background Transform. |TR| Arka plan Transform’una referans.
    [SerializeField] private float parallaxFactor; // |EN| Factor determining the parallax effect intensity. |TR| Parallax efektinin yoğunluğunu belirleyen faktör.
    [SerializeField] private float imageWidthOffset = 10; // |EN| Offset to ensure seamless looping of the background image. |TR| Arka plan görüntüsünün kesintisiz döngüsünü sağlamak için ofset.

    private float imageFullWidth; // |EN| Full width of the background image in world units. |TR| Arka plan görüntüsünün dünya birimlerindeki tam genişliği.
    private float imageHalfWidth; // |EN| Half the width of the background image in world units. |TR| Arka plan görüntüsünün dünya birimlerindeki yarı genişliği.

    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x; // |EN| Get the full width of the background image. |TR| Arka plan görüntüsünün tam genişliğini al.
        imageHalfWidth = imageFullWidth / 2f; // |EN| Calculate half the width. |TR| Yarı genişliği hesapla.
    }

    public void Move(float distanceToMove)
    {
        // |EN| Move the background based on the parallax factor and distance moved. |TR| Parallax faktörüne ve hareket edilen mesafeye göre arka planı hareket ettir.
        background.position += Vector3.right * (distanceToMove * parallaxFactor);
    }
    
    public void LoopBackground(float cameraLeftEdge, float cameraRightEdge)
    {
        float imageRightEdge = (background.position.x + imageHalfWidth) - imageWidthOffset; // |EN| Calculate the right edge of the background image. |TR| Arka plan görüntüsünün sağ kenarını hesapla.
        float imageLeftEdge = (background.position.x - imageHalfWidth) + imageWidthOffset; // |EN| Calculate the left edge of the background image. |TR| Arka plan görüntüsünün sol kenarını hesapla.

        if (imageRightEdge < cameraLeftEdge)
        {
            // |EN| If the image has moved completely off the left side of the camera, reposition it to the right. |TR| Görüntü kameranın sol tarafından tamamen çıktıysa, sağa yeniden konumlandır.
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            // |EN| If the image has moved completely off the right side of the camera, reposition it to the left. |TR| Görüntü kameranın sağ tarafından tamamen çıktıysa, sola yeniden konumlandır.
            background.position -= Vector3.right * imageFullWidth;
        }
    }
}
