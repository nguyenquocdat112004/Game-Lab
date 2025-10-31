using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class OptionController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public TextMeshProUGUI musicLabel, sfxLabel;
    public AudioMixer audioMixer;
    public UnityEngine.UI.Button backButton;
    public AudioClip buttonClickSFX;

    private void Start()
    {
        // Tải cài đặt đã lưu
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        backButton.onClick.AddListener(BackToMenu);
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
        musicLabel.text = $"Music Volume: {Mathf.RoundToInt(value * 100)}%";
    }


    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
        sfxLabel.text = $"SFX Volume: {Mathf.RoundToInt(value * 100)}%";
    }

    public void BackToMenu()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX);
        SceneManager.LoadScene("MainMenu");
    }
}