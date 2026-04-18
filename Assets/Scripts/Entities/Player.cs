using UnityEngine;
using System.Collections;

public abstract class Player:Entity
{
    protected Rigidbody2D rb;
    
    [Header("Ability Cooldown")]
    [SerializeField] protected float abilityCooldownDuration = 2f;
    protected float abilityCooldownTimer = 0f;

    public float AbilityCooldownRemaining => Mathf.Max(0, abilityCooldownTimer);
    public float AbilityCooldownTotal => abilityCooldownDuration;

    protected virtual void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Update()
    {   
        HandleMovement();
        UpdateAbilityCooldown();
        ExecAbility();
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
    protected virtual void ExecAttack(Enemie target)
    {
        isInvencible = true;
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        StartCoroutine(EndInvencibility(invulnerabilityTime));
    }
    protected virtual void ExecAbility(){}
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            if(InputHandler.Instance.ConsumeAttackInput())
            {   
                ExecAttack(collision.gameObject.GetComponent<Enemie>());
            }
            
        }
    }
    private IEnumerator EndInvencibility(float delay)
    {
        yield return new WaitForSeconds(delay);
        isInvencible = false;
    }

    
}