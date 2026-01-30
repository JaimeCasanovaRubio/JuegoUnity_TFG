using UnityEngine;

/// <summary>
/// Enemigo terrestre con patrullaje y detección del jugador.
/// Equivalente a GroundEnemy.java de LibGDX.
/// </summary>
public class GroundEnemy : Entity
{
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRange = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Patrol Bounds")]
    private float leftBound;
    private float rightBound;
    private float upBound;
    private float downBound;

    private bool movingRight = true;
    private bool movingUp = true;
    private bool facingRight = true;

    private Vector3 startPosition;
    private Transform playerTransform;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        maxHealth = 4;
        health = maxHealth;

        // Guardar posición inicial y calcular límites de patrulla
        startPosition = transform.position;
        leftBound = startPosition.x - patrolRange;
        rightBound = startPosition.x + patrolRange;
        upBound = startPosition.y + patrolRange;
        downBound = startPosition.y - patrolRange;

        // Buscar al jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (!isDead)
        {
            CheckPlayerDetection();
            Patrol();
            UpdateEnemyAnimation();
        }
    }

    /// <summary>
    /// Patrulla entre los límites establecidos.
    /// </summary>
    private void Patrol()
    {
        Vector2 movement = Vector2.zero;

        // Movimiento horizontal
        if (movingRight)
        {
            movement.x = moveSpeed;
            facingRight = true;
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
            }
        }
        else
        {
            movement.x = -moveSpeed;
            facingRight = false;
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
            }
        }

        // Movimiento vertical
        if (movingUp)
        {
            movement.y = moveSpeed;
            if (transform.position.y >= upBound)
            {
                movingUp = false;
            }
        }
        else
        {
            movement.y = -moveSpeed;
            if (transform.position.y <= downBound)
            {
                movingUp = true;
            }
        }

        // Aplicar movimiento
        rb.MovePosition(rb.position + movement * Time.deltaTime);

        // Flip del sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !facingRight;
        }
    }

    /// <summary>
    /// Detecta si el jugador está cerca y ajusta la dirección.
    /// </summary>
    private void CheckPlayerDetection()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= detectionRange)
        {
            // El jugador está en rango de detección
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Ajustar dirección horizontal
            if (directionToPlayer.x > 0.1f)
            {
                movingRight = true;
            }
            else if (directionToPlayer.x < -0.1f)
            {
                movingRight = false;
            }

            // Ajustar dirección vertical
            if (directionToPlayer.y > 0.1f)
            {
                movingUp = true;
            }
            else if (directionToPlayer.y < -0.1f)
            {
                movingUp = false;
            }
        }
    }

    /// <summary>
    /// Actualiza la animación del enemigo.
    /// </summary>
    private void UpdateEnemyAnimation()
    {
        if (animator == null) return;

        animator.SetBool("IsMoving", true); // El enemigo siempre está patrullando
    }

    /// <summary>
    /// Llamado cuando colisiona con otro objeto.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && !player.IsInvincible)
            {
                // Si el jugador está atacando, el enemigo recibe daño
                if (player.IsAttacking)
                {
                    TakeDamage(player.Damage);
                }
                else
                {
                    // Si no, el jugador recibe daño
                    player.TakeDamage(damage);
                }
            }
        }
    }

    protected override void Die()
    {
        base.Die();
        // Destruir después de la animación de muerte
        Destroy(gameObject, 1f);
    }

    /// <summary>
    /// Dibuja gizmos en el editor para visualizar el área de patrulla y detección.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Vector3 pos = Application.isPlaying ? startPosition : transform.position;

        // Área de patrulla
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, new Vector3(patrolRange * 2, patrolRange * 2, 0));

        // Área de detección
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, detectionRange);
    }
}
