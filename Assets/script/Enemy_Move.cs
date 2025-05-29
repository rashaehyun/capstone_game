using System.Collections; //점프를 위해 임시로 추가
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    public Transform target;
    float attackDelay;
    float patternDelay;
    bool isAction = false;

    Enemy enemy;
    Animator enemyAnimator;

    enum Pattern { Move, Jump, Dash }
    Pattern cPattern = Pattern.Move;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimator = enemy.enemyAnimator;
        StartCoroutine(PatternChanger());
    }

    void Update()
    {
        attackDelay -= Time.deltaTime;
        if (attackDelay < 0) attackDelay = 0;

        float distance = Vector3.Distance(transform.position, target.position);

        if (attackDelay == 0 && distance <= enemy.fieldOfVision)
        {
            FaceTarget();

            if (distance <= enemy.atkRange)
            {
                AttackTarget();
            }
            else if (!isAction)
            {
                switch (cPattern)
                {
                    case Pattern.Move:
                        MoveToTarget();
                        break;
                    case Pattern.Jump:
                        StartCoroutine(JumpToTarget());
                        break;
                    case Pattern.Dash:
                        StartCoroutine(DashToTarget());
                        break;
                }
            }
        }
        else
        {
            enemyAnimator.SetBool("moving", false);
        }
    }

    void MoveToTarget()
    {
        float dir = target.position.x - transform.position.x;
        dir = (dir < 0) ? -1 : 1;
        transform.Translate(new Vector2(dir, 0) * enemy.moveSpeed * Time.deltaTime);
        enemyAnimator.SetBool("moving", true);
    }

    void FaceTarget()
    {
        if (target.position.x - transform.position.x < 0) // 타겟이 오른쪽에 있을 때
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else // 타겟이 왼쪽에 있을 때
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void AttackTarget()
    {
        //target.GetComponent<Sword_Man>().nowHp -= enemy.atkDmg; //채력바
        enemyAnimator.SetTrigger("attack"); // 공격 애니메이션 실행
        attackDelay = enemy.atkSpeed; // 딜레이 충전
    }

    IEnumerator PatternChanger()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            // 랜덤하게 행동 패턴 결정
            int rand = Random.Range(0, 3); // 0: Walk, 1: Jump, 2: Dash
            cPattern = (Pattern)rand;
        }
    }

    IEnumerator JumpToTarget()
    {
        isAction = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
        float jumpHeight = 2f;
        float duration = 0.5f;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;
            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * height;
            yield return null;
        }

        isAction = false;
    }

    IEnumerator DashToTarget()
    {
        isAction = true;
        float dir = target.position.x - transform.position.x < 0 ? -1 : 1;
        float dashSpeed = enemy.moveSpeed * 3;
        float dashDuration = 0.2f;

        float t = 0;
        while (t < dashDuration)
        {
            transform.Translate(Vector2.right * dir * dashSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        isAction = false;
    }
}