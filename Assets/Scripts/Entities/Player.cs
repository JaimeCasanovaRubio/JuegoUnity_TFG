using UnityEngine;

/// <summary>
/// Clase base para el jugador.
/// Equivalente a Player.java de LibGDX.
/// Usa InputHandler para gestionar el input.
/// </summary>
public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] protected float speed = 5f;
    protected Vector2 movement;
    protected Rigidbody2D rb;
    protected bool facingRight = true;
    protected string lastDirection = "right";

    [Header("Ability")]
    [SerializeField] protected float abilityCooldown = 1f;
    [SerializeField] protected float abilityDuration = 0.1f;
    protected float cooldownTimer = 0f;
    protected float abilityTimer = 0f;
    protected bool abilityActive = false;
    protected bool canUseAbility = true;

    // Para habilidades que afectan al movimiento (como dash)
    protected bool abilityAffectsMovement = false;
    protected Vector2 abilityMoveDirection;

    // Properties públicas
    public float Speed => speed;
    public float AbilityCooldown => cooldownTimer;
    public float MaxAbilityCooldown => abilityCooldown;
    public bool IsAbilityActive => abilityActive;
    public bool CanUseAbility => canUseAbility;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead)
        {
            HandleInput();
            HandleAbilityCooldown();
            UpdateAnimation();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isDead)
        {
            HandleMovement();
        }
    }

    /// <summary>
    /// Procesa la entrada del jugador usando InputHandler.
    /// </summary>
    protected virtual void HandleInput()
    {
        // Verificar que InputHandler esté disponible
        if (InputHandler.Instance == null) return;

        // Obtener movimiento desde InputHandler
        movement = InputHandler.Instance.Movement;
        facingRight = InputHandler.Instance.FacingRight;
        lastDirection = InputHandler.Instance.LastDirection;

        // Ataque
        if (InputHandler.Instance.AttackPressed)
        {
            Attack();
        }

        // Habilidad
        if (InputHandler.Instance.AbilityPressed)
        {
            ActivateAbility();
        }
    }

    /// <summary>
    /// Maneja el movimiento físico del jugador.
    /// </summary>
    protected virtual void HandleMovement()
    {
        if (abilityActive && abilityAffectsMovement)
        {
            // Movimiento de habilidad (como dash)
            rb.MovePosition(rb.position + abilityMoveDirection * Time.fixedDeltaTime);
        }
        else if (!abilityActive)
        {
            // Movimiento normal
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Gestiona el cooldown de la habilidad.
    /// </summary>
    protected virtual void HandleAbilityCooldown()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                canUseAbility = true;
            }
        }

        if (abilityActive)
        {
            abilityTimer -= Time.deltaTime;
            OnAbilityUpdate(Time.deltaTime);

            if (abilityTimer <= 0)
            {
                abilityActive = false;
                abilityAffectsMovement = false;
                OnAbilityEnd();
            }
        }
    }

    /// <summary>
    /// Actualiza la animación del jugador.
    /// </summary>
    protected virtual void UpdateAnimation()
    {
        if (animator == null) return;

        // Flip del sprite según la dirección
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
        }

        // Parámetros del animator
        animator.SetFloat("Speed", movement.magnitude);
        animator.SetBool("IsMoving", movement.magnitude > 0.1f);
    }

    /// <summary>
    /// Inicia un ataque.
    /// </summary>
    public virtual void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            isInvincible = true;
            invincibleTimer = 0.5f;
            attackTimer = 0.5f;

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Debug.Log($"⚔️ {gameObject.name} ataca!");
        }
    }

    /// <summary>
    /// Activa la habilidad especial.
    /// </summary>
    public void ActivateAbility()
    {
        if (canUseAbility && !abilityActive)
        {
            abilityActive = true;
            canUseAbility = false;
            cooldownTimer = abilityCooldown;
            abilityTimer = abilityDuration;

            if (animator != null)
            {
                animator.SetTrigger("Ability");
            }

            ExecuteAbility();
            Debug.Log($"✨ {gameObject.name} usa su habilidad!");
        }
    }

    /// <summary>
    /// Ejecuta la habilidad específica del personaje.
    /// Las clases hijas deben sobrescribir este método.
    /// </summary>
    protected virtual void ExecuteAbility()
    {
        // Implementación por defecto: no hace nada
        // Las clases hijas definen su propia habilidad
    }

    /// <summary>
    /// Llamado cada frame mientras la habilidad está activa.
    /// </summary>
    protected virtual void OnAbilityUpdate(float deltaTime)
    {
        // Para efectos continuos
    }

    /// <summary>
    /// Llamado cuando la habilidad termina.
    /// </summary>
    protected virtual void OnAbilityEnd()
    {
        // Para limpiar efectos
    }

    protected override void Die()
    {
        base.Die();

        // Notificar al GameManager
        if (GameManager.Instance != null)
        {
            // Cambiar a pantalla de muerte después de un delay
            Invoke(nameof(GoToDeathScreen), 1.5f);
        }
    }

    private void GoToDeathScreen()
    {
        GameManager.Instance?.ChangeScene("DeadScreen");
    }
}
