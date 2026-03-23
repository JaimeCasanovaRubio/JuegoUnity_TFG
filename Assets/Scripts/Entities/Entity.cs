using static GameConstants;
using UnityEngine;

public abstract class Entity: MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float MaxHealth = 5;
    [SerializeField] protected float moveSpeed = PLAYER_SPEED;
    [SerializeField] public float CurrentHealth;

    

    [Header("Combat")]
    [SerializeField] protected float invulnerabilityTime = 0.2f;
    

      

    protected float damage {get; private set;}
    public bool isInvencible {get; protected set;} 
    protected bool isDead {get; private set;} 

    protected virtual void Awake()
    {
        CurrentHealth = MaxHealth;
        isInvencible = false;
        isDead = false;
        damage = PLAYER_DAMAGE;
    }

    public virtual void TakeDamage(float amount){
        if (isInvencible || isDead) return;
    
        CurrentHealth -= amount;
    
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            isDead = true;
            OnDeath();
        }   
        else
        {
            StartCoroutine(ApplyInvulnerability());
        }   
    }      
    private System.Collections.IEnumerator ApplyInvulnerability()
    {
        isInvencible = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        isInvencible = false;
    }
    
    public virtual void Heal(float amount){}
    
    protected abstract void OnDeath();
}