using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;


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
    
    [Header("Escenas")]
    [SerializeField] public string menuPrincipal = "MenuScene";

    [Header("EscenasJugables")]
    [SerializeField] public string baseScene = "Base";
    [SerializeField] public string mapaPrueba = "MapaPrueba";
    [SerializeField] public List<string> primerMapa = new List<string>();

    public GameObject SettingsCanvas { get; private set; }
    public GameObject ControlsCanvas { get; private set; }
    public GameObject SelectCharCanvas { get; private set; }

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
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != menuPrincipal)
        {
            if (SettingsCanvas != null && ControlsCanvas != null)
            {
                if(!ControlsCanvas.activeSelf)
                {
                    isPaused = !isPaused;
                    Time.timeScale = isPaused ? 0f : 1f;
                    SettingsCanvas.SetActive(isPaused);
                }
            }
        }
    }

    public void ChangeScene(string sceneName, int index = 6)
    {   
        SceneGestor.ChangeScene(sceneName, index);
    }

    public void StartGameWithCharacter(string characterType)
    {
        PlayerPrefs.SetString("SelectedCharacter", characterType);
        ChangeScene(baseScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach(MapScene mapS in SceneGestor.mpVisited)
        {
            if(mapS.sceneName == scene.name)
            {
                Teleport[] tps = Object.FindObjectsByType<Teleport>(FindObjectsSortMode.None);   
                foreach(Teleport tp in tps)
                {
                    if(mapS.sceneIndex0 != "random1" && tp.index == 0){
                        tp.sceneName = mapS.sceneIndex0;
                        tp.goBack = mapS.isVisited0;
                    }
                    if(mapS.sceneIndex1 != "random1" && tp.index == 1){
                        tp.sceneName = mapS.sceneIndex1;
                        tp.goBack = mapS.isVisited1;
                    }
                    if(mapS.sceneIndex2 != "random1" && tp.index == 2){
                        tp.sceneName = mapS.sceneIndex2;
                        tp.goBack = mapS.isVisited2;
                    }
                    if(mapS.sceneIndex3 != "random1" && tp.index == 3){
                        tp.sceneName = mapS.sceneIndex3;
                        tp.goBack = mapS.isVisited3;
                    }
                }
            }
        }
        if (scene.name == mapaPrueba)
        {
            SpawnHUD();
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
            EnemieSpawner spawner = FindObjectOfType<EnemieSpawner>();
            if(spawner != null) spawner.SpawnEnemies();
            
            if(SceneGestor.doorIndex == 0 || SceneGestor.doorIndex == 1 || SceneGestor.doorIndex == 2 || SceneGestor.doorIndex == 3)
            {
                SceneGestor.SetLastScene(SceneGestor.doorIndex);
                SpawnPlayerAtTP();
            }
            else
            {
                Player player = FindObjectOfType<Player>();
                if (player != null) player.transform.position = Vector3.zero;
            }
        }
        else if (scene.name == baseScene)
        {
            hudSpawned = false;
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
            if(SceneGestor.doorIndex == 0 || SceneGestor.doorIndex == 1 || SceneGestor.doorIndex == 2 || SceneGestor.doorIndex == 3)
            {
                SceneGestor.SetLastScene(SceneGestor.doorIndex);
                SpawnPlayerAtTP();
            }
        }
        else if (primerMapa.Contains(scene.name)
            || SceneGestor.mpVisited.Any(m => m.sceneName == scene.name)
            || (SceneGestor.SavedMap != null && SceneGestor.SavedMap.sceneName == scene.name))
        {
            SpawnHUD();
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
            EnemieSpawner spawner = FindObjectOfType<EnemieSpawner>();
            if(spawner != null)
            {
                if (SceneGestor.SavedMap != null)
                {
                    if (!SceneGestor.SavedMap.isVisited)
                    {
                        spawner.SpawnEnemies();
                        SceneGestor.SavedMap.isVisited = true;
                    }
                }
                else
                {
                    spawner.SpawnEnemies();
                }
            }
            if(SceneGestor.doorIndex == 0|| SceneGestor.doorIndex == 1 
            || SceneGestor.doorIndex == 2 || SceneGestor.doorIndex == 3)
            {
                SceneGestor.SetLastScene(SceneGestor.doorIndex);
                SpawnPlayerAtTP();
            }
            else
            {
                Player player = FindObjectOfType<Player>();
                if (player != null) player.transform.position = Vector3.zero;
            }
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
    private void SpawnPlayerAtTP()
    {
        Player player = FindObjectOfType<Player>();
        if (player == null) return;
        Teleport[] tps = FindObjectsByType<Teleport>(FindObjectsSortMode.None);
        foreach (Teleport teleport in tps)
        {
            if (teleport.index == SceneGestor.indexToTP)
            {   
                Vector2 offset = SceneGestor.indexToTP switch
                {
                    0 => new Vector2(1.5f, 0),   // TP izquierdo → empujar a la derecha
                    2 => new Vector2(-1.5f, 0),   // TP derecho → empujar a la izquierda
                    1 => new Vector2(0, -1.5f),   // TP arriba → empujar hacia abajo
                    3 => new Vector2(0, 1.5f),    // TP abajo → empujar hacia arriba
                    _ => Vector2.zero
                };
                player.transform.position = (Vector2)teleport.transform.position + offset;
                return;
            }
        }
        // Si no se encontró el TP, mover al centro
        player.transform.position = Vector3.zero;
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
