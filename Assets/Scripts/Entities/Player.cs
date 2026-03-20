using UnityEngine;

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
            GameManager.Instance.ChangeScene(tp.sceneName);
        }
    }
    protected virtual void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.Movement;
        rb.linearVelocity = input * moveSpeed;
    }
    protected virtual void ExecAbility(){}

    
}