using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controlador del menú principal.
/// Equivalente a MenuScreen.java de LibGDX.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button quitButton;

    [Header("Scene Names")]
    [SerializeField] private string settingsScene = "Settings";
    [SerializeField] private string controlsScene = "Controls";

    private void Start()
    {
        // Configurar listeners de los botones
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayClicked);
        if (settingsButton != null)
            settingsButton.onClick.AddListener(OnSettingsClicked);
        if (controlsButton != null)
            controlsButton.onClick.AddListener(OnControlsClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    public void OnPlayClicked()
    {
        Debug.Log("▶️ Iniciando juego...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(GameManager.Instance.gameScene);
        }
        else
        {
            SceneManager.LoadScene(GameManager.Instance.gameScene);
        }
    }

    public void OnSettingsClicked()
    {
        Debug.Log("⚙️ Abriendo configuración...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(settingsScene);
        }
        else
        {
            SceneManager.LoadScene(settingsScene);
        }
    }

    public void OnControlsClicked()
    {
        Debug.Log("🎮 Abriendo controles...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(controlsScene);
        }
        else
        {
            SceneManager.LoadScene(controlsScene);
        }
    }

    public void OnQuitClicked()
    {
        Debug.Log("🚪 Saliendo del juego...");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitGame();
        }
        else
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
