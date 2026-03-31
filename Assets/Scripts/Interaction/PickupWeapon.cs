using UnityEngine;

public class PickupWeapon : MonoBehaviour, IInteractable
{
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
    }
    public void EndInteract(object interactor)
    {
        Debug.Log("End Interact");
    }

}
