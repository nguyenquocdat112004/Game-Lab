using UnityEngine;
using UnityEngine.Audio;

// Manages audio playback for sound effects and background music
public class AudioManager : MonoBehaviour
{
    // Singleton instance to persist across scenes
    public static AudioManager Instance;

    // Audio clips for various game events
    public AudioClip buttonClick;
    public AudioClip laserShoot;
    public AudioClip hitAsteroid;
    public AudioClip starFall;
    public AudioClip starCollect;
    public AudioClip backgroundMusic;
    public AudioClip gameOver;

    // AudioSource for sound effects
    private AudioSource sfxSource;
    // AudioSource for background music
    private AudioSource musicSource;
    // AudioMixer for volume control
    public AudioMixer audioMixer;

    // Initialize the AudioManager as a singleton
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize AudioSource components
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;

            // Assign AudioMixer groups
            if (audioMixer != null)
            {
                sfxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
                musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")[0];
            }
            else
            {
                Debug.LogError("AudioMixer is not assigned in AudioManager!");
            }
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Play background music
    public void PlayMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
    }

    // Stop background music
    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    // Set volume for a specific AudioMixer parameter
    public void SetVolume(string parameter, float value)
    {
        if (audioMixer != null)
        {
            audioMixer.SetFloat(parameter, Mathf.Log10(value) * 20f);
        }
    }
}