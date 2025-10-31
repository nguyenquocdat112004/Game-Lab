using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the Main Menu scene's UI and transitions
public class MainMenu : MonoBehaviour
{
    // UI panel for showing game instructions
    public GameObject instructionsPanel;

    // Initialize the Main Menu
    void Start()
    {
        instructionsPanel.SetActive(false);
    }

    // Load the Loading Scene to start the game
    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    // Show the instructions panel
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    // Hide the instructions panel
    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    // Quit the application
    public void QuitGame()
    {
        Application.Quit();
    }
}