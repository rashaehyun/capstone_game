using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] float jumpPower = 5f;
    [SerializeField] int maxJumpCount = 2;

    private bool isGround = false;
    private int jumpCount = 0;
    private bool isJump = false;

    private Rigidbody2D body;
    private Animator anim;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
    }

    private void LateUpdate()
    {
        anim.SetBool("Ground", isGround);
        anim.SetBool("isJump", isJump);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = true;
            jumpCount = 0;
            isJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }

    
    public void OnJump()
    {
        if (jumpCount < maxJumpCount)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0f); 
            body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jumpCount++;
            isGround = false;

            isJump = true;
        }
        
    }
}
