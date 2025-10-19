using UnityEngine;

public class EnemyBattleState : EnemyState
{
    private Transform player; 
    private float lastTimeWasInBattle;

    public EnemyBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer(); // |EN| If we enter battle state without seeing player first time set the timer |TR| Oyuncuyu görmeden savaş durumuna girersek zamanlayıcıyı ayarla

        player ??= enemy.GetPlayerReference(); // |EN| Mean is if player is null then get player reference from enemy |TR| Anlamı, eğer oyuncu null ise düşmandan oyuncu referansını al

        if (ShouldRetreat())
        {
            // |EN| Retreat from player if too close |TR| Çok yakınsa oyuncudan geri çekil
            rb.linearVelocity = new Vector2(-DirectionToPlayer() * enemy.retreatVelocity.x, enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer()); // |EN| Flip enemy to face away from player when retreating |TR| Geri çekilirken düşmanın oyuncudan uzaklaşması için çevir
        }
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
            UpdateBattleTimer();

        if (IsBattleTimeOver())
            stateMachine.ChangeState(enemy.idleState);

        if (IsPlayerInRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocityY); // |EN| Move towards player if not in attack range |TR| Saldırı menzilinde değilse oyuncuya doğru hareket et
    }

    // |EN| Check if battle state duration has expired to transition back to idle state |TR| Boşta duruma geçmek için savaş durumu süresinin dolup dolmadığını kontrol et
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    
    // |EN| Check if the battle time duration has passed since last saw the player |TR| Oyuncuyu en son gördüğünden beri savaş süresi dolup dolmadığını kontrol et
    private bool IsBattleTimeOver() => Time.time >= lastTimeWasInBattle + enemy.battleTimeDuration;

    // |EN| Check if player is within attack range during battle state |TR| Savaş durumunda oyuncunun saldırı menzili içinde olup olmadığını kontrol et 
    private bool IsPlayerInRange() => DistanceToPlayer() < enemy.attackDistance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.minRetreatDistance;

    private float DistanceToPlayer()
    {
        // |EN| If player reference is null, return a large distance value to avoid errors |TR| Oyuncu referansı null ise, hataları önlemek için büyük bir mesafe değeri döndür
        if (player == null)
            return float.MaxValue;

        // |EN| If player reference is valid, return the absolute horizontal distance to the player |TR| Oyuncu referansı geçerliyse, oyuncuya olan mutlak yatay mesafeyi döndür
        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        // |EN| If player reference is null, return 0 to enter idle animation on battle state |TR| Oyuncu referansı null ise, savaş durumunda boşta animasyonuna gitmek için 0 döndür
        if (player == null)
            return 0;

        // |EN| Return 1 if player is to the right, -1 if to the left and enter move animation on battle state |TR| Oyuncu sağdaysa 1, soldaysa -1 döndür ve savaş durumunda hareket animasyonuna gir
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
