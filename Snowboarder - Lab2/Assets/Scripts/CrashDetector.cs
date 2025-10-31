using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] AudioClip crashSFX; // Định nghĩa AudioClip cho âm thanh
    private AudioSource audioSource; // Tham chiếu đến AudioSource
    public Transform head;
    public Transform lowerBody;
    private PlayerController playerController;
    public static Vector3 initialPosition;
    private bool isRecovering = false;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        // Gắn AudioSource từ GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource không được gắn vào GameObject!");
        }

        if (initialPosition == Vector3.zero)
        {
            initialPosition = playerController.transform.position;
        }
    }

    void Update()
    {
        if (isRecovering) return;
        float zRotation = transform.eulerAngles.z;

        // Kiểm tra nếu nhân vật bị lật
        if (zRotation > 90 && zRotation < 270)
        {
            HandleFlip();
        }
    }

    void HandleFlip()
    {
        if (isRecovering) return;

        // Phát âm thanh ngay lập tức
        if (crashSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(crashSFX); // Phát âm thanh ngã
        }
        else
        {
            Debug.LogWarning("crashSFX hoặc AudioSource chưa được gắn!");
        }

        Vector3 fallPosition = transform.position;
        Debug.Log("Fall position: " + fallPosition);

        // Gọi LoseLife để giảm mạng
        playerController.LoseLife(fallPosition);

        // Reset vị trí và mạng nếu hết mạng
        if (playerController.GetLives() <= 0)
        {
            playerController.transform.position = CrashDetector.initialPosition;
            playerController.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            playerController.transform.rotation = Quaternion.identity; // Reset góc quay
            playerController.lives = 3; // Reset mạng
        }
        else
        {
            StartCoroutine(RecoverAfterFall());
        }
    }


    System.Collections.IEnumerator RecoverAfterFall()
    {
        isRecovering = true;
        yield return new WaitForSeconds(1.0f); // Tăng delay lên 1 giây để xử lý physics
        isRecovering = false;
    }
}
