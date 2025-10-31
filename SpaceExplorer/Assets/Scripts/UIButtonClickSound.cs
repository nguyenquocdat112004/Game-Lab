using UnityEngine;

// Plays a click sound when a UI button is clicked
public class UIButtonClickSound : MonoBehaviour
{
    // Audio clip for the button click sound
    public AudioClip clickSound;
    // Static AudioSource for playing the click sound
    private static AudioSource sfxSource;

    // Initialize the AudioSource
    void Awake()
    {
        if (sfxSource == null)
        {
            GameObject sfxPlayer = GameObject.Find("SFXPlayer");
            if (sfxPlayer != null)
                sfxSource = sfxPlayer.GetComponent<AudioSource>();
        }
    }

    // Play the button click sound
    public void PlayClick()
    {
        if (sfxSource != null && clickSound != null)
            sfxSource.PlayOneShot(clickSound);
    }
}