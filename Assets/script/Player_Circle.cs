using UnityEngine;

public class OrbitLookFollower : MonoBehaviour
{
    public Transform player;              // 플레이어 중심
    public Vector3 centerOffset = Vector3.zero; // 중심 위치 오프셋 (ex. 위쪽으로 Vector3(0, 1, 0))
    public float radius = 2f;             // 원 궤도 반지름
    public float angleOffset = 0f;        // 각도 오프셋
    public bool lookAtPlayer = true;      // 플레이어 바라보게 할지

    void Update()
    {
        if (player == null) return;

        // 1. 실제 중심 위치 = 플레이어 위치 + 오프셋
        Vector3 center = player.position + centerOffset;

        // 2. 마우스 방향 계산
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector3 direction = (mouseWorld - center).normalized;

        // 3. 마우스 방향 각도 + 오프셋
        float angleToMouse = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = angleToMouse + angleOffset;

        // 4. 위치 계산 (궤도 위)
        float rad = finalAngle * Mathf.Deg2Rad;
        Vector3 orbitOffset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;
        transform.position = center + orbitOffset;

        // 5. 방향 설정 (선택)
        if (lookAtPlayer)
        {
            Vector3 toCenter = center - transform.position;
            float angle = Mathf.Atan2(toCenter.y, toCenter.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
    }
}
