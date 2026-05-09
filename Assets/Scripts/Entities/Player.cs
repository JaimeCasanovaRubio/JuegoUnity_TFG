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

    [Header("Diseños en juego - Verdugo de Titanes (Armazon1)")]
    [SerializeField] private GameObject verdugo_base;
    [SerializeField] private GameObject verdugo_af1;
    [SerializeField] private GameObject verdugo_af2;
    [SerializeField] private GameObject verdugo_af3;

    [Header("Diseños en juego - Garras de Umbra (Armazon2)")]
    [SerializeField] private GameObject garras_base;
    [SerializeField] private GameObject garras_af1;
    [SerializeField] private GameObject garras_af2;
    [SerializeField] private GameObject garras_af3;

    [Header("Diseños en juego - El Alambique (Armazon3)")]
    [SerializeField] private GameObject alambique_base;
    [SerializeField] private GameObject alambique_af1;
    [SerializeField] private GameObject alambique_af2;
    [SerializeField] private GameObject alambique_af3;

    [Header("Retratos para el libro (sprite estático por combinación)")]
    [SerializeField] private Sprite retrato_verdugo_base;
    [SerializeField] private Sprite retrato_verdugo_af1;
    [SerializeField] private Sprite retrato_verdugo_af2;
    [SerializeField] private Sprite retrato_verdugo_af3;
    [SerializeField] private Sprite retrato_garras_base;
    [SerializeField] private Sprite retrato_garras_af1;
    [SerializeField] private Sprite retrato_garras_af2;
    [SerializeField] private Sprite retrato_garras_af3;
    [SerializeField] private Sprite retrato_alambique_base;
    [SerializeField] private Sprite retrato_alambique_af1;
    [SerializeField] private Sprite retrato_alambique_af2;
    [SerializeField] private Sprite retrato_alambique_af3;

    private string lastArmazonAfinidad = "";

    [Header("Armazones")]
    [SerializeField]public bool Armazon1 = true;  // Verdugo de Titanes (base, desbloqueado)
    [SerializeField]public bool Armazon2 = false; // Garras de Umbra
    [SerializeField]public bool Armazon3 = false; // El Alambique

    [Header("Afinidades")]
    [SerializeField]public bool Afinidad1 = true;
    [SerializeField]public bool Afinidad2 = false;
    [SerializeField]public bool Afinidad3 = false;
    [SerializeField]public bool Afinidad4 = false;

    [SerializeField]public string Armazon = "Armazon1";
    [SerializeField]public string Afinidad = "";  // "" = sin afinidad (muestra sprite base)

    public Sprite CurrentDesignSprite { get; private set; }

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
        animator = GetComponentInChildren<Animator>();
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
        ProveArmazon();
        ProveAfinidad();
        HandleMovement();
        UpdateAbilityCooldown();
        ExecAbility();
        if(InputHandler.Instance.AttackPressed)
        {
            ExecAttack();
        }
    }

    protected void ProveArmazon(){
        switch (Armazon){
            case "Armazon1":
                IsRanged = true;
                IsMelee = false;
                break;
            case "Armazon2":
                IsRanged = false;
                IsMelee = true;
                break;
            case "Armazon3":
                IsRanged = true;
                IsMelee = false;
                break;
        }
        UpdateDesign();
    }
    protected void ProveAfinidad(){
        switch (Afinidad){
            case "Afinidad1":
                break;
            case "Afinidad2":
                break;
            case "Afinidad3":
                break;
            case "Afinidad4":
                break;
        }
        UpdateDesign();
    }

    private void UpdateDesign()
    {
        string combo = Armazon + "_" + Afinidad;
        if (combo == lastArmazonAfinidad) return;
        lastArmazonAfinidad = combo;

        GameObject diseñoActivo = (Armazon, Afinidad) switch
        {
            ("Armazon1", "")          => verdugo_base,
            ("Armazon1", "Afinidad1") => verdugo_af1,
            ("Armazon1", "Afinidad2") => verdugo_af2,
            ("Armazon1", "Afinidad3") => verdugo_af3,
            ("Armazon2", "")          => garras_base,
            ("Armazon2", "Afinidad1") => garras_af1,
            ("Armazon2", "Afinidad2") => garras_af2,
            ("Armazon2", "Afinidad3") => garras_af3,
            ("Armazon3", "")          => alambique_base,
            ("Armazon3", "Afinidad1") => alambique_af1,
            ("Armazon3", "Afinidad2") => alambique_af2,
            ("Armazon3", "Afinidad3") => alambique_af3,
            _                         => verdugo_base
        };

        GameObject[] todos = {
            verdugo_base, verdugo_af1, verdugo_af2, verdugo_af3,
            garras_base,  garras_af1,  garras_af2,  garras_af3,
            alambique_base, alambique_af1, alambique_af2, alambique_af3
        };
        foreach (var d in todos)
            if (d != null) d.SetActive(false);

        if (diseñoActivo != null)
        {
            diseñoActivo.SetActive(true);
            Animator childAnimator = diseñoActivo.GetComponent<Animator>();
            if (childAnimator != null) animator = childAnimator;
        }

        CurrentDesignSprite = (Armazon, Afinidad) switch
        {
            ("Armazon1", "")          => retrato_verdugo_base,
            ("Armazon1", "Afinidad1") => retrato_verdugo_af1,
            ("Armazon1", "Afinidad2") => retrato_verdugo_af2,
            ("Armazon1", "Afinidad3") => retrato_verdugo_af3,
            ("Armazon2", "")          => retrato_garras_base,
            ("Armazon2", "Afinidad1") => retrato_garras_af1,
            ("Armazon2", "Afinidad2") => retrato_garras_af2,
            ("Armazon2", "Afinidad3") => retrato_garras_af3,
            ("Armazon3", "")          => retrato_alambique_base,
            ("Armazon3", "Afinidad1") => retrato_alambique_af1,
            ("Armazon3", "Afinidad2") => retrato_alambique_af2,
            ("Armazon3", "Afinidad3") => retrato_alambique_af3,
            _                         => retrato_verdugo_base
        };
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
                Time.timeScale = 0f;
            }
        }
        if(collision.gameObject.CompareTag("Armazon2")){
            Armazon2 = true;
            Armazon = "Armazon2";
            PlayerPrefs.SetInt("Armazon2_G"+GameSelector.gameSelected, 1);
            Destroy(collision.gameObject);
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