using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f; // Thời gian trễ trước khi chuyển cảnh
    [SerializeField] ParticleSystem finishEffect; // Hiệu ứng hạt khi hoàn thành
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player đã chạm vào FinishLine!");
            if (finishEffect != null)
            {
                GetComponent<AudioSource>().Play();
                finishEffect.Play();
                //Debug.Log("Hiệu ứng hạt đã được kích hoạt!");
            }
            else
            {
                Debug.LogWarning("Particle System chưa được gắn vào script!");
            }

            Invoke("ReloadScene", loadDelay);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(3);
    }
}
