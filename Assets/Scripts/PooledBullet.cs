using UnityEngine;

public class PooledBullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float lifeTime;
    public float bulletRadius;
    public ParticleSystem particleSystem;
    public LayerMask layerMask;
    public string BulletType;
    void Start()
    {
        
    }

    private float timeAlive;
    public void init()
    {
        timeAlive = 0;
    }
    public void Update()
    {
        if (!(timeAlive < lifeTime)) BulletManager.GetPool().Release(this);
        timeAlive += Time.deltaTime;
    }

        void FixedUpdate()
    {
        
            transform.position += transform.forward * speed;
            particleSystem.Play();
            RaycastHit ray;
            bool hit = Physics.SphereCast(transform.position, bulletRadius, transform.forward, out ray, 0, layerMask);
            if (hit)
            {
                HitObject(ray.collider.gameObject);
            }
    }
    public void HitObject(GameObject other)
    {
        Debug.Log($"HIT THE OTHJER : {other.name}");
    }
}
