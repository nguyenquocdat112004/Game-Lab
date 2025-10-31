using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private float minLoadDuration = 2f;

    void Start()
    {
        // Khởi tạo giá trị mặc định
        progressBar.value = 0;
        loadingText.text = "Loading... 0%";

        StartCoroutine(LoadSceneCoroutine());
    }

    IEnumerator LoadSceneCoroutine()
    {
        float timer = 0f;

        // Bắt đầu load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GamePlay");
        asyncLoad.allowSceneActivation = false;

        // Vòng lặp loading
        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;

            // Tính tiến trình load (0-90% từ async, 90-100% từ timer)
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            float timedProgress = Mathf.Clamp01(timer / minLoadDuration);
            float displayProgress = Mathf.Min(progress, timedProgress);

            // Cập nhật UI
            progressBar.value = displayProgress;
            loadingText.text = $"Loading... {displayProgress * 100:F0}%";

            // Chuyển scene khi hoàn thành
            if (displayProgress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}