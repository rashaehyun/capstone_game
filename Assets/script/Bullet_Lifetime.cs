using UnityEngine;

public class Bullet_Liftetime : MonoBehaviour
{
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
