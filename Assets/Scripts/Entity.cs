using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    protected StateMachine stateMachine;

    private bool facingRight = true;                        // |EN| Tracks whether entity is currently facing right direction |TR| Varlığın şu anda sağa doğru bakıp bakmadığını takip eder
    public int facingDirection { get; private set; } = 1;   // |EN| Numerical facing direction: 1 for right, -1 for left |TR| Sayısal bakış yönü: sağ için 1, sol için -1

    [Header("Collision Detection Settings")]
    [SerializeField] protected LayerMask whatIsGround;            // |EN| Layer mask defining which layers count as solid ground |TR| Hangi katmanların katı zemin olarak sayılacağını tanımlayan katman maskesi
    [SerializeField] private float groundCheckDistance = 1.4f;  // |EN| Maximum distance to raycast downward for ground detection |TR| Zemin algılama için aşağı doğru ışın atma maksimum mesafesi
    [SerializeField] private float wallCheckDistance = 0.4f;    // |EN| Maximum distance to raycast forward for wall detection |TR| Duvar algılama için ileri doğru ışın atma maksimum mesafesi
    [SerializeField] private Transform groundCheck;              // |EN| Transform position for ground detection raycast |TR| Zemin algılama ışını için transform pozisyonu
    [SerializeField] Transform primaryWallCheck;                // |EN| Primary transform position for wall detection raycast |TR| Duvar algılama ışını için birincil transform pozisyonu
    [SerializeField] Transform secondaryWallCheck;              // |EN| Secondary transform position for improved wall detection accuracy |TR| Geliştirilmiş duvar algılama doğruluğu için ikincil transform pozisyonu
    public bool groundDetected { get; private set; }
    public bool wallDetected { get; private set; }

    // |EN| Called when script instance is loaded, initializes core components |TR| Script örneği yüklendiğinde çağrılır, temel bileşenleri başlatır
    protected virtual void Awake()
    {
        // |EN| Initialize component references before state machine setup |TR| Durum makinesi kurulumundan önce bileşen referanslarını başlat
        anim = GetComponentInChildren<Animator>(); 
        rb = GetComponent<Rigidbody2D>();

        stateMachine = new StateMachine();
    }

    // |EN| Called before first frame update, used for additional setup |TR| İlk kare güncellemesinden önce çağrılır, ek kurulum için kullanılır
    protected virtual void Start()
    {
        
    }

    // |EN| Called every frame to update entity logic and state machine |TR| Varlık mantığını ve durum makinesini güncellemek için her karede çağrılır
    protected virtual void Update()
    {
        HandleCollisionDetection();        // |EN| Perform collision detection checks (ground and wall detection) |TR| Çarpışma algılama kontrollerini gerçekleştir (zemin ve duvar algılama)
        stateMachine.UpdateActiveState();  // |EN| Update the currently active state in state machine |TR| Durum makinesindeki şu anda aktif olan durumu güncelle
    }

    public void TriggerCurrentStateAnimation()
    {
        stateMachine.currentState.CallAnimationTrigger(); // |EN| Delegates animation trigger to current state for proper handling |TR| Uygun işlem için animasyon tetikleyicisini mevcut duruma devreder
    }

    // |EN| Sets entity velocity and handles directional flipping based on movement |TR| Varlığın hızını ayarlar ve harekete dayalı yön çevirme işlemini yönetir
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity); // |EN| Apply velocity to rigidbody for physics movement |TR| Fizik hareketi için rigidbody'ye hız uygula
        HandleFlip(xVelocity); // |EN| Check and handle entity flipping based on horizontal movement |TR| Yatay harekete dayalı varlık çevirme işlemini kontrol et ve yönet
    }

    // |EN| Handles entity flipping logic based on horizontal velocity direction |TR| Yatay hız yönüne dayalı varlık çevirme mantığını yönetir
    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !facingRight)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    // |EN| Flips entity's facing direction by rotating and updating direction variables |TR| Döndürme ve yön değişkenlerini güncelleme ile varlığın bakış yönünü çevirir
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight; 
        facingDirection *= -1; // |EN| Multiply by -1 to invert the numerical facing direction |TR| Sayısal bakış yönünü tersine çevirmek için -1 ile çarp
    }

    // |EN| Performs raycast-based collision detection for ground and wall checking |TR| Zemin ve duvar kontrolü için ışın tabanlı çarpışma algılaması gerçekleştirir
    private void HandleCollisionDetection()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        // |EN| Uses dual raycasts from different points for more reliable wall detection |TR| Daha güvenilir duvar algılama için farklı noktalardan çift ışın kullanır
        if (secondaryWallCheck != null)
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround)
                        && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        }
        else
        {
            wallDetected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        }
    }

    // |EN| Draws collision detection rays in Scene view for debugging and visualization |TR| Hata ayıklama ve görselleştirme için Sahne görünümünde çarpışma algılama ışınlarını çizer
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0)); 

        if (secondaryWallCheck != null)
        {
            Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDirection, 0));
        }
    }
}
