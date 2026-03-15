using static GameConstants;
using UnityEngine;

public abstract class Entity: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected float maxHealth = 5;
    [SerializeField] protected float moveSpeed = PLAYER_SPEED;
    

    [Header("Combat")]
    [SerializeField] protected float invulnerabilityTime = 0.5f;
    

    protected float currentHealth {get; private set;} 
    protected float damage {get; private set;}
    protected bool isInvencible {get; private set;} 
    protected bool isDead {get; private set;} 

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        isInvencible = false;
        isDead = false;
        damage = PLAYER_DAMAGE;
    }

    public virtual void TakeDamage(float amount){}
    
    public virtual void Heal(float amount){}
    
    protected abstract void OnDeath();
}