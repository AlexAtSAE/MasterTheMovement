using UnityEngine;

public class TeleporterScript : MonoBehaviour
{
    [SerializeField] private Transform tpTo;

    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        other.gameObject.transform.position = tpTo.position;
    }
}
