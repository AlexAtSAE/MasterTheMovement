using JetBrains.Annotations;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("PlayerInfo")]
    public GameObject hand;
    public WeaponType currentWeapon = WeaponType.Hand;
    [Space(10)]
    [Header("Weapon Info")]
    public WeaponAssetReferences weaponAssetReferences;



    public static PlayerWeaponSystem instance;
    private WeaponSystem.PrimaryShootDelegate primaryShootDelegate;
    private void Awake()
    {
        instance = this;
    }
    public void NextWeaponEvent(InputAction.CallbackContext context)
    {
        Debug.Log("Finding next weapon");
        //Find next weapon "${PlayerWeapon_{weapon}"
        
        int currentWeaponNumber = (int)currentWeapon;
        foreach(WeaponType weapontype in WeaponType.GetValues(typeof(WeaponType)))
        {
            bool hasWeapon = PlayerPrefs.GetInt($"PlayerWeapon_{weapontype}",0) == 1;
            if (hasWeapon && weapontype != currentWeapon)
            {
                EquipWeapon(GetWeaponInfo(weapontype));
                Debug.Log("Equipped: " + weapontype);
                return;
            }
        }
        Debug.Log("Equipped hand ");
        EquipWeapon(GetWeaponInfo(WeaponType.Hand));
    }
    public void PreviousWeaponEvent(InputAction.CallbackContext context)
    {


    }
    private void EquipWeapon(WeaponAssetInfo weaponInfo)
    {
        currentWeapon = weaponInfo.type;
        primaryShootDelegate = WeaponSystem.GetPrimaryShoot(currentWeapon);
        /*GameObject child = hand.GetComponentInChildren<Transform>().gameObject;
        if (child != null)  Destroy(child);
        if (weaponInfo.WeaponPrefab != null)
        {
            GameObject weaponInHand = Instantiate(weaponInfo.WeaponPrefab);
            weaponInHand.transform.parent = hand.transform;
        }*/


    }

    bool holdingFire;
    public void ShootEvent(InputAction.CallbackContext context)
    {
        if (holdingFire == context.performed) return;
        holdingFire = context.performed;
        if (holdingFire == false) return;
        if(primaryShootDelegate != null)
        {
            primaryShootDelegate(this);
        }
       
    }


    public static WeaponAssetInfo GetWeaponInfo(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Hand: return instance.weaponAssetReferences.HandInfo;
            case WeaponType.Pistol: return instance.weaponAssetReferences.PistolInfo;
            case WeaponType.Rifle:  return instance.weaponAssetReferences.RifleInfo;
            case WeaponType.RocketLauncher: return instance.weaponAssetReferences.RocketLauncherInfo;
            default: return new WeaponAssetInfo();
        }
    }
}


