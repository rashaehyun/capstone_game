using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    [SerializeField] private float speed;
    private float inputValue;

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
            OnDamaged(collision.transform.position);
        }
    }
    
    //���� �ǰ� �� ����
    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        body.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse); //���� ������ ƨ�ܳ��� �Ÿ� ����

        //�����ð� 1.5�� ���� �Ϲ� ���·� ����
        Invoke("OffDamaged", 1.5f);
    }

    //�Ϲ� ���·� ����
    void OffDamaged()
    {
        gameObject.layer = 10;

        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
