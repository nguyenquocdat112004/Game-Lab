using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

// Manages the loading screen with a progress bar
public class LoadingManager : MonoBehaviour
{
    // Image component for the loading bar fill
    public Image loadingBarFill;
    // Current fill amount of the loading bar
    private float currentFill = 0f;
    // Speed at which the loading bar fills
    public float fillSpeed = 0.5f;
    // AudioSource for loading screen sound
    public AudioSource loadingAudio;
    // Flag to track if the loading audio has started
    private bool hasStartedAudio = false;

    // Start the asynchronous loading process
    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    // Load the Gameplay scene asynchronously
    IEnumerator LoadAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GamePlay");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Update the loading bar progress
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f);
            currentFill = Mathf.Lerp(currentFill, targetProgress, fillSpeed * Time.deltaTime);
            loadingBarFill.fillAmount = currentFill;

            // Play loading audio when the bar starts filling
            if (!hasStartedAudio && currentFill > 0.01f)
            {
                if (loadingAudio != null)
                {
                    loadingAudio.loop = true;
                    loadingAudio.Play();
                }
                hasStartedAudio = true;
            }

            // Complete the loading process
            if (operation.progress >= 0.9f && currentFill >= 0.98f)
            {
                loadingBarFill.fillAmount = 1f;
                if (loadingAudio != null)
                {
                    loadingAudio.Stop();
                }
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}