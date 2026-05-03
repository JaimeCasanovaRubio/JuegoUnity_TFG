using UnityEngine;
using System.Collections;

public abstract class Player:Entity
{
    public static Player Instance { get; private set; }

    protected Rigidbody2D rb;
    
    [Header("Ability Cooldown")]
    [SerializeField] protected float abilityCooldownDuration = 2f;
    protected float abilityCooldownTimer = 0f;

    public float AbilityCooldownRemaining => Mathf.Max(0, abilityCooldownTimer);
    public float AbilityCooldownTotal => abilityCooldownDuration;
    
    public bool attacking {get; protected set;} = false;

    protected virtual void Awake()
    {
        if (Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        base.Awake();
        DontDestroyOnLoad(transform.root.gameObject);
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {   
        HandleMovement();
        UpdateAbilityCooldown();
        ExecAbility();
        if(InputHandler.Instance.AttackPressed)
        {
            ExecAttack();
        }
    }
    
    protected virtual void UpdateAbilityCooldown()
    {
        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }
    }
    
    protected bool CanUseAbility()
    {
        return abilityCooldownTimer <= 0;
    }
    
    protected void StartAbilityCooldown()
    {
        abilityCooldownTimer = abilityCooldownDuration;
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Teleport tp = collision.GetComponent<Teleport>();
        if(tp!=null)
        {
            GameManager.Instance.ChangeScene(tp.sceneName, tp.index);
        }
    }
    protected virtual void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.Movement;
        rb.linearVelocity = input * moveSpeed;
    }
    protected virtual void ExecAttack()
    {
        isInvencible = true;
        attacking = true;
        StartCoroutine(EndInvulnerability(invulnerabilityTime));
        StartCoroutine(EndAttack(1f));
    }
    protected virtual void ExecAbility(){}
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            if(attacking){
                Enemie enemie = collision.gameObject.GetComponent<Enemie>();
                enemie.TakeDamage(damage);
            }
        }
    }
    private IEnumerator EndInvulnerability(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInvencible = false;
    }
    private IEnumerator EndAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        attacking = false;
    }    
}