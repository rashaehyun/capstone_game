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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ��������
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
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

}
