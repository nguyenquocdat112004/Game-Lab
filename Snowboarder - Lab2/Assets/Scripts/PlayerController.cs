using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float boostSpeed = 30f;
    public float moveSpeed = 20f;
    public float jumpForce = 10f;
    [SerializeField] private bool autoMove = true; // Bật/tắt di chuyển tự động
    private Rigidbody2D rb;
    private bool isGrounded;
    private SurfaceEffector2D surfaceEffector2D;
    private Vector3 lastFallPosition;
    public int lives = 3;
    private float distanceTraveled = 0f;
    private Vector3 lastPosition;
    private float baseScore = 0f; // Điểm từ tốc độ và khoảng cách
    private float bonusScore = 0f; // Điểm từ bông tuyết
    public float score => baseScore + bonusScore; // Tổng điểm
    private Vector3 lastCheckpoint;
    private bool isPaused = false;
    [SerializeField] private GameObject gameOverPanel; // Gán GameOverPanel trong Inspector
    [SerializeField] private Button restartButton;    // Gán RestartButton trong Inspector
    [SerializeField] private Button backToMenuButton; // Gán BacktoMenuButton trong Inspector
    [SerializeField] private Button pauseButton;      // Gán PauseButton trong Inspector
    [SerializeField] private GameObject pausePanel;   // Gán PausePanel trong Inspector
    [SerializeField] private Button continueButton;   // Gán ContinueButton trong Inspector
    [SerializeField] private Button optionsButton;    // Gán SettingsButton trong Inspector
    private Vector3 pausePosition;                    // Lưu vị trí khi pause

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 0.5f;
        surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        if (surfaceEffector2D == null) Debug.LogError("SurfaceEffector2D not found!");
        lastPosition = transform.position;
        lastCheckpoint = transform.position; // Bắt đầu với vị trí ban đầu
        if (gameOverPanel != null) gameOverPanel.SetActive(false); // Ẩn GameOverPanel
        if (pausePanel != null) pausePanel.SetActive(false);      // Ẩn PausePanel
        // Gán sự kiện cho các nút
        if (restartButton != null) restartButton.onClick.AddListener(RestartGame);
        if (backToMenuButton != null) backToMenuButton.onClick.AddListener(GoToMenu);
        if (pauseButton != null) pauseButton.onClick.AddListener(TogglePause);
        if (continueButton != null) continueButton.onClick.AddListener(ContinueGame);
        if (optionsButton != null) optionsButton.onClick.AddListener(GoToSettings);
    }

    void Update()
    {
        if (!isPaused)
        {
            Move();
            Jump();
            RespondToBoost();
            UpdateDistance();
            UpdateBaseScore(); // Cập nhật điểm cơ bản
        }
    }

    void RespondToBoost()
    {
        if (Input.GetKey(KeyCode.UpArrow) && surfaceEffector2D != null)
        {
            surfaceEffector2D.speed = boostSpeed;
            Debug.Log("Boost activated, speed: " + boostSpeed);
        }
        else if (surfaceEffector2D != null)
        {
            surfaceEffector2D.speed = moveSpeed;
            Debug.Log("Normal speed: " + moveSpeed);
        }
    }

    void Move()
    {
        float moveX = 0f;
        if (autoMove)
        {
            // Di chuyển tự động về phía trước
            moveX = 1f; // Luôn di chuyển về phải (phía trước)
        }
        else
        {
            // Điều khiển thủ công, chỉ cho phép di chuyển về phía trước
            moveX = Input.GetAxisRaw("Horizontal");
            if (moveX < 0) moveX = 0; // Không cho phép di chuyển lùi
        }

        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveX * moveSpeed;
        rb.linearVelocity = velocity;

        if (moveX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveX == 0) transform.localScale = new Vector3(1, 1, 1); // Giữ hướng khi dừng
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void UpdateDistance()
    {
        float distanceDelta = Vector3.Distance(transform.position, lastPosition);
        distanceTraveled += distanceDelta;
        lastPosition = transform.position;
    }

    void UpdateBaseScore()
    {
        float speedScore = rb.linearVelocity.magnitude * 0.1f;
        float distanceScore = distanceTraveled * 0.05f;
        baseScore = speedScore + distanceScore; // Cập nhật điểm cơ bản
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ kiểm tra va chạm với bông tuyết (Snowflake)
        if (other.CompareTag("Snowflake"))
        {
            // Tăng điểm khi chạm bông tuyết
            bonusScore += 10f; // Tăng điểm bonus
            Debug.Log("Collected Snowflake! Total Score: " + score);
            other.gameObject.SetActive(false); // Biến mất sau khi chạm
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f) isGrounded = true;
    }

    public void LoseLife(Vector3 fallPosition)
    {
        lives--;
        lastFallPosition = fallPosition;
        Debug.Log("Lives: " + lives + ", Last Checkpoint: " + lastCheckpoint);
        if (lives <= 0)
        {
            transform.position = CrashDetector.initialPosition; // Quay về vị trí ban đầu
            rb.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity; // Reset góc quay
            lives = 3; // Reset mạng
            ShowGameOverMenu(); // Hiển thị menu khi hết mạng
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = lastCheckpoint; // Hồi sinh tại checkpoint
            transform.rotation = Quaternion.identity; // Reset góc quay
        }
    }

    private void ShowGameOverMenu()
    {
        if (gameOverPanel != null)
        {
            Time.timeScale = 0f; // Tạm dừng game
            gameOverPanel.SetActive(true); // Hiển thị panel
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian
        gameOverPanel.SetActive(false); // Ẩn panel
        transform.position = CrashDetector.initialPosition; // Quay về vị trí ban đầu
        rb.linearVelocity = Vector2.zero;
        transform.rotation = Quaternion.identity; // Reset góc quay
        lives = 3; // Reset mạng
        lastCheckpoint = transform.position; // Reset checkpoint
        baseScore = 0f; // Reset base score
        bonusScore = 0f; // Reset bonus score
        distanceTraveled = 0f; // Reset distance
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian
        gameOverPanel.SetActive(false); // Ẩn panel
        SceneManager.LoadScene("MainMenu"); // Quay về scene menu
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f; // Tạm dừng game
            pausePosition = transform.position; // Lưu vị trí hiện tại khi pause
            if (pausePanel != null) pausePanel.SetActive(true); // Hiển thị PausePanel
        }
        else
        {
            Time.timeScale = 1f; // Tiếp tục game
            if (pausePanel != null) pausePanel.SetActive(false); // Ẩn PausePanel
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian
        transform.position = pausePosition; // Quay lại vị trí pause
        rb.linearVelocity = Vector2.zero; // Reset vận tốc
        transform.rotation = Quaternion.identity; // Reset góc quay
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false); // Ẩn PausePanel
    }

    public void GoToSettings()
    {
        Time.timeScale = 1f; // Tiếp tục thời gian (nếu cần)
        pausePanel.SetActive(false); // Ẩn PausePanel
        SceneManager.LoadScene("Options");
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpoint = checkpointPosition;
    }

    public int GetLives() { return lives; }
    public float GetScore() { return score; } // Trả về tổng score
    public float GetSpeed() { return rb.linearVelocity.magnitude; }
}