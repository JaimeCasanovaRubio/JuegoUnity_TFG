using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SettingsConfigUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button volverBaseButton;

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

    private void SetupListeners()
    {
        backButton?.onClick.AddListener(OnBackClicked);
        controlsButton?.onClick.AddListener(OnControlsClicked);
        volverBaseButton?.onClick.AddListener(OnVolverBaseClicked);
    }

    private void OnBackClicked()
    {
        GameManager.Instance.ChangeView();
        GameManager.Instance.SettingsCanvas.SetActive(false);
    }

    private void OnControlsClicked()
    {
        var controlsUI = GameManager.Instance.ControlsCanvas?.GetComponentInChildren<ControlsConfigUI>(true);
        controlsUI.onBackEvent.RemoveAllListeners();
        controlsUI.onBackEvent.AddListener(OnControlsBackClicked);


        GameManager.Instance.SettingsCanvas.SetActive(false);
        GameManager.Instance.ControlsCanvas.SetActive(true);
    }
    private void OnControlsBackClicked()
    {
        GameManager.Instance.ControlsCanvas.SetActive(false);
        GameManager.Instance.SettingsCanvas.SetActive(true);
    }

    public void OnVolverBaseClicked()
    {
        GameManager.Instance.SettingsCanvas.SetActive(false);
        GameManager.Instance.ChangeView();
        GameManager.Instance.ChangeScene(GameManager.Instance.baseScene);
    }
}