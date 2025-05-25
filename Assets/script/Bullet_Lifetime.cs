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
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
