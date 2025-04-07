using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    [SerializeField] private float speed;
    private float inputValue;

    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 추가
    private Animator anim;
    private PlayerDash dash;
    public float InputX => inputValue;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 가져오기
        anim = GetComponent<Animator>();
        dash = GetComponent<PlayerDash>();
    }

    private void FixedUpdate()
    {

        if (dash != null && dash.IsDashing)
            return;

        body.linearVelocityX = inputValue * speed;

        // 캐릭터 방향 전환
        if (inputValue > 0)
        { 
            spriteRenderer.flipX = false; // 오른쪽 이동 시 원래 방향
        }
        else if (inputValue < 0)
        { 
            spriteRenderer.flipX = true;  // 왼쪽 이동 시 좌우 반전
        }

        bool isRunning = Mathf.Abs(inputValue) > 0.01f;
        anim.SetBool("isRun", isRunning);
    }

    private void OnMove(InputValue value)
    {
        inputValue = value.Get<Vector2>().x;
    }

}
