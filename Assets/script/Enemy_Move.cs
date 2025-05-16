using System.Collections; //������ ���� �ӽ÷� �߰�
using UnityEngine;

public class Enemy_Move : MonoBehaviour
{
    public Transform target;
    float attackDelay;

    Enemy enemy;
    Animator enemyAnimator;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimator = enemy.enemyAnimator;
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
            else
            {
                //�ִϸ��̼��� Attack�� �ƴ� ���.
                //if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                //{
                MoveToTarget();
                //}
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
        if (target.position.x - transform.position.x > 0) // Ÿ���� �����ʿ� ���� ��
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else // Ÿ���� ���ʿ� ���� ��
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void AttackTarget()
    {
        //target.GetComponent<Sword_Man>().nowHp -= enemy.atkDmg; //ä�¹�
        //enemyAnimator.SetTrigger("attack"); // ���� �ִϸ��̼� ����
        StartCoroutine(JumpEffect());
        attackDelay = enemy.atkSpeed; // ������ ����
    }

    IEnumerator JumpEffect() //���� �ִϸ��̼� ��� �ӽ÷� ���� ���
    {
        Vector3 originPos = transform.position;
        float jumpHeight = 1f;
        float jumpSpeed = 5f;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = Vector3.Lerp(originPos, originPos + Vector3.up * jumpHeight, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * jumpSpeed;
            transform.position = Vector3.Lerp(originPos + Vector3.up * jumpHeight, originPos, t);
            yield return null;
        }
    }
}