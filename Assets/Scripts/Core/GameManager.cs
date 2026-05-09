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
    [SerializeField] private GameObject gameSelectorPrefab;
    [SerializeField] private GameObject magicBookPrefab;

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
    public GameObject GameSelectorCanvas { get; private set; }
    public GameObject MagicBookCanvas { get; private set; }

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
        if(gameSelectorPrefab != null && GameSelectorCanvas == null)
        {
            GameSelectorCanvas = Instantiate (gameSelectorPrefab, transform);
            GameSelectorCanvas.name = "GameSelectorCanvas";
            GameSelectorCanvas.SetActive(false);
        }
        if(magicBookPrefab != null && MagicBookCanvas == null)
        {
            MagicBookCanvas = Instantiate (magicBookPrefab, transform);
            MagicBookCanvas.name = "MagicBookCanvas";
            MagicBookCanvas.SetActive(false);
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
    public void ChangeScene(string sceneName, string roomId, int index = 6)
    {   
        SceneGestor.ChangeScene(sceneName, roomId, index);
    }

    public void StartGameWithCharacter(string characterType, int gameNumber)
    {
        PlayerPrefs.SetString("SelectedCharacter"+gameNumber, characterType); 
        PlayerPrefs.SetInt("Armazon1_G"+gameNumber,1);
        PlayerPrefs.SetInt("Armazon2_G"+gameNumber,0);
        PlayerPrefs.SetInt("Armazon3_G"+gameNumber,0);
        PlayerPrefs.SetInt("Armazon4_G"+gameNumber,0);
        PlayerPrefs.SetInt("Afinidad1_G"+gameNumber,1);
        PlayerPrefs.SetInt("Afinidad2_G"+gameNumber,0);
        PlayerPrefs.SetInt("Afinidad3_G"+gameNumber,0);
        PlayerPrefs.SetInt("Afinidad4_G"+gameNumber,0);

        PlayerPrefs.SetString("SelectedCharacter", characterType);
        
        ChangeScene(baseScene);
           
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Escena cargada: " + scene.name);
        foreach(MapScene mapS in SceneGestor.mpVisited)
        {
            if(mapS.roomId == SceneGestor.SavedMap.roomId)
            {
                Teleport[] tps = Object.FindObjectsByType<Teleport>(FindObjectsSortMode.None);   
                foreach(Teleport tp in tps)
                {
                    if(mapS.sceneIndex0 != "random1" && tp.index == 0){
                        tp.sceneName = mapS.sceneIndex0;
                        tp.targetRoomId = mapS.roomId0;
                        tp.goBack = mapS.isVisited0;
                    }
                    if(mapS.sceneIndex1 != "random1" && tp.index == 1){
                        tp.sceneName = mapS.sceneIndex1;
                        tp.targetRoomId = mapS.roomId1;
                        tp.goBack = mapS.isVisited1;
                    }
                    if(mapS.sceneIndex2 != "random1" && tp.index == 2){
                        tp.sceneName = mapS.sceneIndex2;
                        tp.targetRoomId = mapS.roomId2;
                        tp.goBack = mapS.isVisited2;
                    }
                    if(mapS.sceneIndex3 != "random1" && tp.index == 3){
                        tp.sceneName = mapS.sceneIndex3;
                        tp.targetRoomId = mapS.roomId3;
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
            Player player = FindObjectOfType<Player>();
            if (player != null) player.transform.position = Vector3.zero;
            
        }
        else if (scene.name == baseScene)
        {
            hudSpawned = false;
            if (FindObjectOfType<Player>() == null)
            {
                SpawnPlayer();
            }
        }
        else if (primerMapa.Contains(scene.name)
            || SceneGestor.mpVisited.Any(m => m.roomId == SceneGestor.SavedMap.roomId))
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
    public void CreateOnPlayerPrefs(int game)
    {

        if(game == 1){
            
            Player player = null;
            string character = PlayerPrefs.GetString("SelectedCharacter1");
            foreach(GameObject prefab in characterPrefabs)
            {
                if(prefab.name == character)
                {
                     GameObject newPlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                     player = newPlayer.GetComponent<Player>();
                    break;
                }
            }
            if (player == null) player = FindObjectOfType<Player>();

            player.Armazon1 = PlayerPrefs.GetInt("Armazon1_G1") == 1 ? true : false;
            player.Armazon2 = PlayerPrefs.GetInt("Armazon2_G1") == 1 ? true : false;
            player.Armazon3 = PlayerPrefs.GetInt("Armazon3_G1") == 1 ? true : false;
            player.Afinidad1 = PlayerPrefs.GetInt("Afinidad1_G1") == 1 ? true : false;
            player.Afinidad2 = PlayerPrefs.GetInt("Afinidad2_G1") == 1 ? true : false;
            player.Afinidad3 = PlayerPrefs.GetInt("Afinidad3_G1") == 1 ? true : false;
            player.Afinidad4 = PlayerPrefs.GetInt("Afinidad4_G1") == 1 ? true : false;
        }else if(game == 2){
            Player player = null;
            string character = PlayerPrefs.GetString("SelectedCharacter2");
            foreach(GameObject prefab in characterPrefabs)
            {
                if(prefab.name == character)
                {
                     GameObject newPlayer = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                     player = newPlayer.GetComponent<Player>();
                    break;
                }
            }
            if (player == null) player = FindObjectOfType<Player>();

            player.Armazon1 = PlayerPrefs.GetInt("Armazon1_G2") == 1 ? true : false;
            player.Armazon2 = PlayerPrefs.GetInt("Armazon2_G2") == 1 ? true : false;
            player.Armazon3 = PlayerPrefs.GetInt("Armazon3_G2") == 1 ? true : false;
            player.Afinidad1 = PlayerPrefs.GetInt("Afinidad1_G2") == 1 ? true : false;
            player.Afinidad2 = PlayerPrefs.GetInt("Afinidad2_G2") == 1 ? true : false;
            player.Afinidad3 = PlayerPrefs.GetInt("Afinidad3_G2") == 1 ? true : false;
            player.Afinidad4 = PlayerPrefs.GetInt("Afinidad4_G2") == 1 ? true : false;
            
        }
        GameSelectorCanvas.SetActive(false);
        ChangeScene(baseScene);
    }
    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
