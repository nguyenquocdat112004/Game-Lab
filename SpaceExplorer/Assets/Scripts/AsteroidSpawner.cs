using UnityEngine;

// Spawns asteroids at random positions at the top of the screen
public class AsteroidSpawner : MonoBehaviour
{
    // Prefab for the asteroid GameObject
    public GameObject asteroidPrefab;
    // Rate at which asteroids spawn (seconds between spawns)
    public float spawnRate = 2f;
    // Range for random X position of spawn
    public float xRange = 8f;
    // Y position offset above the camera view for spawning
    public float yRange = 4f;
    // Speed at which spawned asteroids fall
    public float asteroidFallSpeed = 2f;

    // Start spawning asteroids when the spawner is initialized
    void Start()
    {
        StartSpawning();
    }

    // Begin or restart the asteroid spawning process
    void StartSpawning()
    {
        // Cancel any existing spawn invocations
        CancelInvoke("SpawnAsteroid");
        // Start spawning asteroids with a delay of 1 second
        InvokeRepeating("SpawnAsteroid", 1f, spawnRate);
    }

    // Spawn a single asteroid at a random position
    void SpawnAsteroid()
    {
        if (!GameStateManager.IsPaused)
        {
            // Calculate random spawn position at the top of the screen
            float x = Random.Range(-xRange, xRange);
            float y = Camera.main.orthographicSize + 1f;
            Vector2 spawnPos = new Vector2(x, y);

            // Instantiate the asteroid
            GameObject asteroidObj = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

            // Set the asteroid's speed
            Asteroid asteroid = asteroidObj.GetComponent<Asteroid>();
            if (asteroid != null)
            {
                asteroid.SetSpeed(asteroidFallSpeed);
            }
        }
    }

    // Adjust spawn rate and asteroid speed based on game level
    public void IncreaseSpawnRate(int level)
    {
        switch (level)
        {
            case 0:
                spawnRate = 2.0f;
                asteroidFallSpeed = 2f;
                break;
            case 1:
                spawnRate = 1.2f;
                asteroidFallSpeed = 3.5f;
                break;
            case 2:
                spawnRate = 0.6f;
                asteroidFallSpeed = 5f;
                break;
        }

        // Restart spawning with updated parameters
        StartSpawning();
    }
}