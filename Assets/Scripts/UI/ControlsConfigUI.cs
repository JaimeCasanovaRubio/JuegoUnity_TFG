using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI para configurar los controles del juego.
/// Busca los botones automáticamente por nombre.
/// </summary>
public class ControlsConfigUI : MonoBehaviour
{
    // Botones - se buscan automáticamente
    private Button moveLeftButton;
    private Button moveRightButton;
    private Button moveUpButton;
    private Button moveDownButton;
    private Button attackButton;
    private Button abilityButton;
    private Button pauseButton;
    private Button resetButton;
    private Button backButton;

    private Button currentRebindButton;
    private string currentAction;
    private bool waitingForKey = false;

    private void Start()
    {
        // Buscar botones por nombre en la escena
        FindButtons();
        
        // Asignar listeners
        SetupListeners();
        
        // Mostrar teclas actuales
        RefreshUI();
    }

    /// <summary>
    /// Busca los botones automáticamente por nombre.
    /// </summary>
    private void FindButtons()
    {
        // Buscar todos los botones con estos nombres posibles
        moveLeftButton = FindButtonByName("LeftMovButton");
        moveRightButton = FindButtonByName("RightMovButton");
        moveUpButton = FindButtonByName("UpMovButton");
        moveDownButton = FindButtonByName("DownMovButton");
        attackButton = FindButtonByName("AtkButton");
        abilityButton = FindButtonByName("AbilityButton");
        
        
        Debug.Log($"Botones encontrados: MoveLeft={moveLeftButton != null}, Attack={attackButton != null}, Reset={resetButton != null}");
    }

    /// <summary>
    /// Busca un botón por varios nombres posibles.
    /// </summary>
    private Button FindButtonByName(params string[] possibleNames)
    {
        foreach (string name in possibleNames)
        {
            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                Button btn = obj.GetComponent<Button>();
                if (btn != null) return btn;
            }
        }
        return null;
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
        backButton?.onClick.AddListener(GoBack);
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

        var tmpText = button.GetComponentInChildren<TMP_Text>();
        if (tmpText != null) { tmpText.text = text; return; }

        var normalText = button.GetComponentInChildren<Text>();
        if (normalText != null) normalText.text = text;
    }

    private void ResetToDefaults()
    {
        KeyBindings.Instance?.ResetToDefaults();
        RefreshUI();
    }

    private void GoBack()
    {
        GameManager.Instance?.ChangeScene("SettingsMenu");
    }
}
