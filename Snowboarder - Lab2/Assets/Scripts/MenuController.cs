using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button newGameButton, continueButton, optionsButton, exitButton;
    public AudioClip buttonClickSFX;

    private void Start()
    {
        newGameButton.onClick.AddListener(OnNewGame);
        continueButton.onClick.AddListener(OnContinue);
        optionsButton.onClick.AddListener(OnOptions);
        exitButton.onClick.AddListener(OnExit);
    }

    public void OnNewGame()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnContinue()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
        // TODO: Tải tiến độ game (nếu có)
        SceneManager.LoadScene("LoadingScene");
    }

    public void OnOptions()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
        SceneManager.LoadScene("Options");
    }

    public void OnExit()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
        Application.Quit();
    }
}