using UnityEngine;

public class Bullet_Lifetime : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 1;

    [SerializeField] private LayerMask enemyLayer; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // LayerMask �˻�: �浹�� ������Ʈ�� Layer�� enemyLayer�� ���ԵǴ°�?
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            var enemy = collision.GetComponent<Enemy_Type1>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        Debug.Log(" Bullet �浹 �߻�! �浹�� ��ü: " + collision.name);

        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log(" Enemy Layer �浹 ����!");

            Enemy_Type1 enemy = collision.GetComponent<Enemy_Type1>();
            if (enemy != null)
            {
                Debug.Log(" Enemy ������Ʈ �߰�! ������ ����");
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

