using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;    //���� �̸�
    public int maxHp;           //�ִ� ü��
    public int nowHp;           //���� ü��
    public int atkDmg;          //���� ������
    public float atkSpeed;      //���� �ӵ�
    public float moveSpeed;     //�̵� �ӵ�
    public float atkRange;      //���� ����
    public float fieldOfVision; //���� �þ�
    public bool isDead = false; //���� ��� ����

    void SetEnemyStatus(string _enemyName, int _maxHp, int _atkDmg, float _atkSpeed, float _moveSpeed, float _atkRange, float _fieldOfVision)
    {
        enemyName = _enemyName;
        maxHp = _maxHp;
        nowHp = _maxHp;
        atkDmg = _atkDmg;
        atkSpeed = _atkSpeed;
        moveSpeed = _moveSpeed;
        atkRange = _atkRange;
        fieldOfVision = _fieldOfVision;
    }

    public Animator enemyAnimator;

    void Start()
    {
        if (name.Equals("Boss"))
        {
            SetEnemyStatus("Boss", 5, 5, 1.5f, 4, 3.0f, 20f);
        }
        else if (name.StartsWith("Enemy"))
        {
            SetEnemyStatus("Enemy", 5, 5, 1.5f, 4, 2.0f, 20f);
        }

        SetAttackSpeed(atkSpeed);
    }

    public void TakeDamage(int dmg)
    {
        nowHp -= dmg;

        if (nowHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        enemyAnimator.SetTrigger("die");

        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());

        Destroy(gameObject, 3);
    }

    void SetAttackSpeed(float speed)
    {
        enemyAnimator.SetFloat("attackSpeed", speed);
    }
}
