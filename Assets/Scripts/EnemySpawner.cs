using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform cubicSpawnPoint;
    [SerializeField] private Transform quadraticSpawnPoint;
    [SerializeField] private Transform endPoint;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float delayBetweenEnemies = 1f; // Delay before spawning the second enemy

    private void Start()
    {
        // Validate required references before starting the spawner
        if (AreReferencesAssigned())
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }
        else
        {
            Debug.LogWarning("Spawner setup is incomplete. Check if all required references are assigned.");
        }
    }

    private bool AreReferencesAssigned()
    {
        return enemy1Prefab != null && enemy2Prefab != null && cubicSpawnPoint != null
               && quadraticSpawnPoint != null && endPoint != null;
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawning enemies...");

        // Select a random enemy prefab for each spawn point
        GameObject selectedEnemy1 = GetRandomEnemy();
        GameObject selectedEnemy2 = GetRandomEnemy();

        // Spawn first enemy at the cubic spawn point
        SpawnAtPoint(selectedEnemy1, cubicSpawnPoint, true);

        // Spawn the second enemy after a delay
        StartCoroutine(SpawnSecondEnemy(selectedEnemy2));
    }

    private GameObject GetRandomEnemy()
    {
        return Random.Range(0, 2) == 0 ? enemy1Prefab : enemy2Prefab;
    }

    private void SpawnAtPoint(GameObject enemyPrefab, Transform spawnPoint, bool useCubic)
    {
        if (enemyPrefab == null || spawnPoint == null) return;

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBehavior enemyBehavior = enemy.GetComponent<EnemyBehavior>();

        if (enemyBehavior != null)
        {
            enemyBehavior.startPoint = spawnPoint;
            enemyBehavior.endPoint = endPoint;
            enemyBehavior.useCubicLerp = useCubic;
        }
        else
        {
            Debug.LogWarning("Spawned enemy does not have an EnemyBehavior component!");
        }
    }

    private IEnumerator SpawnSecondEnemy(GameObject selectedEnemy)
    {
        yield return new WaitForSeconds(delayBetweenEnemies);
        SpawnAtPoint(selectedEnemy, quadraticSpawnPoint, false);
    }
}
