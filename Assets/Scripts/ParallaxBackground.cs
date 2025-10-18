using UnityEngine;

/*
 * A MonoBehaviour that manages multiple parallax background layers.
 * It updates the position of each background layer based on the camera’s horizontal movement,
 * creating a depth illusion effect as the camera moves.
 * Designed to be used with ParallaxLayer components for modular parallax setups.
 */

/*
 * Birden fazla parallax arka plan katmanını yöneten bir MonoBehaviour.
 * Kameranın yatay hareketine bağlı olarak her katmanın pozisyonunu günceller
 * ve kamera hareket ettikçe derinlik hissi oluşturan bir etki sağlar.
 * Modüler parallax düzenleri için ParallaxLayer bileşenleriyle birlikte kullanılmak üzere tasarlanmıştır.
 */     

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX; // |EN| Last recorded X position of the camera. |TR| Kameranın son kaydedilen X pozisyonu.
    private float cameraHalfWidth; // |EN| Half the width of the camera's viewport in world units. |TR| Kameranın görünüm alanının dünya birimlerindeki yarısı.

    [SerializeField] private ParallaxLayer[] backgroundLayers; // |EN| Array of parallax layers. |TR| Parallax katmanlarının dizisi.

    private void Awake()
    {
        mainCamera = Camera.main; // |EN| Get the main camera reference. |TR| Ana kamera referansını al.
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect; // |EN| Calculate half the width of the camera's viewport. |TR| Kameranın görünüm alanının yarısını hesapla.
        CalculateImageLength(); // |EN| Calculate the width of each background layer's image. |TR| Her arka plan katmanının görüntü genişliğini hesapla.
    }

    // |EN| Update is called at fixed intervals to ensure smooth parallax movement. |TR| Sabit aralıklarla çağrılır ve düzgün parallax hareketi sağlar.
    private void FixedUpdate()
    {
        float currentCameraPositionX = mainCamera.transform.position.x; // |EN| Current X position of the camera. |TR| Kameranın mevcut X pozisyonu.
        float distanceToMove = currentCameraPositionX - lastCameraPositionX; // |EN| Calculate the distance the camera has moved since the last frame. |TR| Kameranın son kareden bu yana ne kadar hareket ettiğini hesapla.
        lastCameraPositionX = currentCameraPositionX; // |EN| Update the last camera position. |TR| Son kamera pozisyonunu güncelle.

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth; // |EN| Calculate the left edge of the camera's viewport. |TR| Kameranın görünüm alanının sol kenarını hesapla.
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth; // |EN| Calculate the right edge of the camera's viewport. |TR| Kameranın görünüm alanının sağ kenarını hesapla.

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove); // |EN| Move each background layer based on the camera movement. |TR| Kamera hareketine göre her arka plan katmanını hareket ettir.
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge); // |EN| Loop the background layer if it goes off-screen. |TR| Ekrandan çıktığında arka plan katmanını döngüye al.
        }
    }

    private void CalculateImageLength()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.CalculateImageWidth(); // |EN| Calculate the width of each background layer's image. |TR| Her arka plan katmanının görüntü genişliğini hesapla.
        }
    }
}
