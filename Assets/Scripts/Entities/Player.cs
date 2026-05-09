using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class Player:Entity
{
    public static Player Instance { get; private set; }

    protected Rigidbody2D rb;
    protected Animator animator;
    
    [Header("Ability Cooldown")]
    [SerializeField] protected float abilityCooldownDuration = 2f;
    protected float abilityCooldownTimer = 0f;
    protected float attackStorageTimer = 0.5f;

    // Cooldown para evitar re-trigger del TP al spawnear
    private float tpCooldown = 0f;
    private const float TP_COOLDOWN_DURATION = 0.5f;

    protected bool IsMelee {get; private set;} = true;
    protected bool IsRanged {get; private set;} = false;

    [Header("Armazones")]
    public bool Armazon1 {get; set;} = false;
    public bool Armazon2 {get; set;} = false;
    public bool Armazon3 {get; set;} = false;

    public bool Afinidad1 {get; set;} = false;
    public bool Afinidad2 {get; set;} = false;
    public bool Afinidad3 {get; set;} = false;
    public bool Afinidad4 {get; set;} = false;
    
    public string Armazon {get; set;} = null;
    public string Afinidad {get; set;} = null;

    [Header("Ranged Attack")]
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected Transform firePoint;
    protected Vector2 lastFacingDirection = Vector2.down;

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
        animator = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnPlayerSceneLoaded;
    }

    protected virtual void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnPlayerSceneLoaded;
    }

    private void OnPlayerSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Activar cooldown al cargar escena para no re-disparar el TP
        tpCooldown = TP_COOLDOWN_DURATION;
    }
    protected virtual void Update()
    {   
        if (tpCooldown > 0) tpCooldown -= Time.deltaTime;
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
    protected virtual void HandleMovement()
    {
        Vector2 input = InputHandler.Instance.Movement;
        rb.linearVelocity = input * moveSpeed;

        if (animator == null) return;

        bool isMoving = input.magnitude > 0.1f;
        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            if (Mathf.Abs(input.y) >= Mathf.Abs(input.x))
            {
                animator.SetInteger("Direction", input.y < 0 ? 0 : 1);
                lastFacingDirection = input.y < 0 ? Vector2.down : Vector2.up;
            }
            else
            {
                animator.SetInteger("Direction", input.x < 0 ? 2 : 3);
                lastFacingDirection = input.x < 0 ? Vector2.left : Vector2.right;
            }
        }
    }
    protected virtual void ExecAttack()
    {
        if (attacking) return;

        if(IsMelee)
        {
            MeleeAttack();
        }   
        if(IsRanged)
        {
            RangedAttack();
        }   
        
    }
    protected virtual void ExecAbility(){}
    protected virtual void MeleeAttack(){
        isInvencible = true;
        attacking = true;
        StartCoroutine(EndInvulnerability(invulnerabilityTime));
        StartCoroutine(EndAttack(attackStorageTimer));    
    }
    protected virtual void RangedAttack(){
        isInvencible = true;
        StartCoroutine(EndInvulnerability(invulnerabilityTime));
        
        if (projectilePrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
            
            float angle = Mathf.Atan2(lastFacingDirection.y, lastFacingDirection.x) * Mathf.Rad2Deg;
            proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            Projectile projectileScript = proj.GetComponentInChildren<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Setup(lastFacingDirection, projectileSpeed, damage, gameObject);
            }
            else
            {
                Debug.LogError("¡ATENCIÓN! El prefab disparado no tiene el script Projectile.cs asociado.");
            }
        }
    }
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
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignorar TPs durante el cooldown post-cambio de escena
        if (collision.gameObject.CompareTag("TP")){
            if (tpCooldown > 0) return;

            Teleport tp = collision.GetComponent<Teleport>();
            if(tp!=null)
            {
                GameManager.Instance.ChangeScene(tp.sceneName, tp.index);
            }
        } 
        if(collision.gameObject.CompareTag("ranged")){
            IsRanged = true;
            IsMelee = false;
            Debug.Log("Arma cambiada aranged");
        }
        if(collision.gameObject.CompareTag("meele")){
            IsRanged = false;
            IsMelee = true;
            Debug.Log("Arma cambiada a melee");
        }
        if(collision.gameObject.CompareTag("magicBook")){
            Debug.Log("Book picked up");
            if(GameManager.Instance.MagicBookCanvas != null){
                Debug.Log("Book Canvas found");
                GameManager.Instance.MagicBookCanvas.SetActive(true);
                GameManager.Instance.TogglePause();
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