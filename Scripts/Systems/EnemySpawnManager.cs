using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Defines the properties of a single wave of enemies.
/// </summary>
[System.Serializable]
public class Wave
{
    public string name;
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval;
    public Transform[] spawnPoints;
}

/// <summary>
/// Manages the spawning of enemy waves throughout the level.
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    [Header("Wave Configuration")]
    [Tooltip("An array of all waves to be spawned in this level.")]
    public Wave[] waves;

    private int currentWaveIndex = 0;
    private int enemiesRemainingInWave;

    private void Start()
    {
        // Start the first wave automatically
        if (waves.Length > 0)
        {
            StartCoroutine(SpawnWaveCoroutine(waves[currentWaveIndex]));
        }
    }

    /// <summary>
    /// Coroutine to handle the spawning logic for a single wave.
    /// </summary>
    private IEnumerator SpawnWaveCoroutine(Wave wave)
    {
        Debug.Log("Starting Wave: " + wave.name);
        enemiesRemainingInWave = wave.count;
        GameManager.Instance.ResetCombo(); // Reset combo at the start of a new wave

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemyPrefab, wave.spawnPoints);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    /// <summary>
    /// Spawns a single enemy at a random available spawn point.
    /// </summary>
    private void SpawnEnemy(GameObject enemyPrefab, Transform[] spawnPoints)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned for the current wave!");
            return;
        }

        // Select a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the enemy
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyAI enemyAI = enemyGO.GetComponent<EnemyAI>();

        // Activate the enemy
        if (enemyAI != null)
        {
            // We can subscribe to an OnDeath event here if EnemyAI had one.
            // For now, GameManager will notify us.
            enemyAI.Activate();
        }
    }

    /// <summary>
    /// This method should be called by an enemy when it is defeated.
    /// </summary>
    public void OnEnemyDefeated()
    {
        enemiesRemainingInWave--;

        if (enemiesRemainingInWave <= 0)
        {
            Debug.Log("Wave " + waves[currentWaveIndex].name + " cleared!");
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                // Start the next wave after a short delay
                StartCoroutine(SpawnWaveCoroutine(waves[currentWaveIndex]));
            }
            else
            {
                Debug.Log("All waves cleared! Level complete.");
                GameManager.Instance.LevelComplete();
            }
        }
    }
}