using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private bool ResetPrefs = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(ResetPrefs) PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt($"PlayerWeapon_{WeaponType.Hand}", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }
}
