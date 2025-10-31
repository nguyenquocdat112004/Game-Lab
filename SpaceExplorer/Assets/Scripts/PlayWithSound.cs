using UnityEngine;
using UnityEngine.SceneManagement;

// Plays a sound and loads a scene with a delay
public class PlayWithSound : MonoBehaviour
{
    // AudioSource for playing the click sound
    public AudioSource audioSource;
    // Audio clip for the button click sound
    public AudioClip clickSound;
    // AudioSource for background music
    public AudioSource bgmAudioSource;
    // Name of the scene to load
    public string sceneToLoad;
    // Delay before loading the scene (in seconds)
    public float delay = 3f;

    // Play the click sound and load the scene
    public void OnPlayClicked()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Stop background music
        if (bgmAudioSource != null)
            bgmAudioSource.Stop();

        // Load the scene after a delay
        Invoke("LoadScene", delay);
    }

    // Load the specified scene
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}