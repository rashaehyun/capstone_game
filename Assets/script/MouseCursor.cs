using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    private RectTransform crosshair;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        Cursor.visible = false; // 기본 커서 숨기기 (선택)
    }

    void Update()
    {
        crosshair.position = Input.mousePosition;
    }
}
