using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    [SerializeField] private float speed;
    private float inputValue;

    [Header("넉백 설정")]
    [SerializeField] private float knockbackPowerX = 7f;  // 좌우 힘
    [SerializeField] private float knockbackPowerY = 4f;  // 위쪽 힘

    [Header("중력 및 낙하 설정")]
    [SerializeField] private float fallGravityMultiplier = 2.5f;  // 낙하 시 중력 배수
    [SerializeField] private float maxFallSpeed = -30f;           // 낙하 최대 속도

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer; // ��������Ʈ ������ �߰�
    private Animator anim;
    private PlayerDash dash;
    public float InputX => inputValue;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ��������
        anim = GetComponent<Animator>();
        dash = GetComponent<PlayerDash>();
    }

    private void FixedUpdate()
    {

        if (dash != null && dash.IsDashing)
            return;

        body.linearVelocityX = inputValue * speed;

        // 낙하 중일 때 중력 강화
        if (body.linearVelocity.y < 0)
        {
            body.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // 낙하 속도 제한
        if (body.linearVelocity.y < maxFallSpeed)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, maxFallSpeed);
        }

        // ĳ���� ���� ��ȯ
        if (inputValue > 0)
        { 
            spriteRenderer.flipX = false; // ������ �̵� �� ���� ����
        }
        else if (inputValue < 0)
        { 
            spriteRenderer.flipX = true;  // ���� �̵� �� �¿� ����
        }

        bool isRunning = Mathf.Abs(inputValue) > 0.01f;
        anim.SetBool("isRun", isRunning);
    }

    private void OnMove(InputValue value)
    {
        inputValue = value.Get<Vector2>().x;
    }

    //���Ϳ� �浹 ���� �Ǵ�
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged();
        }
    }

    //���� �ǰ� �� ����


    // 무적 + 넉백 처리
    void OnDamaged()
    {
        gameObject.layer = 11;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // 진행 방향 반대로 강제 넉백
        float xDirection;
        if (inputValue > 0.1f)
            xDirection = -1f;
        else if (inputValue < -0.1f)
            xDirection = 1f;
        else
            xDirection = spriteRenderer.flipX ? 1f : -1f; // 움직이지 않을 경우, 캐릭터 방향 기준 넉백

        Vector2 knockback = new Vector2(xDirection * knockbackPowerX, knockbackPowerY + 1f); // Y축에도 힘을 더 줌

        body.linearVelocity = Vector2.zero; // 반드시 속도 제거
        body.AddForce(knockback, ForceMode2D.Impulse);

        Invoke("OffDamaged", 1.5f);
    }

    //�Ϲ� ���·� ����
    void OffDamaged()
    {
        gameObject.layer = 10;

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
