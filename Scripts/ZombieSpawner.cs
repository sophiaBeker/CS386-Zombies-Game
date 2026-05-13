using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns zombies in waves. Each wave adds more zombies and increases speed,
/// creating emergent difficulty progression toward the 10-kill win condition.
/// Spawn points are set as child transforms of this GameObject.
/// </summary>
public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie")]
    public GameObject zombiePrefab;

    [Header("Wave Settings")]
    public int   zombiesPerWave        = 3;
    public int   zombiesPerWaveIncrease = 1;   // +N zombies each wave
    public float timeBetweenWaves      = 5f;
    public float spawnInterval         = 0.5f; // time between individual spawns in a wave
    public float speedIncreasePerWave  = 0.15f;

    [Header("Spawn Area Bounds")]
    public Vector2 minBounds = new Vector2(-8f, -4f);
    public Vector2 maxBounds = new Vector2(8f, 4f);

    private Transform[] _spawnPoints;
    private int         _currentWave = 0;
    private float       _baseZombieSpeed;

    // ---------------------------------------------------------------
    void Start()
    {
        // Collect spawn points from children
        _spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            _spawnPoints[i] = transform.GetChild(i);

        if (_spawnPoints.Length == 0)
            Debug.LogWarning("ZombieSpawner: No child spawn points found!");

        // Cache the prefab's base speed
        ZombieAI prefabAI = zombiePrefab.GetComponent<ZombieAI>();
        _baseZombieSpeed = prefabAI != null ? prefabAI.moveSpeed : 2f;

        Debug.Log($"ZombieSpawner found {transform.childCount} spawn points");
        Debug.Log($"Zombie prefab assigned: {zombiePrefab != null}");
        StartCoroutine(SpawnLoop());
    }

    // ---------------------------------------------------------------
    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            _currentWave++;
            int count = zombiesPerWave + (_currentWave - 1) * zombiesPerWaveIncrease;
            yield return StartCoroutine(SpawnWave(count));
        }
    }

    IEnumerator SpawnWave(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnZombie()
    {
        Debug.Log("Attempting to spawn zombie...");
        if (_spawnPoints.Length == 0 || zombiePrefab == null)
        {
            Debug.LogWarning("Cannot spawn - missing spawn points or prefab!");
            return;
        }

        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];

        // Clamp spawn position within bounds
        Vector3 clampedPos = new Vector3(
            Mathf.Clamp(spawnPoint.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(spawnPoint.position.y, minBounds.y, maxBounds.y),
            0f
        );

        GameObject go = Instantiate(zombiePrefab, clampedPos, Quaternion.identity);

        // Apply per-wave speed increase
        ZombieAI ai = go.GetComponent<ZombieAI>();
        if (ai != null)
            ai.moveSpeed = _baseZombieSpeed + (_currentWave - 1) * speedIncreasePerWave;
    }
}
