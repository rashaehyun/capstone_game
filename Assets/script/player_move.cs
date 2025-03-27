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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ��������
    }

    private void FixedUpdate()
    {
        body.linearVelocityX = inputValue * speed;

        // ĳ���� ���� ��ȯ
        if (inputValue > 0)
            spriteRenderer.flipX = false; // ������ �̵� �� ���� ����
        else if (inputValue < 0)
            spriteRenderer.flipX = true;  // ���� �̵� �� �¿� ����
    }

    private void OnMove(InputValue value)
    {
        inputValue = value.Get<Vector2>().x;
    }

}
