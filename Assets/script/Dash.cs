using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private Rigidbody2D body;
    private bool isDashing = false;
    private bool canDash = true;
    private Camera mainCamera;
    private Player_Move move;
    private SpriteRenderer spriteRenderer;

    // ✅ 외부에서 대시 상태 확인 가능 (ex. 이동/점프 차단용)
    public bool IsDashing => isDashing;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        move = GetComponent<Player_Move>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnDash()
    {
        if (!isDashing && canDash)
        {
            StartCoroutine(DashTowardMouse());
        }
    }

    private IEnumerator DashTowardMouse()
    {
        isDashing = true;
        canDash = false;

        // ✅ 중력 영향 제거
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;

        // ✅ 마우스 → 방향 계산
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        if (direction.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (direction.x < -0.1f)
            spriteRenderer.flipX = true;

        if (Mathf.Abs(direction.x) > 0.1f)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }

        // ✅ 수직 튐 방지
        direction = new Vector2(direction.x, direction.y * 0.3f).normalized;

        // ✅ 대시 이펙트 or 애니메이션 트리거 위치 (선택)
        // anim.SetTrigger("Dash");
        // Instantiate(dashEffect, transform.position, Quaternion.identity);

        // ✅ 대시 실행
        body.linearVelocity = direction * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        // ✅ 복구
        body.linearVelocity = Vector2.zero;
        body.gravityScale = originalGravity;
        isDashing = false;

        // ✅ 쿨타임 대기 후 다시 가능
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
