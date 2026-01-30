using UnityEngine;

/// <summary>
/// Spawner que instancia al jugador correcto según la selección.
/// Se coloca en cada escena de juego.
/// </summary>
public class PlayerSpawner : MonoBehaviour
{
    [Header("Character Prefabs")]
    [SerializeField] private GameObject hurtadillaPrefab;
    // Añadir más prefabs según se añadan personajes

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool spawnOnStart = true;

    private Player spawnedPlayer;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnPlayer();
        }
    }

    /// <summary>
    /// Instancia al jugador basándose en la selección guardada.
    /// </summary>
    public void SpawnPlayer()
    {
        string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "Hurtadilla");
        GameObject prefabToSpawn = GetPrefabByName(selectedCharacter);

        if (prefabToSpawn == null)
        {
            Debug.LogError($"❌ No se encontró el prefab para: {selectedCharacter}");
            return;
        }

        Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        GameObject playerObj = Instantiate(prefabToSpawn, position, Quaternion.identity);
        playerObj.name = selectedCharacter;

        spawnedPlayer = playerObj.GetComponent<Player>();

        // Registrar el jugador en el GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayer(spawnedPlayer);
        }

       

        Debug.Log($"✅ Jugador '{selectedCharacter}' spawneado en {position}");
    }

    /// <summary>
    /// Obtiene el prefab correspondiente al nombre del personaje.
    /// </summary>
    private GameObject GetPrefabByName(string characterName)
    {
        switch (characterName)
        {
            case "Hurtadilla":
                return hurtadillaPrefab;
            // Añadir más casos según se añadan personajes
            default:
                Debug.LogWarning($"⚠️ Personaje desconocido: {characterName}, usando Hurtadilla");
                return hurtadillaPrefab;
        }
    }

    public Player GetSpawnedPlayer()
    {
        return spawnedPlayer;
    }
}
