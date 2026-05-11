using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public abstract class Player:Entity
{
    public static Player Instance { get; set; }

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
    [SerializeField]public bool Afinidad0 = true; // Sin afinidad (base, siempre disponible)

    [SerializeField]public string Armazon = "Armazon1";
    [SerializeField]public string Afinidad = "Afinidad0";

    public Sprite CurrentDesignSprite { get; private set; }

    [Header("Efecto visual curación (Afinidad1)")]
    [SerializeField] private GameObject fxVida;

    [Header("Ranged Attack")]
    [SerializeField] protected GameObject[] projectilePrefabs;
    [SerializeField] protected float projectileSpeed = 10f;
    [SerializeField] protected Transform firePoint;
    protected Vector2 lastFacingDirection = Vector2.right;

    public float AbilityCooldownRemaining => Mathf.Max(0, abilityCooldownTimer);
    public float AbilityCooldownTotal => abilityCooldownDuration;
    
    public bool attacking {get; protected set;} = false;

    protected SpriteRenderer sp;

    [SerializeField] protected PlayerSound playerSound;

    bool step1= false;
    [Header("Footstep Settings")]
    [SerializeField] private float footstepInterval = 0.1f;
    private float footstepTimer = 0f;

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
        sp = GetComponent<SpriteRenderer>();
        Armazon = "Armazon1";
        Afinidad = "Afinidad0";
        animator = GetComponentInChildren<Animator>();
        SceneManager.sceneLoaded += OnPlayerSceneLoaded;
        lastArmazonAfinidad = Armazon + "_" + Afinidad;
        ActivateDesign();
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
        if(CurrentHealth <= 0) {
            Debug.Log("Player died");
            OnDeath();
        }
        if (tpCooldown > 0) tpCooldown -= Time.deltaTime;
        ProveArmazon();
        ProveAfinidad();
        CheckDesignChange();
        HandleMovement();
        UpdateAbilityCooldown();
        ExecAbility();
        if (animator != null) animator.SetBool("IsAttacking", attacking);
        if(InputHandler.Instance.AttackPressed)
        {
            if (playerSound != null) playerSound.PlayAttack();
            ExecAttack();
        }
    }

    protected void ProveArmazon(){
        switch (Armazon){
            case "Armazon1":
                IsRanged = false;
                IsMelee = true;
                break;
            case "Armazon2":
                IsRanged = true;
                IsMelee = false;
                break;
            case "Armazon3":
                IsRanged = true;
                IsMelee = false;
                break;
        }
        
    }
    protected void ProveAfinidad(){
        switch (Afinidad){
            case "Afinidad0":
                break;
            case "Afinidad1":
                break;
            case "Afinidad2":
                break;
            case "Afinidad3":
                break;
        }

    }

    private void CheckDesignChange()
    {
        string currentKey = Armazon + "_" + Afinidad;
        if (currentKey != lastArmazonAfinidad)
        {
            lastArmazonAfinidad = currentKey;
            ActivateDesign();
        }
    }

    public virtual void ActivateDesign()
    {
        if (verdugo_base == null) return;

        SetDesignActive(verdugo_base, false);
        SetDesignActive(verdugo_af1, false);
        SetDesignActive(verdugo_af2, false);
        SetDesignActive(verdugo_af3, false);
        SetDesignActive(garras_base, false);
        SetDesignActive(garras_af1, false);
        SetDesignActive(garras_af2, false);
        SetDesignActive(garras_af3, false);
        SetDesignActive(alambique_base, false);
        SetDesignActive(alambique_af1, false);
        SetDesignActive(alambique_af2, false);
        SetDesignActive(alambique_af3, false);

        GameObject designToActivate = null;
        Sprite portraitToSet = null;

        switch (Armazon)
        {
            case "Armazon1":
                switch (Afinidad)
                {
                    case "Afinidad0": designToActivate = verdugo_base;  portraitToSet = retrato_verdugo_base;  break;
                    case "Afinidad1": designToActivate = verdugo_af1;   portraitToSet = retrato_verdugo_af1;   break;
                    case "Afinidad2": designToActivate = verdugo_af2;   portraitToSet = retrato_verdugo_af2;   break;
                    case "Afinidad3": designToActivate = verdugo_af3;   portraitToSet = retrato_verdugo_af3;   break;
                }
                break;
            case "Armazon2":
                switch (Afinidad)
                {
                    case "Afinidad0": designToActivate = garras_base;   portraitToSet = retrato_garras_base;   break;
                    case "Afinidad1": designToActivate = garras_af1;    portraitToSet = retrato_garras_af1;    break;
                    case "Afinidad2": designToActivate = garras_af2;    portraitToSet = retrato_garras_af2;    break;
                    case "Afinidad3": designToActivate = garras_af3;    portraitToSet = retrato_garras_af3;    break;
                }
                break;
            case "Armazon3":
                switch (Afinidad)
                {
                    case "Afinidad0": designToActivate = alambique_base;  portraitToSet = retrato_alambique_base;  break;
                    case "Afinidad1": designToActivate = alambique_af1;   portraitToSet = retrato_alambique_af1;   break;
                    case "Afinidad2": designToActivate = alambique_af2;   portraitToSet = retrato_alambique_af2;   break;
                    case "Afinidad3": designToActivate = alambique_af3;   portraitToSet = retrato_alambique_af3;   break;
                }
                break;
        }

        if (portraitToSet != null) CurrentDesignSprite = portraitToSet;

        SetDesignActive(designToActivate, true);
        if (designToActivate != null)
            animator = designToActivate.GetComponent<Animator>();
    }

    private void SetDesignActive(GameObject design, bool active)
    {
        if (design != null)
        {
            design.SetActive(active);
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
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if(step1){
                    if (playerSound != null) playerSound.PlayFootstep1();
                }else{
                    if (playerSound != null) playerSound.PlayFootstep2();
                }
                step1 = !step1;
                footstepTimer = footstepInterval;
            }

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
    protected virtual void ExecAbility(){
        if(InputHandler.Instance.AbilityPressed)
        {   
            if(CanUseAbility())
            {
                StartAbilityCooldown();
                if (playerSound != null) playerSound.PlayAbility();
            }
        }
    }
    protected virtual void MeleeAttack(){
        isInvencible = true;
        attacking = true;
        StartCoroutine(EndInvulnerability(invulnerabilityTime));
        StartCoroutine(EndAttack(attackStorageTimer));    
    }
    protected virtual void RangedAttack(){
        isInvencible = true;
        StartCoroutine(EndInvulnerability(invulnerabilityTime));

        if (Armazon == "Armazon3")
            FirePlaced();
        else
            FireSingle(lastFacingDirection);
    }

    private void FireSingle(Vector2 direction)
    {
        GameObject prefab = projectilePrefabs != null && projectilePrefabs.Length > 0 ? projectilePrefabs[0] : null;
        if (prefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject proj = Instantiate(prefab, spawnPos, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        proj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Projectile projectileScript = proj.GetComponentInChildren<Projectile>();
        if (projectileScript != null)
            projectileScript.Setup(direction, projectileSpeed, damage, gameObject);
    }

    private void FirePlaced()
    {
        GameObject prefab = projectilePrefabs != null && projectilePrefabs.Length > 1 ? projectilePrefabs[1] : null;
        if (prefab == null) return;

        float radius = 2.0f;
        Vector3 basePos = firePoint != null ? firePoint.position : transform.position;
        Vector3 spawnPos = basePos + (Vector3)(lastFacingDirection * radius);
        GameObject proj = Instantiate(prefab, spawnPos, Quaternion.identity);
        Projectile projectileScript = proj.GetComponentInChildren<Projectile>();
        if (projectileScript != null)
            projectileScript.Setup(lastFacingDirection, 0f, damage, gameObject);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemie"))
        {
            if(attacking){
                Enemie enemie = collision.gameObject.GetComponent<Enemie>();
                enemie.TakeDamage(damage, Afinidad);
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
                if (playerSound != null) playerSound.PlayTP();
                if (!string.IsNullOrEmpty(tp.targetRoomId))
                {
                    GameManager.Instance.ChangeScene(tp.sceneName, tp.targetRoomId, tp.index);
                }
                else
                {
                    GameManager.Instance.ChangeScene(tp.sceneName, tp.index);
                }
            }
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
            PlayerPrefs.Save();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Armazon3")){
            Armazon3 = true;
            Armazon = "Armazon3";
            PlayerPrefs.SetInt("Armazon3_G"+GameSelector.gameSelected, 1);
            PlayerPrefs.Save();
            Destroy(collision.gameObject);
        }if(collision.gameObject.CompareTag("Afinidad1")){
            Afinidad1 = true;
            Afinidad = "Afinidad1";
            PlayerPrefs.SetInt("Afinidad1_G"+GameSelector.gameSelected, 1);
            PlayerPrefs.Save();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Afinidad2")){
            Afinidad2 = true;
            Afinidad = "Afinidad2";
            PlayerPrefs.SetInt("Afinidad2_G"+GameSelector.gameSelected, 1);
            PlayerPrefs.Save();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("Afinidad3")){
            Afinidad3 = true;
            Afinidad = "Afinidad3";
            PlayerPrefs.SetInt("Afinidad3_G"+GameSelector.gameSelected, 1);
            PlayerPrefs.Save();
            Destroy(collision.gameObject);
        }
    }
    public void ShowHealFx()
    {
        if (fxVida != null) StartCoroutine(ShowFxBriefly(fxVida, 1f));
    }
    private IEnumerator ShowFxBriefly(GameObject fx, float duration)
    {
        fx.SetActive(true);
        yield return new WaitForSeconds(duration);
        fx.SetActive(false);
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
    protected override void OnDeath()  
    {   
        if(playerSound != null) playerSound.PlayHurt();
        if(GameManager.Instance.DeadScreenCanvas != null){
            Debug.Log("DeadScreen found");
            Destroy(gameObject);
            Time.timeScale = 0f;
            GameManager.Instance.DeadScreenCanvas.SetActive(true);
        }else{
            Debug.Log("DeadScreen not found");
        }
    }  
}