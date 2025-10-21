using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyIdleState idleState { get; set; }
    public EnemyMoveState moveState { get; set; }
    public EnemyAttackState attackState { get; set; }
    public EnemyBattleState battleState { get; set; }
    public EnemyDeadState deadState { get; set; }
    public EnemyStunnedState stunnedState { get; set; }

    [Header("Battle Settings")]
    public float battleMoveSpeed = 3f; // |EN| Movement speed when in battle state |TR| Savaş durumundayken hareket hızı
    public float attackDistance = 2f; // |EN| Distance from player at which enemy will initiate attack |TR| Düşmanın saldırıya başlayacağı oyuncudan mesafe
    public float battleTimeDuration = 5f; // |EN| Duration in seconds for how long the enemy stays in battle state after losing player sight |TR| Düşmanın oyuncuyu görmeyi kaybettikten sonra savaş durumunda ne kadar süre kaldığı saniye cinsinden
    public float minRetreatDistance = 1f; // |EN| Minimum distance to maintain from player when retreating |TR| Geri çekilirken oyuncudan korunacak minimum mesafe
    public Vector2 retreatVelocity; // |EN| Velocity applied when retreating from player |TR| Oyuncudan geri çekilirken uygulanan hız

    [Header("Stunned State Details")]
    public float stunnedDuration = 1f; // |EN| Duration in seconds for how long the enemy remains stunned |TR| Düşmanın ne kadar süre sersemlemiş kaldığı saniye cinsinden
    public Vector2 stunnedKnockbackForce = new Vector2(5f, 5f); // |EN| Force applied to enemy when stunned |TR| Sersemletildiğinde düşmana uygulanan kuvvet
    protected bool canbeStunned; // |EN| Indicates if the enemy can be countered by the player |TR| Düşmanın oyuncu tarafından karşılanıp karşılanamayacağını gösterir

    [Header("Movement Settings")]
    public float idleDuration = 2f; // |EN| Duration in seconds for how long the enemy stays idle before moving |TR| Düşmanın hareket etmeden önce ne kadar süre boşta kaldığı saniye cinsinden
    public float moveSpeed = 1.4f;

    [Range(0f, 2f)]
    public float moveAnimSpeedMultiplier = 1f;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck; 
    [SerializeField] private float playerCheckDistance = 10f; // |EN| Distance for raycasting to detect player |TR| Oyuncuyu algılamak için ışınlama mesafesi
    public Transform player { get; private set; } // |EN| Reference to the detected player transform |TR| Algılanan oyuncu transformuna referans


    // |EN| Slowdown the enemy's movement and animation speed temporarily |TR| Düşmanın hareket ve animasyon hızını geçici olarak yavaşlat
    protected override IEnumerator SlowdownEntityCo(float slowMultiplier, float duration)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleMoveSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;

        float speedMultiplier = 1 - slowMultiplier; // |EN| Calculate speed multiplier based on slow effect |TR| Yavaşlatma etkisine göre hız çarpanını hesapla

        moveSpeed *= speedMultiplier;
        battleMoveSpeed *= speedMultiplier;
        anim.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleMoveSpeed;
        anim.speed = originalAnimSpeed;
    }

    public void EnableCounterWindow(bool enable) => canbeStunned = enable; // |EN| Enables or disables the enemy's ability to be countered |TR| Düşmanın karşılanabilme yeteneğini etkinleştirir veya devre dışı bırakır

    public override void EntityDeath()
    {
        base.EntityDeath();

        stateMachine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        // |EN| If already in battle state or attack state, do nothing |TR| Zaten savaş veya atak durumundaysa, hiçbir şey yapma
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState) return;

        this.player = player; // |EN| Set the detected player transform |TR| Algılanan oyuncu transformunu ayarla
        stateMachine.ChangeState(battleState);
    }
    
    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;

        return player;
    }

    // |EN| Get reference to player from raycast detection we are using like that because of we able to enter this state only by raycasting to player and also more efficient than other methods
    // |TR| Oyuncuya referans almak için kullandığımız ışınlama, çünkü bu duruma yalnızca oyuncuya ışınlama yaparak girebiliyoruz ve ayrıca diğer yöntemlerden daha verimli
    public RaycastHit2D PlayerDetected()
    {
        // |EN| Raycast to check for player detection and if hit ground before player then ignore |TR| Oyuncu algılanması için ışın atar ve oyuncudan önce zemin vurulursa yoksayar
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default; // |EN| Returns default if nothing is hit or if the hit object is not the player |TR| Hiçbir şey vurulmazsa veya vurulan nesne oyuncu değilse varsayılan değeri döndürür

        return hit; // |EN| Returns the RaycastHit2D result of the player detection raycast |TR| Oyuncu algılanırsa RaycastHit2D bilgilerini döndürür, aksi takdirde null döndürür
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // |EN| Draws player detection ray in Scene view for debugging and visualization |TR| Hata ayıklama ve görselleştirme için Sahne görünümünde oyuncu algılama ışınını çizer
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * playerCheckDistance), playerCheck.position.y));

        // |EN| Draws attack and retreat distance rays in Scene view for debugging and visualization |TR| Hata ayıklama ve görselleştirme için Sahne görünümünde saldırı ve geri çekilme mesafesi ışınlarını çizer
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * attackDistance), playerCheck.position.y));

        // |EN| Draws retreat distance ray in Scene view for debugging and visualization |TR| Hata ayıklama ve görselleştirme için Sahne görünümünde geri çekilme mesafesi ışınını çizer
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * minRetreatDistance), playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath; // |EN| Subscribe to player death event to handle enemy behavior |TR| Düşman davranışını yönetmek için oyuncu ölüm olayına abone ol
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath; // |EN| Unsubscribe from player death event to prevent memory leaks |TR| Bellek sızıntılarını önlemek için oyuncu ölüm olayından aboneliği kaldır
    }
}
