using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractScript : MonoBehaviour
{
    public Transform raycastFrom;
    public float interactRange;
    public bool InteractInput { get; private set; }
    public void InteractEvent(InputAction.CallbackContext context)
    {
        InteractInput = context.performed;
        InteractUpdate(InteractInput);
        
    }
    private void Update()
    {
        Debug.DrawRay(raycastFrom.position, raycastFrom.forward* interactRange, Color.blue, 0.1f);
        /*if (!InteractInput && interactingWith != null) {
            interactingWith.EndInteract(this);
            interactingWith = null;
            interactingWithObject = null;
            return;
        }*/
        if(interactingWithObject != null && interactingWith != null)
        {
            
            RaycastHit hit;
            bool raycastHit = Physics.Raycast(raycastFrom.position, (interactingWithObject.transform.position - raycastFrom.position).normalized, out hit, interactRange);
            if (!raycastHit)
            {
                interactingWith.EndInteract(this);
                interactingWithObject = null;
                interactingWith = null;
            }
        }
        

    }
    GameObject interactingWithObject;
    IInteractable interactingWith;
    private void InteractUpdate(bool value)
    {
        if (!value) return;
        
        RaycastHit hit;
        bool raycastHit = Physics.Raycast(raycastFrom.position, raycastFrom.forward, out hit, interactRange);
        if(raycastHit)
        {
            interactingWithObject =     hit.collider.gameObject;
            interactingWith = interactingWithObject.GetComponent<IInteractable>();
            interactingWith.BeginInteract(this);
        }

        
        
    }

    
}
