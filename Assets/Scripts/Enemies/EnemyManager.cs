using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
   public static EnemyManager Instance;
    void Awake()
    {
        Instance = this;
        ObjectPool<PooledEnemy> objectPool = new ObjectPool<PooledEnemy>
            (
            createFunc: CreateItem,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroyItem,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
            );
    }
    PooledEnemy CreateItem()
    {
        PooledEnemy enemy = new PooledEnemy();
        
        return null;
    }
    void OnGet(PooledEnemy gameObject)
    {

    }
    void OnRelease(PooledEnemy gameObject)
    {
    }
    void OnDestroyItem(PooledEnemy gameObject)
    {
        
    }
}

public class PooledEnemy
{
    public GameObject gameObject { get; private set; }
    public PooledEnemy()
    {
        //EnemyManager.Instance;
        //gameObject = GameObject.Instantiate();
    }
}
