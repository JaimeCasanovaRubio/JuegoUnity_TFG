using UnityEngine;

public class EnemieSpawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject [] enemyPrefabs;

    [Header("Spawn Points")]
    [SerializeField] private Transform [] spawnPoints;

    public virtual void SpawnEnemies()
    {
        if(enemyPrefabs == null || enemyPrefabs.Length == 0) return;
        if(spawnPoints == null || spawnPoints.Length == 0) return;

        foreach(Transform point in spawnPoints)
        {
            GameObject enemyPrefab  = enemyPrefabs [Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemyPrefab, point.position, Quaternion.identity);
        }
    }
    
}