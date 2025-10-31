using TMPro;
using UnityEngine;

// Manages the player's score and updates the UI
public class ScoreManager : MonoBehaviour
{
    // Current score, shared across scenes
    public static int score = 0;
    // UI element to display the score
    public TextMeshProUGUI scoreText;
    // Singleton instance to persist across scenes
    private static ScoreManager instance;

    // Ensure only one ScoreManager exists
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScoreText();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Initialize score and UI
    void Start()
    {
        score = 0;
        UpdateScoreText();
    }

    // Update score UI when needed
    void Update()
    {
        if (!GameStateManager.IsPaused && scoreText != null)
        {
            UpdateScoreText();
        }
    }

    // Add points to the score and update UI
    public static void AddScore(int points)
    {
        if (!GameStateManager.IsPaused)
        {
            score += points;
            if (instance != null && instance.scoreText != null)
            {
                instance.UpdateScoreText();
            }
            else
            {
                Debug.LogWarning("ScoreManager instance or scoreText is null! Attempting to reinitialize...");
                if (instance == null)
                {
                    GameObject scoreManagerObj = new GameObject("ScoreManager");
                    instance = scoreManagerObj.AddComponent<ScoreManager>();
                    DontDestroyOnLoad(scoreManagerObj);
                }
                instance.InitializeScoreText();
                instance.UpdateScoreText();
            }
        }
    }

    // Update the score text UI
    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
            Debug.Log("Score updated to: " + score);
        }
        else
        {
            Debug.LogError("scoreText is not assigned!");
        }
    }

    // Find and assign the scoreText UI element
    private void InitializeScoreText()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            if (scoreText == null)
            {
                Debug.LogError("No ScoreText found in the scene! Please assign a TextMeshProUGUI named 'ScoreText' or drag it into the Inspector.");
            }
        }
    }
}