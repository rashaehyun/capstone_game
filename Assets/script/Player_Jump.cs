using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField]
    float jumpPower;
    [SerializeField]
    bool isGround;

    private int jumpCount = 0;  // ���� Ƚ���� ������ ����
    private int maxJumpCount = 2; // �ִ� ���� Ƚ��

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
        // "Ground" �±׸� ���� ������Ʈ�� �������� ��
        if (collision.CompareTag("Ground"))
        {
            // �ٴڿ� ������ ���� Ƚ�� ����
            if (jumpCount > 0)
                jumpCount = 0;
            isGround = true; // �ٴڿ� ���� �� isGround�� true�� ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �ٴ��� ������ isGround�� false�� ����
        if (collision.CompareTag("Ground"))
            isGround = false;
    }

    private void OnJump()
    {
        if ((isGround && jumpCount == 0) || jumpCount == 1)  // ���� Ƚ���� 0�̰ų� 1�� ���� ����
        {
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;  // ���� Ƚ�� ����

            isGround = false;  // ������ ���� isGround�� false�� ����
        }
    }
}
