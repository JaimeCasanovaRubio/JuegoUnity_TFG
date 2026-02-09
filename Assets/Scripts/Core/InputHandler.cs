using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Clase que centraliza el manejo de input del jugador.
/// Procesa las teclas configurables desde KeyBindings.
/// </summary>
public class InputHandler : MonoBehaviour
{
    private static InputHandler _instance;
    public static InputHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputHandler>();
                
                if (_instance == null)
                {
                    GameObject obj = new GameObject("InputHandler");
                    _instance = obj.AddComponent<InputHandler>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // Estado del input
    private Vector2 movement;
    private bool attackPressed;
    private bool abilityPressed;
    private bool pausePressed;
    
    // Dirección del jugador
    private bool facingRight = true;
    private string lastDirection = "right";

    // Properties públicas para acceder al estado del input
    public Vector2 Movement => movement;
    public bool AttackPressed => attackPressed;
    public bool AbilityPressed => abilityPressed;
    public bool PausePressed => pausePressed;
    public bool FacingRight => facingRight;
    public string LastDirection => lastDirection;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        ProcessInput();
    }

    /// <summary>
    /// Procesa toda la entrada del jugador usando KeyBindings configurables.
    /// Compatible con el nuevo Input System.
    /// </summary>
    private void ProcessInput()
    {
        // Resetear inputs de un solo frame
        attackPressed = false;
        abilityPressed = false;
        pausePressed = false;

        // Verificar que KeyBindings esté disponible
        if (KeyBindings.Instance == null) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Resetear movimiento
        movement.x = 0;
        movement.y = 0;

        // Movimiento usando KeyBindings (controles configurables)
        if (IsKeyPressed(keyboard, KeyBindings.Instance.MoveLeft))
        {
            movement.x = -1;
            lastDirection = "left";
            facingRight = false;
        }
        if (IsKeyPressed(keyboard, KeyBindings.Instance.MoveRight))
        {
            movement.x = 1;
            lastDirection = "right";
            facingRight = true;
        }
        if (IsKeyPressed(keyboard, KeyBindings.Instance.MoveUp))
        {
            movement.y = 1;
            lastDirection = "up";
        }
        if (IsKeyPressed(keyboard, KeyBindings.Instance.MoveDown))
        {
            movement.y = -1;
            lastDirection = "down";
        }

        // Ataque usando KeyBindings
        if (WasKeyPressedThisFrame(keyboard, KeyBindings.Instance.Attack))
        {
            attackPressed = true;
        }

        // Habilidad usando KeyBindings
        if (WasKeyPressedThisFrame(keyboard, KeyBindings.Instance.Ability))
        {
            abilityPressed = true;
        }

        // Pausa usando KeyBindings
        if (IsKeyPressed(keyboard, KeyBindings.Instance.Pause))
        {
            Debug.Log("Pause pressed");
            pausePressed = true;
        }

        // Normalizar para evitar movimiento diagonal más rápido
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
    }

    /// <summary>
    /// Comprueba si una tecla está presionada usando el nuevo Input System.
    /// </summary>
    public bool IsKeyPressed(Keyboard keyboard, KeyCode keyCode)
    {
        var key = GetKeyFromKeyCode(keyboard, keyCode);
        return key != null && key.isPressed;
    }

    /// <summary>
    /// Comprueba si una tecla fue presionada este frame.
    /// </summary>
    public bool WasKeyPressedThisFrame(Keyboard keyboard, KeyCode keyCode)
    {
        var key = GetKeyFromKeyCode(keyboard, keyCode);
        return key != null && key.wasPressedThisFrame;
    }

    /// <summary>
    /// Comprueba si una tecla está presionada (versión simplificada).
    /// </summary>
    public bool IsKeyPressed(KeyCode keyCode)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return false;
        return IsKeyPressed(keyboard, keyCode);
    }

    /// <summary>
    /// Comprueba si una tecla fue presionada este frame (versión simplificada).
    /// </summary>
    public bool WasKeyPressedThisFrame(KeyCode keyCode)
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return false;
        return WasKeyPressedThisFrame(keyboard, keyCode);
    }

    /// <summary>
    /// Convierte un KeyCode al equivalente del nuevo Input System.
    /// </summary>
    public UnityEngine.InputSystem.Controls.KeyControl GetKeyFromKeyCode(Keyboard keyboard, KeyCode keyCode)
    {
        return keyCode switch
        {
            KeyCode.A => keyboard.aKey,
            KeyCode.B => keyboard.bKey,
            KeyCode.C => keyboard.cKey,
            KeyCode.D => keyboard.dKey,
            KeyCode.E => keyboard.eKey,
            KeyCode.F => keyboard.fKey,
            KeyCode.G => keyboard.gKey,
            KeyCode.H => keyboard.hKey,
            KeyCode.I => keyboard.iKey,
            KeyCode.J => keyboard.jKey,
            KeyCode.K => keyboard.kKey,
            KeyCode.L => keyboard.lKey,
            KeyCode.M => keyboard.mKey,
            KeyCode.N => keyboard.nKey,
            KeyCode.O => keyboard.oKey,
            KeyCode.P => keyboard.pKey,
            KeyCode.Q => keyboard.qKey,
            KeyCode.R => keyboard.rKey,
            KeyCode.S => keyboard.sKey,
            KeyCode.T => keyboard.tKey,
            KeyCode.U => keyboard.uKey,
            KeyCode.V => keyboard.vKey,
            KeyCode.W => keyboard.wKey,
            KeyCode.X => keyboard.xKey,
            KeyCode.Y => keyboard.yKey,
            KeyCode.Z => keyboard.zKey,
            KeyCode.Space => keyboard.spaceKey,
            KeyCode.LeftShift => keyboard.leftShiftKey,
            KeyCode.RightShift => keyboard.rightShiftKey,
            KeyCode.LeftControl => keyboard.leftCtrlKey,
            KeyCode.RightControl => keyboard.rightCtrlKey,
            KeyCode.LeftAlt => keyboard.leftAltKey,
            KeyCode.RightAlt => keyboard.rightAltKey,
            KeyCode.Escape => keyboard.escapeKey,
            KeyCode.Tab => keyboard.tabKey,
            KeyCode.Backspace => keyboard.backspaceKey,
            KeyCode.Return => keyboard.enterKey,
            KeyCode.UpArrow => keyboard.upArrowKey,
            KeyCode.DownArrow => keyboard.downArrowKey,
            KeyCode.LeftArrow => keyboard.leftArrowKey,
            KeyCode.RightArrow => keyboard.rightArrowKey,
            KeyCode.Alpha0 => keyboard.digit0Key,
            KeyCode.Alpha1 => keyboard.digit1Key,
            KeyCode.Alpha2 => keyboard.digit2Key,
            KeyCode.Alpha3 => keyboard.digit3Key,
            KeyCode.Alpha4 => keyboard.digit4Key,
            KeyCode.Alpha5 => keyboard.digit5Key,
            KeyCode.Alpha6 => keyboard.digit6Key,
            KeyCode.Alpha7 => keyboard.digit7Key,
            KeyCode.Alpha8 => keyboard.digit8Key,
            KeyCode.Alpha9 => keyboard.digit9Key,
            _ => null
        };
    }
}
