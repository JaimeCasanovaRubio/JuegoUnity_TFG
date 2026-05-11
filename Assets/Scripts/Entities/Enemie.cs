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
    [SerializeField] protected bool Stun = false;
    [SerializeField] protected bool Burn = false;
    private bool stunOnCooldown = false;

    [Header("Efectos visuales afinidades")]
    [SerializeField] private GameObject fxSangre;
    [SerializeField] private GameObject fxFuego;
    [SerializeField] private GameObject fxStun;

    private SpriteRenderer sr;
    
    protected Vector2 moveDirection = Vector2.right;
    protected float distanceOfPatrol = 0;

   
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
        sr = GetComponent<SpriteRenderer>();
        state = EnemyState.Patrol;
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
        if(Stun) return;
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
       if(distanceOfPatrol >= rangeOfPatrol){
        moveDirection *= -1;
        distanceOfPatrol = 0;
       }else{
        distanceOfPatrol += Time.deltaTime;
        rb.linearVelocity = moveDirection * patrolSpeed;
       }
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
        
    }
    public virtual void TakeDamage(float amount, string afinidad){
        base.TakeDamage(amount);

        rb.linearVelocity = Vector2.zero;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(-direction * knockback, ForceMode2D.Impulse);

        StartCoroutine(ApplyKnockback());

        switch(afinidad){
            case "Afinidad1":
                Player.Instance.CurrentHealth += 2;
                Player.Instance.ShowHealFx();
                StartCoroutine(ShowFxBriefly(fxSangre, 1f));
                break;
            case "Afinidad2":
                StartCoroutine(Burned(5f));
                break;
            case "Afinidad3":
                if (!stunOnCooldown) StartCoroutine(Stuned(1f));
                break;
            case "Afinidad0":
                break;
        }
    }   
    private IEnumerator ApplyKnockback()
    {
        knocked = true;
        yield return new WaitForSeconds(0.1f);
        rb.linearVelocity = Vector2.zero;
        knocked = false;
    }
    private IEnumerator ShowFxBriefly(GameObject fx, float duration)
    {
        if (fx == null) yield break;
        fx.SetActive(true);
        yield return new WaitForSeconds(duration);
        fx.SetActive(false);
    }
    private IEnumerator Stuned(float duration){
        stunOnCooldown = true;
        Stun = true;
        if (sr != null) sr.enabled = false;
        if (fxStun != null) fxStun.SetActive(true);
        yield return new WaitForSeconds(duration);
        Stun = false;
        if (fxStun != null) fxStun.SetActive(false);
        if (sr != null) sr.enabled = true;
        yield return new WaitForSeconds(4f);
        stunOnCooldown = false;
    }
    private IEnumerator Burned(float duration){
        float timer = duration;
        Burn = true;
        if (sr != null) sr.enabled = false;
        if (fxFuego != null) fxFuego.SetActive(true);
        while(timer > 0)
        {
            base.TakeDamage(damage * 0.25f);
            timer -= Time.deltaTime;
            yield return null;
        }
        Burn = false;
        if (fxFuego != null) fxFuego.SetActive(false);
        if (sr != null) sr.enabled = true;
    }
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}