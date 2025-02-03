using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy1Prefab;  
    public GameObject enemy2Prefab; 
    public Transform cubicSpawnPoint;
    public Transform quadraticSpawnPoint;
    public Transform endPoint;
    public float spawnInterval = 3f;
    public float delayBetweenEnemies = 1f; // Delay in seconds

    private void Start()
    {
        // Ensure the spawner is correctly set up
        if (enemy1Prefab != null && enemy2Prefab != null && cubicSpawnPoint != null && quadraticSpawnPoint != null && endPoint != null)
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
        }
        else
        {
            Debug.LogWarning("Some required fields are not assigned in the Inspector!");
        }
    }

    private void SpawnEnemy()
    {
        // Log to ensure this method is being triggered
        Debug.Log("Spawning enemies");

        // Randomly select an enemy prefab to spawn at each spawn point
        GameObject selectedEnemy1 = Random.Range(0, 2) == 0 ? enemy1Prefab : enemy2Prefab;
        GameObject selectedEnemy2 = Random.Range(0, 2) == 0 ? enemy1Prefab : enemy2Prefab;

        // Instantiate the first enemy at cubic spawn point
        GameObject enemy1 = Instantiate(selectedEnemy1, cubicSpawnPoint.position, Quaternion.identity);
        enemy1.GetComponent<EnemyBehavior>().startPoint = cubicSpawnPoint;
        enemy1.GetComponent<EnemyBehavior>().endPoint = endPoint;
        enemy1.GetComponent<EnemyBehavior>().useCubicLerp = true;

        // Start coroutine to delay the spawning of the second enemy
        StartCoroutine(SpawnSecondEnemy(selectedEnemy2));
    }

    // Coroutine to spawn second enemy after delay
    private IEnumerator SpawnSecondEnemy(GameObject selectedEnemy2)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delayBetweenEnemies);

        // Instantiate the second enemy at quadratic spawn point
        GameObject enemy2 = Instantiate(selectedEnemy2, quadraticSpawnPoint.position, Quaternion.identity);
        enemy2.GetComponent<EnemyBehavior>().startPoint = quadraticSpawnPoint;
        enemy2.GetComponent<EnemyBehavior>().endPoint = endPoint;
        enemy2.GetComponent<EnemyBehavior>().useCubicLerp = false;
    }
}
