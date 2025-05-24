using UnityEngine;

public class WallGrab2 : MonoBehaviour
{
    [Header("벽 감지")]
    [SerializeField] private Transform wallRayOrigin;        // Ray 시작점
    [SerializeField] private float wallCheckDistance = 0.3f; // 감지 거리
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 baseRayDirection = Vector2.right; // 👉 기본 방향은 오른쪽
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D body;
    private PlayerJump playerJump;

    private bool isWallGrabbing;
    private float originalGravity;

    private bool prevFlipX;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 또는 [SerializeField]로 수동 연결
        originalGravity = body.gravityScale;
    }

    private void Update()
    {
        bool isGrounded = IsGrounded();
        bool isTouchingWall = IsTouchingWall();
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalVelocity = body.linearVelocity.y;

        bool shouldStartGrabbing = isTouchingWall && !isGrounded && verticalVelocity < -1.0f && Mathf.Abs(horizontalInput) > 0.1f;

        if (shouldStartGrabbing && !isWallGrabbing)
        {
            isWallGrabbing = true;
            body.gravityScale = 0f;
            body.linearVelocity = Vector2.zero;
        }

        if (isWallGrabbing)
        {
            if (shouldStartGrabbing)
            {
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                isWallGrabbing = false;
                body.gravityScale = originalGravity;
            }
        }

        if (!isWallGrabbing && body.gravityScale != originalGravity)
        {
            body.gravityScale = originalGravity;
        }

        bool currentFlipX = spriteRenderer.flipX;

        if (currentFlipX != prevFlipX)
        {
            Debug.Log($"[WallGrab2] flipX changed → {currentFlipX}");
            prevFlipX = currentFlipX;
        }
    }

    private bool IsGrounded()
    {
        return playerJump != null && playerJump.GetIsGrounded();
    }

    private bool IsTouchingWall()
    {
        if (wallRayOrigin == null || spriteRenderer == null) return false;

        // 👉 flipX 기준으로 방향 반전
        float flip = spriteRenderer.flipX ? -1f : 1f;
        Vector2 direction = baseRayDirection * flip;

        RaycastHit2D hit = Physics2D.Raycast(wallRayOrigin.position, direction, wallCheckDistance, wallLayer);

#if UNITY_EDITOR
        Debug.DrawRay(wallRayOrigin.position, direction * wallCheckDistance, Color.cyan);
#endif

        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (wallRayOrigin != null && spriteRenderer != null)
        {
            float flip = spriteRenderer.flipX ? -1f : 1f;
            Vector3 direction = baseRayDirection * flip;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(wallRayOrigin.position, wallRayOrigin.position + (Vector3)(direction * wallCheckDistance));
        }
    }

    public bool IsGrabbingWall()
    {
        return isWallGrabbing;
    }
}
