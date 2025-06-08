using UnityEngine;

public class Scroll_bg : MonoBehaviour
{
    [Tooltip("ī�޶� Transform�� ��������.")]
    public Transform cameraTransform;

    [Tooltip("�з����� �̵� ����. 0.1~1 ���� ��õ")]
    public float parallaxMultiplier = 0.5f;

    [Tooltip("��� �̵� �ӵ� (���� Ŭ���� �� ������ �����)")]
    public float followSpeed = 5f;

    [Tooltip("��� ���� Pixels Per Unit ��")]
    public float pixelsPerUnit = 32f;

    [Header("OffSet")]
    [Tooltip("����� X�� ������")]
    public float offsetX = 0f;

    [Tooltip("����� Y�� ������")]
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
            Debug.LogError("SpriteRenderer �Ǵ� Sprite�� �����ϴ�!");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        // ��ǥ ��ġ ��� (X, Y)
        float targetX = cameraTransform.position.x * parallaxMultiplier + offsetX;
        // float targetY = cameraTransform.position.y * parallaxMultiplier + offsetY; // Y�� ������ ����

        // ���� ��ũ�� (X�ุ ����)
        float deltaX = targetX - transform.position.x;
        if (Mathf.Abs(deltaX) >= textureUnitSizeX)
        {
            targetX += deltaX % textureUnitSizeX;
        }

        // �ε巴�� ���󰡱� (X�� ����, Y�� ���� ��ġ ����)
        float newX = Mathf.Lerp(transform.position.x, targetX, followSpeed * Time.deltaTime);
        // float newY = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime); // Y�� ������ ����
        float newY = transform.position.y;

        transform.position = new Vector3(newX, newY, transform.position.z);

        // ����: �ȼ� ���� ���� ����
        // float snappedX = Mathf.Round(transform.position.x * pixelsPerUnit) / pixelsPerUnit;
        // float snappedY = Mathf.Round(transform.position.y * pixelsPerUnit) / pixelsPerUnit;
        // transform.position = new Vector3(snappedX, snappedY, transform.position.z);
    }
}
