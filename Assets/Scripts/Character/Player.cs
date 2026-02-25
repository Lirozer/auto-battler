using UnityEngine;

public class Player : Character
{
    private const float ACTIVE_ATTACK_MANA_COST = 20f;
    private const float DAMAGE_BOOST_MANA_COST = 50f;
    private const float MANA_REGENERATION_COUNT = 2f;

    private const float DAMAGE_MODIFICATOR = 2f;
    private const float ATTACK_SPEED_MODIFICATOR = 0.4f;

    private const float DAMAGE_BOOST_DURATION = 10f;

    private const float STUN_CHANCE = 0.3f;

    public static Player Instance { get; private set; }

    public static bool CanUseActiveAttack => (Instance.currentMana >= ACTIVE_ATTACK_MANA_COST);
    public static bool CanUseDamageBoostMode => (Instance.currentMana >= DAMAGE_BOOST_MANA_COST);

    private Enemy[] enemies;

    private Timer manaRegenerationCooldown;
    private Timer damageBoostTimer;

    protected override void OnAwake()
    {
        base.OnAwake();

        if (Instance == null )
        {
            Instance = this;
        }

        manaRegenerationCooldown = gameObject.AddComponent<Timer>();
        damageBoostTimer = gameObject.AddComponent<Timer>();
    }

    protected override void OnStart()
    {
        base.OnStart();

        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        if (enemies.Length > 0)
        {
            SetTarget(enemies[0]);
        }

        manaRegenerationCooldown.StartTimer(1);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (!HasValidTarget)
        {
            Enemy target = FindFirstAliveEnemy();
            SetTarget(target);
        }

        if (manaRegenerationCooldown.State == TimerState.Completed)
        {
            RegenerateMana();
            manaRegenerationCooldown.StartTimer(1);
        }

        if (damageBoostTimer.State == TimerState.Completed)
        {
            DamageBoostModeOff();
        }
    }

    public static void SetTarget(Enemy target)
    {
        if (target == null)
        {
            return;
        }

        if (Instance.target is Enemy enemy)
        {
            enemy.UnMark();
        }

        Instance.target = target;
        target.Mark();
    }

    public static void ActiveAttack()
    {
        Instance.Attack();
        Instance.currentMana -= ACTIVE_ATTACK_MANA_COST;
    }

    public static void DamageBoostModeOn()
    {
        Instance.damage *= DAMAGE_MODIFICATOR;
        Instance.attackSpeed *= ATTACK_SPEED_MODIFICATOR;

        Instance.currentMana -= DAMAGE_BOOST_MANA_COST;

        Instance.damageBoostTimer.StartTimer(DAMAGE_BOOST_DURATION);
    }

    protected override void Attack()
    {
        base.Attack();
        TryStun();
    }

    private void DamageBoostModeOff()
    {
        Instance.damage /= DAMAGE_MODIFICATOR;
        Instance.attackSpeed /= ATTACK_SPEED_MODIFICATOR;

        Instance.damageBoostTimer.StopTimer();
    }

    private void RegenerateMana()
    {
        currentMana += MANA_REGENERATION_COUNT;

        if (currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    private void TryStun()
    {
        if (!(target is Enemy enemy))
        {
            return;
        }

        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= STUN_CHANCE)
        {
            enemy.ApplyStun();
        }
    }

    private Enemy FindFirstAliveEnemy()
    {
        enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (Enemy enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                return enemy;
            }
        }

        return null;
    }
}
