using UnityEngine;

public class WallGrab2 : MonoBehaviour
{
    [Header("벽 감지")]
    [SerializeField] private Transform wallRayOrigin;
    [SerializeField] private float wallCheckDistance = 0.3f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector2 baseRayDirection = Vector2.right;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D body;
    private PlayerJump playerJump;
    private Animator animator;

    private bool isWallGrabbing;
    private bool prevFlipX;
    private float originalGravity;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerJump = GetComponent<PlayerJump>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalGravity = body.gravityScale;
    }

    private void Update()
    {
        // 현재 상태 정보 수집
        bool isGrounded = IsGrounded();
        bool isTouchingWall = IsTouchingWall();
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalVelocity = body.linearVelocity.y;

        // 붙기 조건
        bool shouldStartGrabbing =
            isTouchingWall &&
            !isGrounded &&
            verticalVelocity < -1.0f &&
            Mathf.Abs(horizontalInput) > 0.1f;

        // ray 방향 반전 처리
        FlipRayOriginWithSprite();

        // 상태 변화 시에만 처리
        if (shouldStartGrabbing != isWallGrabbing)
        {
            isWallGrabbing = shouldStartGrabbing;

            body.gravityScale = isWallGrabbing ? 0f : originalGravity;
            if (isWallGrabbing)
                body.linearVelocity = Vector2.zero; // 중단
            // animator 전이 트리거
            animator.SetBool("WallGrab", isWallGrabbing);

            Debug.Log(isWallGrabbing ? "🧲 Wall Grab 시작" : "⬅️ 벽에서 떨어짐");
        }

        // (선택) flipX 변화 감지
        TrackFlipXChange();
    }

    private void FlipRayOriginWithSprite()
    {
        if (wallRayOrigin == null || spriteRenderer == null) return;

        Vector3 localPos = wallRayOrigin.localPosition;
        localPos.x = Mathf.Abs(localPos.x) * (spriteRenderer.flipX ? -1f : 1f);
        wallRayOrigin.localPosition = localPos;
    }

    private void TrackFlipXChange()
    {
        bool currentFlipX = spriteRenderer.flipX;
        if (currentFlipX != prevFlipX)
        {
            prevFlipX = currentFlipX;
            //Debug.Log($"[FlipX] Changed to {currentFlipX}");
        }
    }

    private bool IsTouchingWall()
    {
        if (wallRayOrigin == null || spriteRenderer == null) return false;

        float flip = spriteRenderer.flipX ? -1f : 1f;
        Vector2 direction = baseRayDirection * flip;

        RaycastHit2D hit = Physics2D.Raycast(
            wallRayOrigin.position,
            direction,
            wallCheckDistance,
            wallLayer
        );

        //Debug.DrawRay(wallRayOrigin.position, direction * wallCheckDistance, Color.cyan);
        return hit.collider != null;
    }

    private bool IsGrounded()
    {
        return playerJump != null && playerJump.GetIsGrounded();
    }

    public bool IsGrabbingWall()
    {
        return isWallGrabbing;
    }
}
