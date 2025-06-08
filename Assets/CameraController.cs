using UnityEngine;
using UnityEngine.U2D; // PixelPerfectCamera 네임스페이스

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4, -10);
    public float smoothTime = 0.2f;

    [Header("Pixel Perfect Setting")]
    public int pixelsPerUnit = 32;
    public int refWidth = 1920;
    public int refHeight = 1080;

    [Header("Camera Bounds")]
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Pixel Perfect Camera 설정
        /*PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();
        if (ppc != null)
        {
            ppc.assetsPPU = pixelsPerUnit;
            ppc.refResolutionX = refWidth;
            ppc.refResolutionY = refHeight;
            ppc.upscaleRT = true;
            ppc.pixelSnapping = true;
        }
        else
        {
            Debug.LogWarning("Pixel Perfect Camera 컴포넌트가 없습니다.");
        }*/
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // x, y 축만 clamp
            //float clampedX = Mathf.Clamp(smoothPosition.x, minPosition.x, maxPosition.x);
            //float clampedY = Mathf.Clamp(smoothPosition.y, minPosition.y, maxPosition.y);

           // transform.position = new Vector3(clampedX, clampedY, smoothPosition.z); // z는 offset에서 유지
        }
    }
}

