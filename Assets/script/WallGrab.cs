using UnityEngine;

public class WallGrab : MonoBehaviour
{
    [Header("벽 체크")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckRadius = 0.02f;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D body;
    private PlayerJump playerJump;
    private bool isWallGrabbing;
    private float originalGravity;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();

        // 👉 현재 Rigidbody의 gravityScale을 기억해둠 (예: 2)
        originalGravity = body.gravityScale;
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();
        bool isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, wallLayer);
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalVelocity = body.linearVelocity.y;

        bool shouldStartGrabbing = isTouchingWall && !isGrounded && verticalVelocity < -0.1f && Mathf.Abs(horizontalInput) > 0.1f;

        // 매달리기 시작 조건
        if (shouldStartGrabbing && !isWallGrabbing)
        {
            isWallGrabbing = true;
            body.gravityScale = 0f;
            body.linearVelocity = Vector2.zero;
        }

        // 매달림 유지
        if (isWallGrabbing)
        {
            if (shouldStartGrabbing)
            {
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                // 해제
                isWallGrabbing = false;
                body.gravityScale = originalGravity;
            }
        }

        // 벽에도 닿지 않고, 매달림도 아닌 경우 → 중력 반드시 복원
        if (!isWallGrabbing && body.gravityScale == 0f)
        {
            body.gravityScale = originalGravity;
        }

        // 매달림 여부 + 물리 상태 요약 로그
        Debug.Log($"[WallGrab] wall={isTouchingWall}, grounded={isGrounded}, grabbing={isWallGrabbing}, grav={body.gravityScale}, vel={body.linearVelocity}");

        // 조건 성립 여부 확인용 추가 로그
        Debug.Log($"[WallGrab Condition] touchingWall={isTouchingWall}, grounded={isGrounded}, velY={verticalVelocity}, inputX={horizontalInput}, shouldGrab={shouldStartGrabbing}");

        if (!isGrounded && !isWallGrabbing)
        {
            Debug.Log($"🧪 FLOATING STATE → wall={isTouchingWall}, grounded={isGrounded}, grabbing={isWallGrabbing}, grav={body.gravityScale}, vel={body.linearVelocity}");
        }

        if (!isWallGrabbing && body.gravityScale != originalGravity)
        {
            body.gravityScale = originalGravity;
        }

    }

    private bool IsGrounded()
    {
        return playerJump != null && playerJump.GetIsGrounded();
    }

    private void OnDrawGizmosSelected()
    {
        if (wallCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(wallCheck.position, wallCheckRadius);
        }
    }
    public bool IsGrabbingWall()
    {
        return isWallGrabbing;
    }
}
