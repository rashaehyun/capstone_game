using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 2);
    }

    void FixedUpdate()
    {
        //�̵�
        //����Ƽ ���� ���� ����
#pragma warning disable CS0618
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);
#pragma warning restore CS0618

        //�÷��� üũ
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        //����������� ���� ��ȯ
        if (rayHit.collider == null) {
            Turn();
        }
    }

    void Think()
    {
        //���� Ȱ��
        nextMove = 2 * Random.Range(-1, 2);

        //���� �ִϸ��̼�
        anim.SetInteger("RunSpeed", nextMove);
        
        //���� �̵� ����
        if (nextMove != 0) {
            spriteRenderer.flipX = nextMove >= 1;
        }
        /*
        if (nextMove != 0)
        {
            if (nextMove >= 1)
            {
                spriteRenderer.flipX = true;
            }
            else if (nextMove <= -1)
            {
                spriteRenderer.flipX = false;
            }
        }*/

        //���
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove >= 1;

        CancelInvoke();
        Invoke("Think", 2);
    }
}
