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
    public string gameScene = "OniricForest";
    public string characterSelectionScene = "CharacterSelection";

    private string lastScene;
    private string lastPlayScene;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
        ChangeScene(gameScene);
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
