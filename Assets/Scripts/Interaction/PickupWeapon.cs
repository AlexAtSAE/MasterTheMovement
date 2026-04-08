using UnityEngine;

public class PickupWeapon : MonoBehaviour, IInteractable
{
    public WeaponType unlockingWeapon; //PlayerWeapon_0 for pistol
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginInteract(object interactor)
    {
        Debug.Log("Begin interact");
        PlayerPrefs.SetInt($"PlayerWeapon_{unlockingWeapon}", 1);
        
    }
    public void EndInteract(object interactor)
    {
        Debug.Log("End Interact");
        Destroy(gameObject);
    }

}
