using UnityEngine;
using UnityEngine.SceneManagement;

// Controls the player's spaceship movement, shooting, and collision handling
public class PlayerController : MonoBehaviour
{
    // Speed of spaceship movement
    public float moveSpeed = 5f;
    // Prefab for the laser projectile
    public GameObject laserPrefab;
    // Transform where lasers are spawned
    public Transform firePoint;
    // Time between laser shots
    public float fireRate = 0.5f;
    // Timer to track when the next shot can be fired
    private float fireTimer = 0f;
    // Boundaries for clamping spaceship movement within the camera view
    private float minX, maxX, minY, maxY;

    // Initialize player settings on start
    void Start()
    {
        // Play background music when the player spawns
        AudioManager.Instance.PlayMusic();
        // Calculate camera boundaries for clamping movement
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // Adjust boundaries based on spaceship sprite size
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        minX = bottomLeft.x + halfWidth;
        maxX = topRight.x - halfWidth;
        minY = bottomLeft.y + halfHeight;
        maxY = topRight.y - halfHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStateManager.IsPaused)
        {
            // Get input for horizontal and vertical movement
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            // Normalize movement to prevent faster diagonal movement
            Vector3 movement = new Vector3(moveX, moveY, 0f).normalized;
            // Move the spaceship
            transform.Translate(movement * moveSpeed * Time.deltaTime);

            // Clamp position to keep spaceship within camera bounds
            Vector3 clampedPos = transform.position;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
            clampedPos.y = Mathf.Clamp(clampedPos.y, minY, maxY);
            transform.position = clampedPos;

            // Handle automatic shooting
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
            }
        }
    }

    // Instantiate a laser at the fire point
    void Shoot()
    {
        if (!GameStateManager.IsPaused)
        {
            Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.laserShoot);
        }
    }

    // Handle collisions with asteroids and stars
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!GameStateManager.IsPaused)
        {
            if (other.CompareTag("Asteroid"))
            {
                // End game on asteroid collision
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlaySFX(AudioManager.Instance.gameOver);
                SceneManager.LoadScene("EndGame");
            }
            else if (other.CompareTag("Star"))
            {
                // Add points and destroy star on collection
                ScoreManager.AddScore(2);
                AudioManager.Instance.PlaySFX(AudioManager.Instance.starCollect);
                Destroy(other.gameObject);
            }
        }
    }
}