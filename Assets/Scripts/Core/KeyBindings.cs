using UnityEngine;

/// <summary>
/// Gestiona los controles configurables del juego.
/// Equivalente a KeyBindings.java de LibGDX.
/// Usa PlayerPrefs de Unity para persistencia.
/// </summary>
public class KeyBindings : MonoBehaviour
{
    public static KeyBindings Instance { get; private set; }

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

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadBindings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Carga los controles desde PlayerPrefs.
    /// </summary>
    private void LoadBindings()
    {
        MoveLeft = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_LEFT, (int)DEFAULT_MOVE_LEFT);
        MoveRight = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_RIGHT, (int)DEFAULT_MOVE_RIGHT);
        MoveUp = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_UP, (int)DEFAULT_MOVE_UP);
        MoveDown = (KeyCode)PlayerPrefs.GetInt(KEY_MOVE_DOWN, (int)DEFAULT_MOVE_DOWN);
        Attack = (KeyCode)PlayerPrefs.GetInt(KEY_ATTACK, (int)DEFAULT_ATTACK);
        Ability = (KeyCode)PlayerPrefs.GetInt(KEY_ABILITY, (int)DEFAULT_ABILITY);
        Pause = (KeyCode)PlayerPrefs.GetInt(KEY_PAUSE, (int)DEFAULT_PAUSE);
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
