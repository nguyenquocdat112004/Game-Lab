using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDDisplay : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI speedText;
    public PlayerController player;

    void Update()
    {
        livesText.text = "Lives: " + player.GetLives();
        scoreText.text = "Score: " + player.GetScore().ToString("F1");
        speedText.text = "Speed: " + player.GetSpeed().ToString("F1") + " m/s";
    }
}