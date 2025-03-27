using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    float jumpPower;
    [SerializeField]
    bool isGround;

    private int jumpCount = 0;  // 점프 횟수를 추적할 변수
    private int maxJumpCount = 2; // 최대 점프 횟수

    Rigidbody2D body;
    Animator anim;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        anim.SetBool("Ground", isGround);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // "Ground" 태그를 가진 오브젝트와 접촉했을 때
        if (collision.CompareTag("Ground"))
        {
            // 바닥에 닿으면 점프 횟수 리셋
            if (jumpCount > 0)
                jumpCount = 0;
            isGround = true; // 바닥에 있을 때 isGround를 true로 설정
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 바닥을 떠나면 isGround를 false로 설정
        if (collision.CompareTag("Ground"))
            isGround = false;
    }

    private void OnJump()
    {
        if ((isGround && jumpCount == 0) || jumpCount == 1)  // 점프 횟수가 0이거나 1일 때만 점프
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;  // 점프 횟수 증가

            isGround = false;  // 점프할 때는 isGround를 false로 설정
        }
    }
}
