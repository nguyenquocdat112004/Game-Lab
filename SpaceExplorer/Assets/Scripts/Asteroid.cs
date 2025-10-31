using UnityEngine;

// Controls the behavior of individual asteroid objects in the game
public class Asteroid : MonoBehaviour
{
    // Speed at which the asteroid moves downward
    public float speed = 2f;
    // Rigidbody2D component for physics-based movement
    private Rigidbody2D rb;
    // Array of possible asteroid sprites for visual variety
    public Sprite[] asteroidSprites;

    // Initialize asteroid on creation
    void Start()
    {
        // Get the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Set initial downward velocity
        rb.linearVelocity = Vector2.down * speed;

        // Randomly assign a sprite from the asteroidSprites array
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (asteroidSprites.Length > 0 && sr != null)
        {
            sr.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        }
    }

    // Set a new speed for the asteroid and update its velocity
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.down * speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Destroy the asteroid if it moves below the camera's view
        if (transform.position.y < -Camera.main.orthographicSize - 1f)
        {
            Destroy(gameObject);
        }
    }

    // Destroy the asteroid when it becomes invisible (e.g., leaves the screen)
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}