using UnityEngine;

public class JumpPadScript : MonoBehaviour
{
    [SerializeField] private float BOING;
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if(rb != null)
        {
            rb.linearVelocity = rb.linearVelocity + Vector3.up * BOING;
        }
    }
}
