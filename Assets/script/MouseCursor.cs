using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    private RectTransform crosshair;

    void Start()
    {
        crosshair = GetComponent<RectTransform>();
        Cursor.visible = false; // �⺻ Ŀ�� ����� (����)
    }

    void Update()
    {
        crosshair.position = Input.mousePosition;
    }
}
