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

    [Header("Bezier Control Points")]
    [SerializeField] private Transform cubicControlPoint1;
    [SerializeField] private Transform cubicControlPoint2;
    [SerializeField] private Transform quadraticControlPoint;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float delayBetweenEnemies = 1f;

    private void Start()
    {
        if (AreReferencesAssigned())
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }
        else
        {
            Debug.LogWarning("Spawner setup is incomplete. Please assign all required references.");
        }
    }

    private bool AreReferencesAssigned()
    {
        return enemy1Prefab && enemy2Prefab && cubicSpawnPoint && quadraticSpawnPoint &&
               endPoint && cubicControlPoint1 && cubicControlPoint2 && quadraticControlPoint;
    }

    private void SpawnEnemy()
    {
        Debug.Log("Spawning enemies...");

        // Spawn first enemy with cubic Bezier path
        SpawnAtPoint(GetRandomEnemy(), cubicSpawnPoint, true);

        // Spawn second enemy after delay with quadratic Bezier path
        StartCoroutine(SpawnSecondEnemy());
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

            if (useCubic)
            {
                enemyBehavior.cubicControlPoint1 = cubicControlPoint1;
                enemyBehavior.cubicControlPoint2 = cubicControlPoint2;
            }
            else
            {
                enemyBehavior.quadraticControlPoint = quadraticControlPoint;
            }
        }
        else
        {
            Debug.LogWarning("Spawned enemy lacks an EnemyBehavior component!");
        }
    }

    private IEnumerator SpawnSecondEnemy()
    {
        yield return new WaitForSeconds(delayBetweenEnemies);
        SpawnAtPoint(GetRandomEnemy(), quadraticSpawnPoint, false);
    }
}
