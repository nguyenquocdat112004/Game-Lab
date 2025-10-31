using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages game state, including pausing, resuming, and settings
public class GameStateManager : MonoBehaviour
{
    // Flag to track if the game is paused
    public static bool IsPaused = false;
    // UI panel for the pause menu
    public GameObject pausePanel;
    // Button to resume the game
    public Button resumeButton;
    // Button to open the settings panel
    public Button settingsButton;

    // Initialize the game state
    void Start()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    // Pause the game and show the pause menu
    public void PauseGame()
    {
        if (!IsPaused)
        {
            Time.timeScale = 0f;
            IsPaused = true;
            pausePanel.SetActive(true);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            AudioManager.Instance.StopMusic();
        }
    }

    // Resume the game and hide the pause menu
    public void ResumeGame()
    {
        if (IsPaused)
        {
            Time.timeScale = 1f;
            IsPaused = false;
            pausePanel.SetActive(false);
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            AudioManager.Instance.PlayMusic();
        }
    }

    // Open the settings panel
    public void OpenSettings()
    {
        var controller = FindAnyObjectByType<SettingPanelController>();
        if (controller != null)
        {
            controller.OpenSettings();
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        }
        else
        {
            Debug.LogError("No SettingPanelController found in the scene!");
        }
    }

    // Reset the game to its initial state
    public void ResetGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        ScoreManager.score = 0;
        if (FindAnyObjectByType<ScoreManager>() != null)
        {
            FindAnyObjectByType<ScoreManager>().UpdateScoreText();
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Transition to the End Game scene
    public void QuitGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        SceneManager.LoadScene("EndGame");
    }
}
