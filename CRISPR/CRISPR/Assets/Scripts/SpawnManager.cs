using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Spawn range
    public Vector2 spawnMin = new Vector2(-10f, -5f);
    public Vector2 spawnMax = new Vector2(10f, 5f);

    // Prefab list
    public List<GameObject> enemyPrefabs;

    // Spawn rate
    public float spawnInterval = 1.5f;

    // Max enemies (changeable)
    public int maxEnemies = 10;

    public Transform enemyParent;

    // Enemy2Small only
    public int enemy2SmallIndex = 1;
    public int minEnemy2SmallCount = 1;   // Minimum Enemy2Small

    List<GameObject> spawned = new List<GameObject>();

    List<GameObject> spawnedEnemy2Small = new List<GameObject>();

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0) return;

        if (enemyParent == null)
        {
            enemyParent = transform;
        }

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            spawned.RemoveAll(x => x == null);
            spawnedEnemy2Small.RemoveAll(x => x == null);

            if (spawned.Count >= maxEnemies) continue;

            if (spawnedEnemy2Small.Count < minEnemy2SmallCount)
            {
                SpawnEnemy2Small();
            }
            else
            {
                SpawnRandom();
            }
        }
    }

    void SpawnRandom()
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        SpawnFromPrefab(prefab);
    }

    void SpawnEnemy2Small()
    {
        if (enemy2SmallIndex < 0 || enemy2SmallIndex >= enemyPrefabs.Count) return;

        GameObject prefab = enemyPrefabs[enemy2SmallIndex];
        SpawnFromPrefab(prefab);
    }

    void SpawnFromPrefab(GameObject prefab)
    {
        if (prefab == null) return;

        // Random spawn pos
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        GameObject newEnemy = Instantiate(prefab, spawnPos, Quaternion.identity, enemyParent);
        spawned.Add(newEnemy);

        // Track Enemy2Small quantity
        if (enemyPrefabs[enemy2SmallIndex] == prefab)
        {
            spawnedEnemy2Small.Add(newEnemy);
        }
    }

    public void NotifyEnemyDestroyed(GameObject enemy)
    {
        if (enemy == null) return;

        spawned.Remove(enemy);
        spawnedEnemy2Small.Remove(enemy);
    }
}