using UnityEngine;

// Controls the behavior of laser projectiles
public class Laser : MonoBehaviour
{
    // Speed at which the laser moves upward
    public float speed = 10f;

    // Move the laser upward each frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // Handle collisions with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            // Destroy the asteroid and laser, add score, and play sound
            AudioManager.Instance.PlaySFX(AudioManager.Instance.hitAsteroid);
            Destroy(other.gameObject);
            Destroy(gameObject);
            ScoreManager.AddScore(1);
        }
    }

    // Destroy the laser when it leaves the screen
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}