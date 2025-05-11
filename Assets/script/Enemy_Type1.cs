using System.Collections;
using UnityEngine;

public class Enemy_Type1 : MonoBehaviour
{
    //컴포넌트 참조
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;

    //플레이어 추적 및 공격 관련
    public Transform target;                  // 추적 대상 (플레이어)
    public float moveSpeed = 1f;              // 이동 속도
    public float fieldOfVision = 10f;          // 플레이어 감지 범위
    public float atkRange = 1.5f;               // 공격 범위
    //public float atkDmg = 10f;                // 공격 데미지
    public float atkSpeed = 3f;             // 공격 속도
    float attackDelay = 0f;                   // 공격 대기 시간 타이머

    //상태 관리
    public int nextMove;                      // 순찰 방향 (-1, 0, 1)
    float lostTargetTime = 0f;                // 감지 후 공격 못 한 시간
    float chargeThreshold = 5f;        // 돌진 조건(공격 범위 밖에서 있는 시간)

    bool isCharging = false;
    float chargeTime = 0.5f; // 돌진 지속 시간
    float chargeTimer = 0f;

    //몬스터 체력
    public int maxHp = 5;
    private int currentHp;

    private bool hasSpottedPlayer = false;
    private float spottedTimer = 0f;
    [SerializeField] private float delayBeforeCharge = 2f;
    [SerializeField] private float patrolSpeed = 1.5f; // 순찰 속도

    private float lastTurnTime = 0f;
    [SerializeField] private float turnCooldown = 1f;  // 회전 쿨타임

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Invoke("Think", 2);
        
    }

    private void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;

        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (isCharging)
        {
            ChargeToTarget();
            chargeTimer -= Time.deltaTime;
            if (chargeTimer <= 0f)
            {
                isCharging = false;
            }
            return;
        }
        if (distance <= fieldOfVision)
        {
            FaceTarget();

            if (!hasSpottedPlayer)
            {
                hasSpottedPlayer = true;
                spottedTimer = 0f;  // 처음 인식 시 타이머 초기화
            }

            spottedTimer += Time.deltaTime;

            if (!isCharging && spottedTimer >= delayBeforeCharge)
            {
                isCharging = true;
                chargeTimer = chargeTime;
                spottedTimer = 0f;
            }

            if (!isCharging)
            {
                MoveToTarget(); // ✅ 돌진 중이 아닐 때만 이동
            }

            // 공격
            if (distance <= atkRange && attackDelay <= 0)
            {
                AttackTarget();
            }

            return; // 여기서는 돌진 중이거나 플레이어 추적 상태일 때만 빠짐
        }
        else
        {
            // 범위 밖이면 상태 초기화
            hasSpottedPlayer = false;
            spottedTimer = 0f;
            isCharging = false;

            if (!IsInvoking("Think"))
            {
                Invoke("Think", 2f);
            }

            anim.SetBool("moving", false);
        }
    }


    void FixedUpdate()
    {
        // 순찰 중일 때만 적용
        if (target == null || Vector2.Distance(transform.position, target.position) > fieldOfVision)
        {
            rigid.linearVelocity = new Vector2(nextMove * patrolSpeed, rigid.linearVelocity.y);

            // ✅ 순찰 방향에 따라 스프라이트 방향 갱신
            if (nextMove != 0)
                spriteRenderer.flipX = nextMove == 1;

            // 플랫폼 체크
            Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.4f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider == null && Time.time - lastTurnTime >= turnCooldown)
            {
                Turn();
            }
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);
        anim.SetInteger("RunSpeed", nextMove);

        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == 1;

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

        if (!IsInvoking("Think"))
        {
            Invoke("Think", Random.Range(2f, 5f));
        }
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        lastTurnTime = Time.time;

        CancelInvoke();
        Invoke("Think", 2);
    }

    void MoveToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;

        transform.Translate(new Vector2(dir, 0) * moveSpeed * Time.deltaTime);
        anim.SetBool("moving", true);
    }

    void ChargeToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;

        transform.Translate(new Vector2(dir * 2, 0) * moveSpeed * 4 * Time.deltaTime);
        //anim.SetTrigger("charge"); // 돌진 애니메이션 (추가 가능)
        lostTargetTime = 0f; // 돌진 후 시간 초기화
    }

    void FaceTarget()
    {
        float diff = target.position.x - transform.position.x;
        spriteRenderer.flipX = diff > 0;
    }

    void AttackTarget()
    {
        // 데미지 처리
        //target.GetComponent<Sword_Man>().nowHp -= atkDmg;

        // 임시 시각 효과
        StartCoroutine(JumpEffect());

        // 공격 딜레이
        attackDelay = atkSpeed;
    }

    IEnumerator JumpEffect()
    {
        Vector3 originPos = transform.position;
        float jumpHeight = 1f;
        float jumpSpeed = 5f;

        // 올라가기
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = Vector3.Lerp(originPos, originPos + Vector3.up * jumpHeight, t);
            yield return null;
        }

        // 내려오기
        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = Vector3.Lerp(originPos + Vector3.up * jumpHeight, originPos, t);
            yield return null;
        }
    }

    //피격 판정 및 체력
    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        Debug.Log("Enemy hit! HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    //몬스터 죽음
    void Die()
    {
        // 애니메이션, 파티클, 점수 증가 등 추가 가능
        Destroy(gameObject);
    }

}
