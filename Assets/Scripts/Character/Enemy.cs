using UnityEngine;

public class Enemy : Character
{
    private const string MARK_NAME = "Mark";

    private const float RESPAWN_DELAY = 2f;
    private const float STUN_DURATION = 2f;

    private const float HEALTH_REGENERATION_COUNT = 5f;
    private const float REGENERATE_HEALTH_CHANCE = 0.25f;

    private static GameObject enemyPrefab;

    public void Mark() => SetMarkVisibility(true);
    public void UnMark() => SetMarkVisibility(false);

    private Timer respawnCooldown;
    private Timer stunDurationTimer;

    protected override void OnAwake()
    {
        base.OnAwake();

        respawnCooldown = gameObject.AddComponent<Timer>();
        stunDurationTimer = gameObject.AddComponent<Timer>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        target = Player.Instance;

        if (enemyPrefab == null)
        {
            enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        }
    }

    protected override void OnUpdate()
    {
        if (!IsAlive && respawnCooldown.State == TimerState.Completed)
        {
            Respawn();
            return;
        }

        if (stunDurationTimer.State == TimerState.Running)
        {
            return;
        }

        if (stunDurationTimer.State == TimerState.Completed)
        {
            if (attackSpeed > 0)
            {
                attackCooldown.StartTimer(1 / attackSpeed);
            }

            stunDurationTimer.StopTimer();
        }

        base.OnUpdate();
    }

    public void ApplyStun()
    {
        if (!IsAlive)
        {
            return;
        }

        attackCooldown.StopTimer();
        stunDurationTimer.StartTimer(STUN_DURATION);
    }

    protected override void Attack()
    {
        base.Attack();
        TryRegenerateHealth();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        respawnCooldown.StartTimer(RESPAWN_DELAY);
    }

    private void TryRegenerateHealth()
    {
        if (!IsAlive)
        {
            return;
        }

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= REGENERATE_HEALTH_CHANCE)
        {
            currentHealth = Mathf.Min(currentHealth + HEALTH_REGENERATION_COUNT, maxHealth);
        }
    }

    private void Respawn()
    {
        if (enemyPrefab == null)
        {
            return;
        }

        GameObject newEnemy = Instantiate(enemyPrefab, transform.parent);

        newEnemy.transform.localPosition = transform.localPosition;
        newEnemy.transform.localRotation = transform.localRotation;
        newEnemy.transform.localScale = transform.localScale;

        Destroy(gameObject);
    }

    private void SetMarkVisibility(bool visible)
    {
        GameObject mark = transform.Find(MARK_NAME).gameObject;

        if (mark != null)
        {
            mark.SetActive(visible);
        }
    }
}
