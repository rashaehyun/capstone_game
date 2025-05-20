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
