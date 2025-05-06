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
        // LayerMask 검사: 충돌한 오브젝트의 Layer가 enemyLayer에 포함되는가?
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            var enemy = collision.GetComponent<Enemy_Type1>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        Debug.Log(" Bullet 충돌 발생! 충돌한 객체: " + collision.name);

        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Debug.Log(" Enemy Layer 충돌 감지!");

            Enemy_Type1 enemy = collision.GetComponent<Enemy_Type1>();
            if (enemy != null)
            {
                Debug.Log(" Enemy 컴포넌트 발견! 데미지 적용");
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

