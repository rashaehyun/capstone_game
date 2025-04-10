using UnityEngine;
using UnityEngine.U2D; // PixelPerfectCamera 네임스페이스

public class CameraFollowSmooth : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 4, -10);
    public float smoothTime = 0.2f;

    [Header("Pixel Perfect Setting")]
    public int pixelsPerUnit = 32;
    public int refWidth = 1920;
    public int refHeight = 1080;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Pixel Perfect Camera 컴포넌트 찾아서 설정
        PixelPerfectCamera ppc = GetComponent<PixelPerfectCamera>();
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
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
