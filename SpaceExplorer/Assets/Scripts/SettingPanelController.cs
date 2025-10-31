using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

// Manages the settings panel for adjusting audio volumes
public class SettingPanelController : MonoBehaviour
{
    // UI References
    [Header("UI References")]
    // Settings panel UI
    public GameObject settingPanel;
    // Button to open the settings panel
    public Button settingsButton;
    // Button to save settings
    public Button saveButton;
    // Button to cancel settings changes
    public Button cancelButton;
    // Slider for music volume
    public Slider musicSlider;
    // Slider for sound effects volume
    public Slider soundSlider;

    // Audio
    [Header("Audio")]
    // AudioMixer for volume control
    public AudioMixer audioMixer;

    // Original slider values for canceling changes
    private float originalMusicSliderValue;
    private float originalSFXSliderValue;

    // Initialize the settings panel
    void Start()
    {
        settingPanel.SetActive(false);

        // Assign button click listeners
        settingsButton.onClick.AddListener(OpenSettings);
        cancelButton.onClick.AddListener(CancelSettings);
        saveButton.onClick.AddListener(SaveSettings);

        // Assign slider value change listeners
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChanged(); });
        soundSlider.onValueChanged.AddListener(delegate { OnSFXSliderChanged(); });

        // Load saved settings
        LoadSettings();
    }

    // Open the settings panel and store current slider values
    public void OpenSettings()
    {
        settingPanel.SetActive(true);
        originalMusicSliderValue = musicSlider.value;
        originalSFXSliderValue = soundSlider.value;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    // Revert to original slider values and close the settings panel
    void CancelSettings()
    {
        musicSlider.SetValueWithoutNotify(originalMusicSliderValue);
        soundSlider.SetValueWithoutNotify(originalSFXSliderValue);
        ApplyVolume("MusicVolume", originalMusicSliderValue);
        ApplyVolume("SFXVolume", originalSFXSliderValue);
        settingPanel.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    // Save the current slider values and close the settings panel
    void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", soundSlider.value);
        PlayerPrefs.Save();
        settingPanel.SetActive(false);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
    }

    // Load saved volume settings
    void LoadSettings()
    {
        float musicValue = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicSlider.SetValueWithoutNotify(musicValue);
        soundSlider.SetValueWithoutNotify(sfxValue);
        ApplyVolume("MusicVolume", musicValue);
        ApplyVolume("SFXVolume", sfxValue);
    }

    // Update music volume based on slider value
    void OnMusicSliderChanged()
    {
        ApplyVolume("MusicVolume", musicSlider.value);
    }

    // Update sound effects volume based on slider value
    void OnSFXSliderChanged()
    {
        ApplyVolume("SFXVolume", soundSlider.value);
    }

    // Apply volume to the AudioMixer parameter
    void ApplyVolume(string parameter, float sliderValue)
    {
        float volume = sliderValue <= 0.0001f ? -80f : Mathf.Log10(sliderValue) * 20f;
        audioMixer.SetFloat(parameter, volume);
    }
}