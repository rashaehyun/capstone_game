using UnityEngine;

public class Scroll_bg : MonoBehaviour
{
    [Tooltip("카메라 Transform을 넣으세요.")]
    public Transform cameraTransform;

    [Tooltip("패럴랙스 이동 비율. 0.1~1 사이 추천")]
    public float parallaxMultiplier = 0.5f;

    [Tooltip("배경 이동 속도 (값이 클수록 더 빠르게 따라옴)")]
    public float followSpeed = 5f;

    [Tooltip("사용 중인 Pixels Per Unit 값")]
    public float pixelsPerUnit = 32f;

    [Header("OffSet")]
    [Tooltip("배경의 X축 오프셋")]
    public float offsetX = 0f;

    [Tooltip("배경의 Y축 오프셋")]
    public float offsetY = 0f;

    private float textureUnitSizeX;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            textureUnitSizeX = sr.sprite.texture.width / sr.sprite.pixelsPerUnit;
        }
        else
        {
            Debug.LogError("SpriteRenderer 또는 Sprite가 없습니다!");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        // 목표 위치 계산 (X, Y)
        float targetX = cameraTransform.position.x * parallaxMultiplier + offsetX;
        // float targetY = cameraTransform.position.y * parallaxMultiplier + offsetY; // Y축 따라가지 않음

        // 무한 스크롤 (X축만 적용)
        float deltaX = targetX - transform.position.x;
        if (Mathf.Abs(deltaX) >= textureUnitSizeX)
        {
            targetX += deltaX % textureUnitSizeX;
        }

        // 부드럽게 따라가기 (X만 적용, Y는 현재 위치 유지)
        float newX = Mathf.Lerp(transform.position.x, targetX, followSpeed * Time.deltaTime);
        // float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime); // Y축 따라가지 않음
        float newY = transform.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);

        // 선택: 픽셀 스냅 생략 가능
        // float snappedX = Mathf.Round(transform.position.x * pixelsPerUnit) / pixelsPerUnit;
        // float snappedY = Mathf.Round(transform.position.y * pixelsPerUnit) / pixelsPerUnit;
        // transform.position = new Vector3(snappedX, snappedY, transform.position.z);
    }
}
