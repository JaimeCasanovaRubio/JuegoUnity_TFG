using UnityEngine;

/// <summary>
/// Clase base para todas las entidades del juego.
/// Equivalente a Entity.java de LibGDX.
/// </summary>
public class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 3;
    [SerializeField] protected int health;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected bool isDead = false;

    [Header("Invincibility")]
    [SerializeField] protected bool isInvincible = false;
    [SerializeField] protected float invincibleTimer = 0f;
    [SerializeField] protected float invincibleDuration = 1f;

    [Header("Attack")]
    [SerializeField] protected bool isAttacking = false;
    [SerializeField] protected float attackTimer = 0f;

    [Header("Visual Feedback")]
    [SerializeField] protected float hitAnimationTimer = 0f;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected bool tp = false;

    // Properties públicas
    public int MaxHealth => maxHealth;
    public int Health
    {
        get => health;
        set => health = Mathf.Clamp(value, 0, maxHealth);
    }
    public int Damage => damage;
    public bool IsDead
    {
        get => isDead;
        set => isDead = value;
    }
    public bool IsInvincible => isInvincible;
    public bool IsAttacking => isAttacking;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        health = maxHealth;
    }

    protected virtual void Update()
    {
        HandleInvincibility();
        HandleAttackTimer();
        HandleHitAnimation();
    }

    /// <summary>
    /// Gestiona el timer de invencibilidad.
    /// </summary>
    protected virtual void HandleInvincibility()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }

            // Efecto visual de parpadeo durante invencibilidad
            if (spriteRenderer != null)
            {
                float alpha = Mathf.PingPong(Time.time * 10, 1f) > 0.5f ? 1f : 0.3f;
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }
        else
        {
            // Restaurar alpha normal
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }
    }

    /// <summary>
    /// Gestiona el timer de ataque.
    /// </summary>
    protected virtual void HandleAttackTimer()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                isAttacking = false;
            }
        }
    }

    /// <summary>
    /// Gestiona la animación de recibir daño.
    /// </summary>
    protected virtual void HandleHitAnimation()
    {
        if (hitAnimationTimer > 0)
        {
            hitAnimationTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Aplica daño a la entidad.
    /// </summary>
    public virtual void TakeDamage(int damageAmount)
    {
        if (!isInvincible && !isDead)
        {
            health -= damageAmount;
            isInvincible = true;
            invincibleTimer = invincibleDuration;
            hitAnimationTimer = 0.5f;

            // Trigger animación de daño
            if (animator != null)
            {
                animator.SetTrigger("Hit");
            }

            Debug.Log($"{gameObject.name} recibió {damageAmount} de daño. Vida: {health}/{maxHealth}");

            if (health <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// Cura a la entidad.
    /// </summary>
    public virtual void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log($"💖 {gameObject.name} curó {amount}. Vida: {health}/{maxHealth}");
    }

    /// <summary>
    /// Llamado cuando la entidad muere.
    /// </summary>
    protected virtual void Die()
    {
        isDead = true;
        Debug.Log($"💀 {gameObject.name} ha muerto.");

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
    }

    /// <summary>
    /// Detecta colisiones con objetos del mundo (Colliders sin "Is Trigger").
    /// </summary>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        HandleTeleport(collision.gameObject);
    }

    /// <summary>
    /// Detecta colisiones con triggers (Colliders con "Is Trigger" activado).
    /// </summary>
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        HandleTeleport(other.gameObject);
    }

    /// <summary>
    /// Maneja la lógica de teletransporte.
    /// </summary>
    private void HandleTeleport(GameObject other)
    {
        if (other.CompareTag("TP"))
        {
            Debug.Log("TP detectado!");
            TeleportZone teleport = other.GetComponent<TeleportZone>();
            string targetScene = teleport != null ? teleport.TargetScene : "OniricForest";

            if (GameManager.Instance != null)
            {
                GameManager.Instance.ChangeScene(targetScene);
            }
        }
    }
}
