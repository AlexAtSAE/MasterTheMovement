using JetBrains.Annotations;
using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponSystem : MonoBehaviour
{
    public int selectedWeaponIndex;
    public GameObject hand;
    //0 is no weapon but cannot swap to 0

    [SerializeField] private PlayerWeaponDefinition[] weaponDefinitions;
    private PlayerWeaponDefinition CurrentDefinition;
    public void NextWeaponEvent(InputAction.CallbackContext context)
    {
        //int targetIndex = selectedWeaponIndex + 1;
        //selectedWeaponIndex = Math.Min(cycleWeaponMax, targetIndex);
        CurrentDefinition = weaponDefinitions[0];
    }
    public void PreviousWeaponEvent(InputAction.CallbackContext context)
    {
        //int attemptToSwapTo = selectedWeaponIndex+1;
        PlayerWeaponDefinition? weapon = GetWeapon(0);
        if (weapon != null)
        {
            weapon.Init();
            hand = weapon.weaponAsset;
        }


    }
    private PlayerWeaponDefinition? GetWeapon(int index)
    {
        int hasWeapon = PlayerPrefs.GetInt($"PlayerWeapon_{index}");
        if (hasWeapon == 1) {
            PlayerWeaponDefinition weaponDef;
            return weaponDefinitions[index];
        }
        return null;
    }
    bool holdingFire;
    public void ShootEvent(InputAction.CallbackContext context)
    {
        if (holdingFire == context.performed) return;
        holdingFire = context.performed;
        if (holdingFire == false) return;
        if(CurrentDefinition != null) CurrentDefinition.Behaviour.Fire(this);
    }

}
