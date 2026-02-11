using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawner que instancia enemigos en la escena.
/// Se coloca en cada escena de juego donde haya enemigos.
/// </summary>
public class EnemiesSpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private int enemiesPerWave = 3;

    [Header("Respawn")]
    [Tooltip("Intervalo en segundos entre oleadas. 0 = solo spawn inicial.")]
    [SerializeField] private float spawnInterval = 0f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnWave();
        }

        if (spawnInterval > 0f)
        {
            spawnCoroutine = StartCoroutine(SpawnLoop());
        }
    }

    /// <summary>
    /// Coroutine que spawnea oleadas de enemigos periódicamente.
    /// </summary>
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnWave();
        }
    }

    /// <summary>
    /// Spawnea una oleada de enemigos en los spawn points disponibles.
    /// </summary>
    public void SpawnWave()
    {
        // Limpiar referencias a enemigos ya destruidos
        CleanupDeadEnemies();

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogError("❌ EnemiesSpawner: No hay prefabs de enemigos asignados.");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("❌ EnemiesSpawner: No hay spawn points asignados.");
            return;
        }

        int spawned = 0;

        for (int i = 0; i < enemiesPerWave && activeEnemies.Count < maxEnemies; i++)
        {
            // Elegir un spawn point (cíclico)
            Transform point = spawnPoints[i % spawnPoints.Length];

            // Elegir un prefab aleatorio
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            spawned++;
        }

        Debug.Log($"👹 Oleada spawneada: {spawned} enemigos. Total activos: {activeEnemies.Count}");
    }

    /// <summary>
    /// Elimina de la lista los enemigos que ya han sido destruidos.
    /// </summary>
    private void CleanupDeadEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    /// <summary>
    /// Detiene el spawneo periódico de enemigos.
    /// </summary>
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
            Debug.Log("🛑 EnemiesSpawner: Spawneo detenido.");
        }
    }

    /// <summary>
    /// Devuelve el número de enemigos activos actualmente.
    /// </summary>
    public int GetActiveEnemyCount()
    {
        CleanupDeadEnemies();
        return activeEnemies.Count;
    }

    /// <summary>
    /// Dibuja gizmos en el editor para visualizar los spawn points.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (spawnPoints == null) return;

        Gizmos.color = Color.red;
        foreach (Transform point in spawnPoints)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.5f);
                Gizmos.DrawIcon(point.position, "enemy_spawn", true);
            }
        }
    }
}
