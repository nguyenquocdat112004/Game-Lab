using UnityEngine;

// Spawns stars at random positions within the camera view
public class StarSpawner : MonoBehaviour
{
    // Prefab for the star GameObject
    public GameObject starPrefab;
    // Rate at which stars spawn (seconds between spawns)
    public float spawnRate = 5f;
    // Range for random X position of spawn
    public float xRange = 8f;
    // Range for random Y position of spawn
    public float yRange = 8f;

    // Start spawning stars when the spawner is initialized
    void Start()
    {
        InvokeRepeating("SpawnStar", 1f, spawnRate);
    }

    // Spawn a single star at a random position
    void SpawnStar()
    {
        if (!GameStateManager.IsPaused)
        {
            // Calculate random spawn position
            Vector2 spawnPos = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
            Instantiate(starPrefab, spawnPos, Quaternion.identity);
        }
    }

    // Destroy the star if it leaves the screen
    void Update()
    {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
        {
            Destroy(gameObject);
        }
    }
}