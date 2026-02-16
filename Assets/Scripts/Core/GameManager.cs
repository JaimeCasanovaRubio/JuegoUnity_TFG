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
    public string settingsScene = "SettingsMenu";
    public string baseScene = "Base";
    public string gameScene = "OniricForest";
    public string characterSelectionScene = "CharacterSelection";

    

    [Header("UI Prefabs")]
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private GameObject controlsPrefab;

    // Referencias a las instancias creadas
    public GameObject SettingsCanvas { get; private set; }
    public GameObject ControlsCanvas { get; private set; }

    public string currentScene;
    public string lastScene;
    public string lastPlayScene;

    private bool isPaused = false;
    // Escenas donde se puede pausar (escenas de juego)
    private string[] playableScenes;

    private void Awake()
    {
        Debug.Log($"[GameManager] Awake on {gameObject.name} (Scene: {SceneManager.GetActiveScene().name})");

        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            
            // IMPORTANTE: DontDestroyOnLoad solo funciona en objetos raíz.
            // Si el objeto tiene un padre, lo desvinculamos primero.
            if (transform.parent != null)
            {
                Debug.Log($"[GameManager] Desvinculando de {transform.parent.name} para asegurar persistencia.");
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);
            Debug.Log($"[GameManager] ✅ Instancia asignada y persistente (DontDestroyOnLoad). ID: {gameObject.GetInstanceID()}");
            
            // Instanciar UI persistente desde Prefabs
            InitializeUI();

            // Definir las escenas donde se puede pausar
            playableScenes = new string[] { gameScene, baseScene, "OniricForest" };
            
            // Asegurar que los singletons de input existan
            _ = KeyBindings.Instance;
            _ = InputHandler.Instance;
        }
        else
        {
            Debug.Log($"[GameManager] ❌ Duplicado detectado en {gameObject.name}. Destruyendo instancia...");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Instancia los prefabs de UI como hijos del GameManager para que sean persistentes.
    /// </summary>
    private void InitializeUI()
    {
        if (settingsPrefab != null && SettingsCanvas == null)
        {
            SettingsCanvas = Instantiate(settingsPrefab, transform);
            SettingsCanvas.name = "SettingsCanvas (Persistent)";
            SettingsCanvas.SetActive(false);
        }

        if (controlsPrefab != null && ControlsCanvas == null)
        {
            ControlsCanvas = Instantiate(controlsPrefab, transform);
            ControlsCanvas.name = "ControlsCanvas (Persistent)";
            ControlsCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputHandler.Instance != null && InputHandler.Instance.IsPausePressed())
        {
            if (IsInPlayableScene())
            {
                ChangeView();
            }
        }
    }

    /// <summary>
    /// Cambia entre el estado de juego y el menú de pausa/ajustes.
    /// </summary>
    public void ChangeView()
    {
        isPaused = !isPaused;

        // Congelar/Reanudar tiempo
        Time.timeScale = isPaused ? 0f : 1f;

        // Mostrar/Ocultar el canvas de ajustes
        if (SettingsCanvas != null)
        {
            SettingsCanvas.SetActive(isPaused);
        }

        Debug.Log(isPaused ? "⏸️ Juego Pausado" : "▶️ Juego Reanudado");
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
