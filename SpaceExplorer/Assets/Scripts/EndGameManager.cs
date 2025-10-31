using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the End Game scene, displaying the final score and options
public class EndGameManager : MonoBehaviour
{
    // UI panel for the end game screen
    public GameObject endGameUI;
    // Text element to display the final score
    public TMP_Text scoreText;

    // Initialize the end game screen
    void Start()
    {
        ShowEndGame();
    }

    // Display the end game UI with the final score
    public void ShowEndGame()
    {
        endGameUI.SetActive(true);
        scoreText.text = "Your score: " + ScoreManager.score;
        // Pause the game
        Time.timeScale = 0f;
    }

    // Load the Main Menu scene
    public void OnMainMenu()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    // Quit the application
    public void OnQuit()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
        Application.Quit();
    }
}