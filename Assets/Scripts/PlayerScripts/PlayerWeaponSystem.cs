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
    public Transform shootDirection;
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
        int amountOfWeapons = WeaponType.GetValues(typeof(WeaponType)).Length;
        currentWeaponNumber = (currentWeaponNumber + 1) % amountOfWeapons;
        currentWeapon = (WeaponType)currentWeaponNumber;
        EquipWeapon(GetWeaponInfo(currentWeapon));
        Debug.Log("Equipped: " + currentWeapon);
        
        /*foreach (WeaponType weapontype in WeaponType.GetValues(typeof(WeaponType)))
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
        EquipWeapon(GetWeaponInfo(WeaponType.Hand));*/
    }
    public void PreviousWeaponEvent(InputAction.CallbackContext context)
    {
        int currentWeaponNumber = (int)currentWeapon;
        int amountOfWeapons = WeaponType.GetValues(typeof(WeaponType)).Length;
        currentWeaponNumber = (currentWeaponNumber - 1) % amountOfWeapons;
        currentWeapon = (WeaponType)currentWeaponNumber;
        EquipWeapon(GetWeaponInfo(currentWeapon));
        Debug.Log("Equipped: " + currentWeapon);

    }
    private void EquipWeapon(WeaponAssetInfo weaponInfo)
    {
        currentWeapon = weaponInfo.type;
        primaryShootDelegate = WeaponSystem.GetPrimaryShoot(currentWeapon);
        /*Transform[] handChildren = hand.GetComponentsInChildren<Transform>();
        foreach (Transform child in handChildren) {
            if (child.name != weaponInfo.name) child.gameObject.SetActive(false);
            else child.gameObject.SetActive(true);
        }*/

        int handChildCount = hand.transform.childCount;
        Transform[] children = new Transform[handChildCount];
        for (int i = 0; i < handChildCount; i++)
        {
            Transform child = hand.transform.GetChild(i);
            if (child.name != weaponInfo.name) child.gameObject.SetActive(false);
            else child.gameObject.SetActive(true);
        }

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
            default: return instance.weaponAssetReferences.HandInfo;
        }
    }
}


