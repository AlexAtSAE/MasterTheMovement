using TMPro;
using UnityEngine;

public class PlayerSpeedDisplayer : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    Rigidbody rb;
    PlayerController pms;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pms = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textMeshPro.SetText($"Velocity: {rb.linearVelocity} \n Speed: {rb.linearVelocity.magnitude} \n State: {pms.currentState.Name}");
    }
}
