using UnityEngine;
using System.Collections;

public abstract class Enemie:Entity
{
    protected Rigidbody2D rb;
    protected Player player;
    protected EnemyState state;

    [Header ("Parameters")]
    [SerializeField] protected float rangeOfPatrol; 
    [SerializeField] protected float rangeOfDetection;
    [SerializeField] protected float patrolSpeed = 2f;
    [SerializeField] protected float chaseSpeed = 3f;
    [SerializeField] protected float knockback = 1f;

    private bool knocked = false;
    protected Vector2 startPosition;
    protected Vector2 moveDirection = Vector2.right;

   
    protected float lastAttackTime;

     protected enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

   protected virtual void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        state = EnemyState.Patrol;
        startPosition = transform.position;
    }
    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
    }
    protected virtual void Update()
    {
        if(CurrentHealth <= 0)
        {
            OnDeath();
        }
        if(knocked) return;
        HandleMovement();
    }
    protected virtual void HandleMovement()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= rangeOfDetection)
            state = EnemyState.Chase;
        else
            state = EnemyState.Patrol;
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack(player);
                break;
        }
    }
    protected virtual void Patrol()
    {
        float distanceFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distanceFromStart) >= rangeOfPatrol)
            moveDirection *= -1;
        rb.linearVelocity = moveDirection * patrolSpeed;
    }
    protected virtual void Chase()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }
    protected virtual void Attack(Player target)
    {
        if (target != null)
        {   
            if(!target.isInvencible)
            {
                target.TakeDamage(damage);
            }
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {   Player player = collision.gameObject.GetComponent<Player>();
            if(player.isInvencible) {
                return;
            }
            Attack(player);
        }
    }
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        rb.linearVelocity = Vector2.zero;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(-direction * knockback, ForceMode2D.Impulse);

        StartCoroutine(ApplyKnockback());
    }
    private IEnumerator ApplyKnockback()
    {
        knocked = true;
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;
        knocked = false;
    }
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}