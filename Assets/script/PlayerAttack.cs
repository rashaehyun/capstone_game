using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Transform firePoint;  // 발사 위치 (자식 오브젝트)
    public float attackCooldown = 0.3f; // 투사체 발사 간격
    public SpriteRenderer spriteRenderer; // 좌우 시선 판별용

    private Camera mainCamera;
    private float lastAttackTime = -Mathf.Infinity; // 마지막 발사 시간 기록

    void Awake()
    {
        mainCamera = Camera.main;

        // SpriteRenderer 자동 연결 (없으면 수동 연결도 가능)
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
            // 마우스 클릭 + 쿨타임 체크
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot();
            lastAttackTime = Time.time; // 마지막 발사 시간 갱신
        }
    }

    void Shoot()
    {
            // 마우스 위치 계산
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

            // 방향 벡터 계산
        Vector2 direction = (mouseWorldPos - firePoint.position).normalized;

            // 발사 위치
        Vector3 spawnPos = firePoint.position;

            // 발사 방향 보정 (좌우 반전 시 firePoint도 반대 위치일 경우)
        if (spriteRenderer.flipX)
        {
            // 좌우 반전이라면 firePoint 위치도 반대일 수 있으니 x축 대칭 처리
            spawnPos.x = transform.position.x - (firePoint.position.x - transform.position.x);
        }

            // 투사체 생성 및 발사
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;

        // 공격 사운드 재생
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlayAttackSound();
        }
    }


}
