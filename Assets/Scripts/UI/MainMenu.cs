using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlador del menú principal.
/// Equivalente a MenuScreen.java de LibGDX.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuUI;

    private void Start()
    {
        Debug.Log("[MainMenu] Start - Buscando UI persistente...");
        // Configurar los eventos de "Atrás" de los menús persistentes
        if (GameManager.Instance != null)
        {
            // Usar GetComponentInChildren por si el script no está en la raíz del prefab
            var controlsUI = GameManager.Instance.ControlsCanvas?.GetComponentInChildren<ControlsConfigUI>(true);
            if (controlsUI != null)
            {
                Debug.Log("[MainMenu] Vinculando Back de Controles");
                // Importante: Eliminar listeners previos para evitar duplicados si se recarga la escena
                controlsUI.onBackEvent.RemoveListener(OnBackFromMenu);
                controlsUI.onBackEvent.AddListener(OnBackFromMenu);
            }
            else
            {
                Debug.LogWarning("[MainMenu] No se encontró ControlsConfigUI en el canvas persistente (o sus hijos).");
            }

            var gameSelectorUI = GameManager.Instance.GameSelectorCanvas?.GetComponentInChildren<GameSelector>(true);
            if (gameSelectorUI != null)
            {
                gameSelectorUI.onBackEvent.RemoveListener(OnBackFromMenu);
                gameSelectorUI.onBackEvent.AddListener(OnBackFromMenu);
            }
        
        }
        else
        {
            Debug.LogWarning("[MainMenu] GameManager.Instance es null en Start.");
        }
    }

    public void OnPlayClicked()
    {
        Debug.Log("▶️ Iniciando juego...");
        if (GameManager.Instance != null && GameManager.Instance.GameSelectorCanvas != null)
        {
            mainMenuUI.SetActive(false);
            GameManager.Instance.GameSelectorCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("[MainMenu] No se pudo encontrar el canvas de Seleccion de Partida persistente.");
        }
        
    }

    public void OnControlsClicked()
    {
        if (GameManager.Instance != null && GameManager.Instance.ControlsCanvas != null)
        {
            mainMenuUI.SetActive(false);
            GameManager.Instance.ControlsCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError("[MainMenu] No se pudo encontrar el canvas de Controles persistente.");
        }
    }

    private void OnBackFromMenu()
    {
        Debug.Log("[MainMenu] Regresando al menú principal...");
        
        // Desactivar explícitamente los canvas persistentes
        if (GameManager.Instance != null)
        {   
            if (GameManager.Instance.GameSelectorCanvas != null) GameManager.Instance.GameSelectorCanvas.SetActive(false);
            if (GameManager.Instance.ControlsCanvas != null) GameManager.Instance.ControlsCanvas.SetActive(false);
            if (GameManager.Instance.SelectCharCanvas != null) GameManager.Instance.SelectCharCanvas.SetActive(false);
        }
        
        mainMenuUI.SetActive(true);
    }
    public void OnQuitClicked()
    {
        Debug.Log("[MainMenu] Saliendo del juego...");
        if (GameManager.Instance != null) GameManager.Instance.QuitGame();
    }
}
