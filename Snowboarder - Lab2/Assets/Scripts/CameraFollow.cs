using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Đối tượng cần theo dõi (trái bóng)
    public Vector3 offset;    // Khoảng cách giữa camera và đối tượng
    public float verticalOffset = 2f; // Điều chỉnh vị trí trái bóng trên màn hình

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = target.position + offset;
            newPosition.y += verticalOffset; // Đẩy camera lên cao hơn
            transform.position = newPosition;
        }
    }
}
