using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float cameraSpeed = 5.0f;

    public Vector2 minPosition;
    public Vector2 maxPosition;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(smoothPos.x, minPosition.x, maxPosition.x);
        float clampedY = Mathf.Clamp(smoothPos.y, minPosition.y, maxPosition.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
