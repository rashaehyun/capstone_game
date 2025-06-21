using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;    //몬스터 이름
    public int maxHp;           //최대 체력
    public int nowHp;           //현재 체력
    public int atkDmg;          //공격 데미지
    public float atkSpeed;      //공격 속도
    public float moveSpeed;     //이동 속도
    public float atkRange;      //공격 범위
    public float fieldOfVision; //몬스터 시야
    public bool isDead = false; //몬스터 사망 여부

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
