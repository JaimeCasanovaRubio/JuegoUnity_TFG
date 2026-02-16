using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

/// <summary>
/// UI para configurar los controles del juego.
/// Busca los botones automáticamente por nombre.
/// </summary>
public class ControlsConfigUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button attackButton;
    [SerializeField] private Button abilityButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button backButton;

    [Header("Navigation")]
    public UnityEvent onBackEvent = new UnityEvent();

    private Button currentRebindButton;
    private string currentAction;
    private bool waitingForKey = false;

    private void Start()
    {
        // Intentar encontrar el botón de atrás si no está asignado
        if (backButton == null)
        {
            backButton = transform.Find("BackButton")?.GetComponent<Button>();
            if (backButton == null) backButton = transform.Find("Back")?.GetComponent<Button>();
            
            if (backButton != null) Debug.Log($"[ControlsUI] Botón 'Back' encontrado automáticamente: {backButton.name}");
            else Debug.LogWarning("[ControlsUI] No se pudo encontrar el botón 'Back' automáticamente. Por favor, asígnalo en el inspector.");
        }

        // Asignar listeners
        SetupListeners();
        
        // Mostrar teclas actuales
        RefreshUI();
    }


    /// <summary>
    /// Configura los listeners de los botones.
    /// </summary>
    private void SetupListeners()
    {
        moveLeftButton?.onClick.AddListener(() => StartRebind(moveLeftButton, "key_move_left"));
        moveRightButton?.onClick.AddListener(() => StartRebind(moveRightButton, "key_move_right"));
        moveUpButton?.onClick.AddListener(() => StartRebind(moveUpButton, "key_move_up"));
        moveDownButton?.onClick.AddListener(() => StartRebind(moveDownButton, "key_move_down"));
        attackButton?.onClick.AddListener(() => StartRebind(attackButton, "key_attack"));
        abilityButton?.onClick.AddListener(() => StartRebind(abilityButton, "key_ability"));
        pauseButton?.onClick.AddListener(() => StartRebind(pauseButton, "key_pause"));

        resetButton?.onClick.AddListener(ResetToDefaults);
        backButton?.onClick.AddListener(OnControlsBackClicked);
    }

    private void Update()
    {
        if (waitingForKey)
        {
            // Usar el nuevo Input System para detectar teclas
            var keyboard = UnityEngine.InputSystem.Keyboard.current;
            if (keyboard == null) return;

            foreach (var key in keyboard.allKeys)
            {
                if (key.wasPressedThisFrame)
                {
                    // Convertir la tecla del nuevo Input System a KeyCode
                    KeyCode keyCode = ConvertToKeyCode(key);
                    if (keyCode != KeyCode.None)
                    {
                        CompleteRebind(keyCode);
                        break;
                    }
                }
            }
        }
    }
    

    /// <summary>
    /// Convierte una tecla del nuevo Input System a KeyCode.
    /// </summary>
    private KeyCode ConvertToKeyCode(UnityEngine.InputSystem.Controls.KeyControl key)
    {
        string keyName = key.keyCode.ToString();
        
        // Intentar parsear directamente
        if (System.Enum.TryParse<KeyCode>(keyName, true, out KeyCode result))
        {
            return result;
        }

        // Mapeos especiales para teclas que no coinciden exactamente
        return keyName switch
        {
            "Digit0" => KeyCode.Alpha0,
            "Digit1" => KeyCode.Alpha1,
            "Digit2" => KeyCode.Alpha2,
            "Digit3" => KeyCode.Alpha3,
            "Digit4" => KeyCode.Alpha4,
            "Digit5" => KeyCode.Alpha5,
            "Digit6" => KeyCode.Alpha6,
            "Digit7" => KeyCode.Alpha7,
            "Digit8" => KeyCode.Alpha8,
            "Digit9" => KeyCode.Alpha9,
            "LeftShift" => KeyCode.LeftShift,
            "RightShift" => KeyCode.RightShift,
            "LeftCtrl" => KeyCode.LeftControl,
            "RightCtrl" => KeyCode.RightControl,
            "LeftAlt" => KeyCode.LeftAlt,
            "RightAlt" => KeyCode.RightAlt,
            _ => KeyCode.None
        };
    }

    private void StartRebind(Button button, string action)
    {
        currentRebindButton = button;
        currentAction = action;
        waitingForKey = true;
        SetButtonText(button, "Presiona...");
    }

    private void CompleteRebind(KeyCode newKey)
    {
        waitingForKey = false;
        KeyBindings.Instance?.SetBinding(currentAction, newKey);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (KeyBindings.Instance == null)
        {
            Debug.LogWarning("KeyBindings.Instance no encontrado!");
            return;
        }

        SetButtonText(moveLeftButton, KeyBindings.Instance.MoveLeft.ToString());
        SetButtonText(moveRightButton, KeyBindings.Instance.MoveRight.ToString());
        SetButtonText(moveUpButton, KeyBindings.Instance.MoveUp.ToString());
        SetButtonText(moveDownButton, KeyBindings.Instance.MoveDown.ToString());
        SetButtonText(attackButton, KeyBindings.Instance.Attack.ToString());
        SetButtonText(abilityButton, KeyBindings.Instance.Ability.ToString());
        SetButtonText(pauseButton, KeyBindings.Instance.Pause.ToString());
    }

    private void SetButtonText(Button button, string text)
    {
        if (button == null) return;

        // Intentar con TextMeshPro
        var tmpText = button.GetComponentInChildren<TMP_Text>();
        if (tmpText != null) 
        { 
            tmpText.text = text; 
            return; 
        }

        // Intentar con el texto normal de Unity
        var normalText = button.GetComponentInChildren<Text>();
        if (normalText != null) 
        {
            normalText.text = text;
            return;
        }
        
        Debug.LogWarning($"[ControlsUI] No se encontró componente de texto en el botón: {button.name}");
    }

    private void ResetToDefaults()
    {
        KeyBindings.Instance?.ResetToDefaults();
        RefreshUI();
    }

    /// <summary>
    /// Maneja el cierre del menú de controles y avisa a los interesados.
    /// </summary>
    public void OnControlsBackClicked()
    {
        Debug.Log("[ControlsUI] Back clicked!");
        onBackEvent?.Invoke();
    }
}
