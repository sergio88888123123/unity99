using UnityEngine;

/// <summary>
/// Controla la aparición de enemigos y coleccionables.
/// Escala la dificultad con el nivel actual.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("Spawners")]
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    public GameObject collectiblePrefab;

    [Header("Tiempos")]
    public float baseEnemySpawnTime = 5f;
    public float baseCollectibleSpawnTime = 3f;

    private float enemyTimer;
    private float collectibleTimer;

    private void Start()
    {
        enemyTimer = baseEnemySpawnTime;
        collectibleTimer = baseCollectibleSpawnTime;
    }

    private void Update()
    {
        enemyTimer -= Time.deltaTime;
        collectibleTimer -= Time.deltaTime;

        if (enemyTimer <= 0f)
        {
            SpawnEnemy();
            enemyTimer = GetEnemySpawnTime();
        }

        if (collectibleTimer <= 0f)
        {
            SpawnCollectible();
            collectibleTimer = GetCollectibleSpawnTime();
        }
    }

    private float GetEnemySpawnTime()
    {
        int level = GameManager.Instance != null ? GameManager.Instance.currentLevelIndex : 1;
        float difficultyFactor = 1f;

        if (GameManager.Instance != null)
        {
            switch (GameManager.Instance.difficulty)
            {
                case Difficulty.Easy:
                    difficultyFactor = 1.2f;
                    break;
                case Difficulty.Normal:
                    difficultyFactor = 1f;
                    break;
                case Difficulty.Hard:
                    difficultyFactor = 0.7f;
                    break;
            }
        }

        return Mathf.Clamp(baseEnemySpawnTime / (level * 0.7f) * difficultyFactor, 1.0f, 10f);
    }

    private float GetCollectibleSpawnTime()
    {
        int level = GameManager.Instance != null ? GameManager.Instance.currentLevelIndex : 1;
        return Mathf.Clamp(baseCollectibleSpawnTime * (1f + level * 0.2f), 1.5f, 8f);
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return null;

        int index = Random.Range(0, spawnPoints.Length);
        return spawnPoints[index];
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        Transform spawn = GetRandomSpawnPoint();
        if (spawn == null) return;

        Instantiate(enemyPrefab, spawn.position, spawn.rotation);
    }

    private void SpawnCollectible()
    {
        if (collectiblePrefab == null) return;
        Transform spawn = GetRandomSpawnPoint();
        if (spawn == null) return;

        Instantiate(collectiblePrefab, spawn.position, spawn.rotation);
    }
}
