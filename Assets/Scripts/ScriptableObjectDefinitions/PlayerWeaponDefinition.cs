using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    Hand,
    Pistol,
    Rifle,
    RocketLauncher
}

[CreateAssetMenu(fileName = "WeaponAssetInfo", menuName = "ScriptableObjects/Weapons/WeaponAssetInfo", order = 1)]
public class WeaponAssetInfo : ScriptableObject
{
    public WeaponType type;
    public string Name;
    public string Description;
    public GameObject BulletPrefab;
    // sound
    public GameObject WeaponPrefab;
}

[CreateAssetMenu(fileName = "WeaponAssetReferences", menuName = "ScriptableObjects/Weapons/WeaponAssetReferences", order = 1)]
public class WeaponAssetReferences : ScriptableObject
{
    public WeaponAssetInfo HandInfo;
    public WeaponAssetInfo PistolInfo;
    public WeaponAssetInfo RifleInfo;
    public WeaponAssetInfo RocketLauncherInfo;
}

public static class WeaponSystem {
    public delegate void PrimaryShootDelegate(PlayerWeaponSystem pws);
    public static PrimaryShootDelegate GetPrimaryShoot(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Hand:
                return HandShoot;
            case WeaponType.Pistol:
                return PistolShoot;
            case WeaponType.Rifle:
                return RifleShoot;
            case WeaponType.RocketLauncher:
                return RocketLauncherShoot;
            default: return (PlayerWeaponSystem pws) =>{ };
        }
    }

    private static void HandShoot(PlayerWeaponSystem pws)
    {
        Debug.Log("hand shoot");
    }
    private static void PistolShoot(PlayerWeaponSystem pws) 
    {
        Debug.Log("pistol shoot");
        WeaponAssetInfo info = pws.weaponAssetReferences.PistolInfo;
        PooledBullet newBullet = BulletManager.GetPool().Get();
        newBullet.transform.position = pws.hand.transform.position;
        newBullet.transform.rotation = pws.shootDirection.transform.rotation;
        
        Debug.DrawRay(pws.hand.transform.position, pws.transform.forward*50.0f, Color.red, 0.5f);
    }
    private static void RifleShoot(PlayerWeaponSystem pws)
    {

    }
    private static void RocketLauncherShoot(PlayerWeaponSystem pws)
    {
        
    }
}