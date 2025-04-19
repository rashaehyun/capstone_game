using UnityEngine;

public class OrbitLookFollower : MonoBehaviour
{
    public Transform player;              // �÷��̾� �߽�
    public Vector3 centerOffset = Vector3.zero; // �߽� ��ġ ������ (ex. �������� Vector3(0, 1, 0))
    public float radius = 2f;             // �� �˵� ������
    public float angleOffset = 0f;        // ���� ������
    public bool lookAtPlayer = true;      // �÷��̾� �ٶ󺸰� ����

    void Update()
    {
        if (player == null) return;

        // 1. ���� �߽� ��ġ = �÷��̾� ��ġ + ������
        Vector3 center = player.position + centerOffset;

        // 2. ���콺 ���� ���
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector3 direction = (mouseWorld - center).normalized;

        // 3. ���콺 ���� ���� + ������
        float angleToMouse = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = angleToMouse + angleOffset;

        // 4. ��ġ ��� (�˵� ��)
        float rad = finalAngle * Mathf.Deg2Rad;
        Vector3 orbitOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
        transform.position = center + orbitOffset;

        // 5. ���� ���� (����)
        if (lookAtPlayer)
        {
            Vector3 toCenter = center - transform.position;
            float angle = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
    }
}
