using UnityEngine;

public class Character : MonoBehaviour
{
    private const string IMAGE_NAME = "Button";

    [SerializeField] protected float maxHealth;
    [SerializeField] protected float maxMana;

    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;

    public bool HasValidTarget => (target != null && target.IsAlive);
    public bool IsAlive => (currentHealth > 0);

    protected float currentHealth;
    protected float currentMana;

    protected Character target;
    protected Timer attackCooldown;

    private void Awake() => OnAwake();
    private void Start() => OnStart();
    private void Update() => OnUpdate();

    protected virtual void OnAwake()
    {
        attackCooldown = gameObject.AddComponent<Timer>();
    }

    protected virtual void OnStart()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;

        if (attackSpeed > 0)
        {
            attackCooldown.StartTimer(1 / attackSpeed);
        }
    }

    protected virtual void OnUpdate()
    {
        if (!IsAlive)
        {
            return;
        }

        if (attackCooldown.State != TimerState.Completed)
        {
            return;
        }

        Attack();

        if (attackSpeed <= 0)
        {
            return;
        }

        attackCooldown.StartTimer(1 / attackSpeed);
    }

    public float GetHealth()
    {
        if (maxHealth <= 0)
        {
            return 0;
        }

        return (currentHealth / maxHealth);
    }

    public float GetMana()
    {
        if (maxMana <= 0)
        {
            return 0;
        }

        return (currentMana / maxMana);
    }

    protected virtual void Attack()
    {
        if (!HasValidTarget)
        {
            return;
        }

        target.TakeDamage(damage);
    }

    protected virtual void OnDeath()
    {
        Hide();
    }

    private void Hide()
    {
        GameObject image = transform.Find(IMAGE_NAME).gameObject;

        if (image != null)
        {
            image.SetActive(false);
        }
        
        GameObject healthBar = transform.GetComponentInChildren<HealthBar>().gameObject;

        if (healthBar != null)
        {
            healthBar.SetActive(false);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (!IsAlive)
        {
            OnDeath();
        }
    }
}