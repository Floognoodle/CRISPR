using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Vector2 spawnMin = new Vector2(-10f, -5f);
    public Vector2 spawnMax = new Vector2(10f, 5f);

    public List<GameObject> enemyPrefabs;
    public float spawnInterval = 1.5f;
    public int maxEnemies = 10;
    public Transform enemyParent;

    List<GameObject> spawned = new List<GameObject>();

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs set.");
            return;
        }

        if (enemyParent == null) enemyParent = transform;

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            spawned.RemoveAll(x => x == null);

            if (spawned.Count < maxEnemies)
            {
                SpawnOne();
            }
        }
    }

    void SpawnOne()
    {
        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        if (prefab == null) return;

        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        GameObject go = Instantiate(prefab, pos, Quaternion.identity, enemyParent);
        spawned.Add(go);
    }

    // call this when an enemy dies to remove it fast
    public void NotifyEnemyDestroyed(GameObject enemy)
    {
        if (enemy == null) return;
        spawned.Remove(enemy);
    }
}