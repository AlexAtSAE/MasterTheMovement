using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDefiniton", menuName = "ScriptableObjects/WeaponDefinition")]
public class PlayerWeaponDefinition : ScriptableObject
{
    public GameObject weaponAsset;
    public GameObject projectile;
    public AudioClip fireSound;
    [SerializeField] public WeaponBehaviours baseBehaviour;
    public WeaponBehaviour Behaviour {
        get => GetWeaponBehaviour();
        private set { } 
    }

    private WeaponBehaviour GetWeaponBehaviour()
    {
        switch (baseBehaviour)
        {
            case WeaponBehaviours.Pistol    : return pistolBehaviour; 
            default                         : return defaultBehaviour;
        }
    }
    private PistolBehaviour pistolBehaviour;
    private WeaponBehaviour defaultBehaviour;
    public void Init()
    {
        pistolBehaviour = new PistolBehaviour(weaponAsset, fireSound, projectile);
        defaultBehaviour = new WeaponBehaviour(weaponAsset, fireSound, projectile);
    }




}

public enum WeaponBehaviours
{
    Pistol
}

[System.Serializable]
public class WeaponBehaviour
{
    protected GameObject asset;
    protected AudioClip sound;
    protected GameObject projectile;
    public WeaponBehaviour(GameObject asset, AudioClip sound, GameObject projectile)
    {
        this.asset = asset;
        this.sound = sound;
        this.projectile = projectile;
    }

    public virtual void Fire(object source) { }
}
[System.Serializable]
public class PistolBehaviour : WeaponBehaviour
{
    public PistolBehaviour(GameObject asset, AudioClip sound, GameObject projectile) : base(asset, sound, projectile){
        this.asset = asset;
        this.sound = sound;
        this.projectile = projectile;
    }

    public override void Fire(object source) {
        
        Transform fromTransform = null;
        
        if (source is PlayerWeaponSystem)
        {
            PlayerWeaponSystem pws = (PlayerWeaponSystem)source;
            fromTransform = pws.hand.transform;
        }
        if (fromTransform != null) {
            if (projectile == null) Debug.Log("WHY");
            GameObject bullet = GameObject.Instantiate(projectile, fromTransform);
            BulletManager.Instance.RegisterBullet(bullet);
        }
        
        Debug.Log("pow!");

    }
}