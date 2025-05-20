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

    public Animator enemyAnimator;

    void Start()
    {
        if (name.Equals("Enemy1"))
        {
            SetEnemyStatus("Enemy1", 100, 10, 1.5f, 2, 0.94f, 10f);
        }
    }

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
}
