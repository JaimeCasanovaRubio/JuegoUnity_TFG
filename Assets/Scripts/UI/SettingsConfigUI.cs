using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SettingsConfigUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backButton;

    [Header("Navigation")]
    public UnityEvent onBackEvent = new UnityEvent();

    private void Start()
    {
        // Intentar encontrar el botón de atrás si no está asignado
        if (backButton == null)
        {
            backButton = transform.Find("BackButton")?.GetComponent<Button>();
            if (backButton == null) backButton = transform.Find("Back")?.GetComponent<Button>();
            
            if (backButton != null) Debug.Log($"[SettingsUI] Botón 'Back' encontrado automáticamente: {backButton.name}");
        }

        SetupListeners();
    }

    private void OnControlsClicked()
    {
          if (GameManager.Instance != null && GameManager.Instance.ControlsCanvas != null)
        {
            GameManager.Instance.SettingsCanvas.SetActive(false);
            GameManager.Instance.ControlsCanvas.SetActive(true);
        }
        else    
        {
            Debug.LogError("[MainMenu] No se pudo encontrar el canvas de Controles persistente.");
        }
    }

    private void SetupListeners()
    {
        backButton?.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        Debug.Log("[SettingsUI] Back clicked!");
        onBackEvent?.Invoke();
    }
}