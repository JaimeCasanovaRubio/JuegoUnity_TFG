using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador principal del juego.
/// Gestiona la navegación entre escenas y la UI persistente.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private GameObject [] characterPrefabs;

    [Header("UI Prefabs")]
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private GameObject controlsPrefab;
    [SerializeField] private GameObject selectCharPrefab;
    [SerializeField] private GameObject hudPrefab;

    private HUDController hudController;
    private bool hudSpawned = false;

    [Header("EscenasJugables")]
    [SerializeField] public string baseScene = "base";
    [SerializeField] public string mapaPrueba = "MapaPrueba";

    [Header("TP")]
    [SerializeField] private GameObject [] tp;

    public GameObject SettingsCanvas { get; private set; }
    public GameObject ControlsCanvas { get; private set; }
    public GameObject SelectCharCanvas { get; private set; }

    public string currentScene;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            if (transform.parent != null)
            {
                transform.SetParent(null);
            }

            DontDestroyOnLoad(gameObject);
            InitializeUI();

            _ = KeyBindings.Instance;
            _ = InputHandler.Instance;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeUI()
    {
        if (settingsPrefab != null && SettingsCanvas == null)
        {
            SettingsCanvas = Instantiate(settingsPrefab, transform);
            SettingsCanvas.name = "SettingsCanvas";
            SettingsCanvas.SetActive(false);
        }

        if (controlsPrefab != null && ControlsCanvas == null)
        {
            ControlsCanvas = Instantiate(controlsPrefab, transform);
            ControlsCanvas.name = "ControlsCanvas";
            ControlsCanvas.SetActive(false);
        }
        if(selectCharPrefab != null && SelectCharCanvas == null)
        {
            SelectCharCanvas = Instantiate (selectCharPrefab, transform);
            SelectCharCanvas.name = "SelectCharCanvas";
            SelectCharCanvas.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputHandler.Instance != null && InputHandler.Instance.IsPausePressed())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (SettingsCanvas != null)
        {
            SettingsCanvas.SetActive(isPaused);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void StartGameWithCharacter(string characterType)
    {
        PlayerPrefs.SetString("SelectedCharacter", characterType);
        ChangeScene(baseScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mapaPrueba)
        {
            SpawnHUD();
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
            EnemieSpawner spawner = FindObjectOfType<EnemieSpawner>();
            if(spawner != null) spawner.SpawnEnemies();
        }
        else if (scene.name == baseScene)
        {
            hudSpawned = false;
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
            EnemieSpawner spawner = FindObjectOfType<EnemieSpawner>();
            if(spawner != null) spawner.SpawnEnemies();
        }
        else
        {
            hudSpawned = false;
        }
    }
    
    private void SpawnHUD()
    {   
        if (hudPrefab != null && !hudSpawned)
        {
            GameObject hudInstance = Instantiate(hudPrefab, transform);
            hudInstance.name = "HUD";
            hudController = hudInstance.GetComponent<HUDController>();
            hudSpawned = true;
        }
    }
    public void SpawnPlayer()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Hurtadilla");
    
        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab.name == selectedCharacter)
            {
                Instantiate(prefab, Vector3.zero, Quaternion.identity);
                return;
            }
        }
    
        Debug.LogWarning($"No se encontró prefab para: {selectedCharacter}");
    }      

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
