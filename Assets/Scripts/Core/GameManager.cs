using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador principal del juego. Equivalente a GameController.java de LibGDX.
/// Gestiona el estado global del juego, el jugador actual y la navegación entre escenas.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Current Player")]
    public Player currentPlayer;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";
    public string controlsScene = "ControlsConfig";
    public string settingsScene = "SettingsMenu";
    public string baseScene = "Base";
    public string gameScene = "OniricForest";
    public string characterSelectionScene = "CharacterSelection";

    public string currentScene;
    public string lastScene;
    public string lastPlayScene;

    // Escenas donde se puede pausar (escenas de juego)
    private string[] playableScenes;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Definir las escenas donde se puede pausar
            playableScenes = new string[] { gameScene, baseScene, "OniricForest" };
            
            // Asegurar que los singletons de input existan
            _ = KeyBindings.Instance;
            _ = InputHandler.Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        HandlePauseInput();
    }

    /// <summary>
    /// Maneja el input de pausa globalmente.
    /// Solo funciona en escenas de juego.
    /// </summary>
    private void HandlePauseInput()
    {
        // Verificar que estamos en una escena jugable
        if (!IsInPlayableScene()) return;

        // Verificar si se presionó la tecla de pausa
        if (InputHandler.Instance != null && InputHandler.Instance.PausePressed)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Comprueba si estamos en una escena donde se puede pausar.
    /// </summary>
    private bool IsInPlayableScene()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        foreach (string scene in playableScenes)
        {
            if (currentScene == scene) return true;
        }
        return false;
    }

    /// <summary>
    /// Activa/desactiva la pausa.
    /// </summary>
    public void TogglePause()
    {
        // Ir a la escena de settings (menú de pausa)
        ChangeScene(settingsScene);
    }

    /// <summary>
    /// Cambia a una nueva escena.
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        // Guardar la escena anterior
        lastScene = SceneManager.GetActiveScene().name;

        // Si es una escena de juego, guardarla
        if (sceneName == gameScene || sceneName == "Base")
        {
            lastPlayScene = sceneName;
        }

        // Si el jugador está muerto, reiniciar su estado
        if (currentPlayer != null && currentPlayer.IsDead)
        {
            SetupPlayer();
        }
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;


        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Regresa a la última escena visitada.
    /// </summary>
    public void GoToLastScene()
    {
        if (!string.IsNullOrEmpty(lastScene))
        {
            ChangeScene(lastScene);
        }
    }

    /// <summary>
    /// Regresa a la última escena de juego.
    /// </summary>
    public void GoToLastPlayScene()
    {
        if (!string.IsNullOrEmpty(lastPlayScene))
        {
            ChangeScene(lastPlayScene);
        }
    }

    /// <summary>
    /// Configura el jugador seleccionado.
    /// </summary>
    public void SetPlayer(Player player)
    {
        currentPlayer = player;
        SetupPlayer();
    }

    /// <summary>
    /// Reinicia el estado del jugador.
    /// </summary>
    private void SetupPlayer()
    {
        if (currentPlayer != null)
        {
            currentPlayer.transform.position = new Vector3(2f, 3f, 0f);
            currentPlayer.Health = currentPlayer.MaxHealth;
            currentPlayer.IsDead = false;
        }
    }

    /// <summary>
    /// Inicia el juego con el personaje seleccionado.
    /// </summary>
    public void StartGameWithCharacter(string characterType)
    {
        // Instanciar el personaje correcto basado en el tipo
        // Esto se manejará cuando cargue la escena de juego
        PlayerPrefs.SetString("SelectedCharacter", characterType);
        ChangeScene(baseScene);
    }

    /// <summary>
    /// Sale del juego.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
