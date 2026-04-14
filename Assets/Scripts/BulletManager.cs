using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public class BulletManager : MonoBehaviour
{
    public static BulletManager Instance;
    [SerializeField] private GameObject bulletPrefab;

    private ObjectPool<PooledBullet> BulletPool;

    private void Awake()
    {
        Instance = this;
        BulletPool = new ObjectPool<PooledBullet>(
            createFunc: createBullet,
            actionOnGet: OnGet,
            actionOnDestroy: OnDestroyItem,
            actionOnRelease: OnRelease,
            defaultCapacity:20,
            maxSize:100
            );
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static ObjectPool<PooledBullet> GetPool() => Instance.BulletPool;

    private PooledBullet createBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.SetActive(false);
        return bullet.GetComponent<PooledBullet>();
    }
    private void OnDestroyItem(PooledBullet bullet)
    {
        Destroy(bullet);
    }
    private void OnRelease(PooledBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void OnGet(PooledBullet bullet)
    {
        bullet.init();
        bullet.gameObject.SetActive(true);
    }

}
