using UnityEngine;
using UnityEngine.UI;

// Manages game difficulty by changing backgrounds and asteroid spawn rates
public class GameDifficultyManager : MonoBehaviour
{
    // Array of background sprites for different levels
    public Sprite[] backgroundSprites;
    // Image component for the background
    public Image backgroundImage;
    // Duration of each difficulty level (in seconds)
    public float levelDuration = 60f;

    // Current difficulty level
    private int currentLevel = 0;
    // Timer to track level duration
    private float timer = 0f;

    // Initialize the first level
    void Awake()
    {
        SetLevel(0);
    }

    // Update the timer and change levels when needed
    void Update()
    {
        if (!GameStateManager.IsPaused)
        {
            timer += Time.deltaTime;

            // Change to the next level if the timer exceeds the duration
            if (timer >= levelDuration && currentLevel < backgroundSprites.Length - 1)
            {
                currentLevel++;
                SetLevel(currentLevel);
                timer = 0f;
            }
        }
    }

    // Set the background and asteroid spawn rate for a specific level
    void SetLevel(int level)
    {
        if (backgroundImage.sprite == backgroundSprites[level])
            return;

        // Update the background sprite
        backgroundImage.sprite = backgroundSprites[level];
        backgroundImage.color = Color.white;

        // Adjust asteroid spawn rate
        FindAnyObjectByType<AsteroidSpawner>().IncreaseSpawnRate(level);
    }
}