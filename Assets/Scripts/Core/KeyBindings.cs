using UnityEngine;

/// <summary>
/// Gestiona los controles configurables del juego.
/// Equivalente a KeyBindings.java de LibGDX.
/// Usa PlayerPrefs de Unity para persistencia.
/// </summary>
public class KeyBindings : MonoBehaviour
{
    private static KeyBindings _instance;
    public static KeyBindings Instance
    {
        get
        {
            // Si no existe, crearlo automáticamente
            if (_instance == null)
            {
                // Buscar si ya existe uno en la escena
                _instance = FindObjectOfType<KeyBindings>();
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject("KeyBindings");
                    _instance = obj.AddComponent<KeyBindings>();
                    // Cargar bindings inmediatamente al crear desde el getter
                    _instance.LoadBindingsInternal();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    


    // Claves para PlayerPrefs
    private const string KEY_MOVE_LEFT = "key_move_left";
    private const string KEY_MOVE_RIGHT = "key_move_right";
    private const string KEY_MOVE_UP = "key_move_up";
    private const string KEY_MOVE_DOWN = "key_move_down";
    private const string KEY_ATTACK = "key_attack";
    private const string KEY_ABILITY = "key_ability";
    private const string KEY_PAUSE = "key_pause";

    // Valores por defecto
    private const KeyCode DEFAULT_MOVE_LEFT = KeyCode.A;
    private const KeyCode DEFAULT_MOVE_RIGHT = KeyCode.D;
    private const KeyCode DEFAULT_MOVE_UP = KeyCode.W;
    private const KeyCode DEFAULT_MOVE_DOWN = KeyCode.S;
    private const KeyCode DEFAULT_ATTACK = KeyCode.J;
    private const KeyCode DEFAULT_ABILITY = KeyCode.K;
    private const KeyCode DEFAULT_PAUSE = KeyCode.Escape;

    // Controles actuales
    public KeyCode MoveLeft { get; private set; }
    public KeyCode MoveRight { get; private set; }
    public KeyCode MoveUp { get; private set; }
    public KeyCode MoveDown { get; private set; }
    public KeyCode Attack { get; private set; }
    public KeyCode Ability { get; private set; }
    public KeyCode Pause { get; private set; }

    private bool isInitialized = false;

    private void Awake()
    {
        // Singleton pattern mejorado
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        if (!isInitialized)
        {
            LoadBindingsInternal();
            isInitialized = true;
        }
    }

    private void OnEnable()
    {
        // Asegurar que siempre tenga los bindings cargados
        if (!isInitialized)
        {
            LoadBindingsInternal();
            isInitialized = true;
        }
    }

    /// <summary>
    /// Carga los controles desde PlayerPrefs.
    /// </summary>
    private void LoadBindingsInternal()
    {
        // DEBUG: Borrar PlayerPrefs corruptos para forzar valores por defecto
        // NOTA: Eliminar esta línea después de probar
        PlayerPrefs.DeleteAll();
        
        MoveLeft = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_LEFT, (int)DEFAULT_MOVE_LEFT);
        MoveRight = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_RIGHT, (int)DEFAULT_MOVE_RIGHT);
        MoveUp = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_UP, (int)DEFAULT_MOVE_UP);
        MoveDown = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_DOWN, (int)DEFAULT_MOVE_DOWN);
        Attack = (KeyCode)PlayerPrefs.GetInt(KEY_ATTACK, (int)DEFAULT_ATTACK);
        Ability = (KeyCode)PlayerPrefs.GetInt(KEY_ABILITY, (int)DEFAULT_ABILITY);
        Pause = (KeyCode)PlayerPrefs.GetInt(KEY_PAUSE, (int)DEFAULT_PAUSE);
        
        Debug.Log($"[KeyBindings] Loaded - Pause key: {Pause}");
        isInitialized = true;
    }

    /// <summary>
    /// Guarda los controles actuales en PlayerPrefs.
    /// </summary>
    public void SaveBindings()
    {
        PlayerPrefs.SetInt(KEY_MOVE_LEFT, (int)MoveLeft);
        PlayerPrefs.SetInt(KEY_MOVE_RIGHT, (int)MoveRight);
        PlayerPrefs.SetInt(KEY_MOVE_UP, (int)MoveUp);
        PlayerPrefs.SetInt(KEY_MOVE_DOWN, (int)MoveDown);
        PlayerPrefs.SetInt(KEY_ATTACK, (int)Attack);
        PlayerPrefs.SetInt(KEY_ABILITY, (int)Ability);
        PlayerPrefs.SetInt(KEY_PAUSE, (int)Pause);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Restaura los controles a sus valores por defecto.
    /// </summary>
    public void ResetToDefaults()
    {
        MoveLeft = DEFAULT_MOVE_LEFT;
        MoveRight = DEFAULT_MOVE_RIGHT;
        MoveUp = DEFAULT_MOVE_UP;
        MoveDown = DEFAULT_MOVE_DOWN;
        Attack = DEFAULT_ATTACK;
        Ability = DEFAULT_ABILITY;
        Pause = DEFAULT_PAUSE;
        SaveBindings();
    }

    /// <summary>
    /// Asigna una nueva tecla a una acción.
    /// </summary>
    public void SetBinding(string action, KeyCode keycode)
    {
        switch (action)
        {
            case KEY_MOVE_LEFT:
                MoveLeft = keycode;
                break;
            case KEY_MOVE_RIGHT:
                MoveRight = keycode;
                break;
            case KEY_MOVE_UP:
                MoveUp = keycode;
                break;
            case KEY_MOVE_DOWN:
                MoveDown = keycode;
                break;
            case KEY_ATTACK:
                Attack = keycode;
                break;
            case KEY_ABILITY:
                Ability = keycode;
                break;
            case KEY_PAUSE:
                Pause = keycode;
                break;
        }
        SaveBindings();
    }
}
