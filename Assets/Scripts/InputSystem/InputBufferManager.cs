using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBufferManager : MonoBehaviour
{
    public static InputBufferManager instance;
    public InputMappingContext GroundIMC;
    InputMappingContext mappingContext;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        DontDestroyOnLoad(this);
        
    }

    public void Update()
    {
        if(mappingContext == null) return;
        foreach (var mapping in mappingContext.keyNameMapping)
        {
            if(Input.GetKey(mapping.Key)) InputBuffer.SetKeyDown(mapping.Name,true);
        }
    }

    public void FixedUpdate()
    {
        InputBuffer.Reset();
    }

    public static void SetMappingContext(InputMappingContext mappingContext)
    {
        Debug.Log("SetMappingContext");
        if (instance == null)
        {
            Debug.Log("instance is null");
            return;
        }
        foreach (var mapping in mappingContext.keyNameMapping)
            Debug.Log(mapping.Name);
        instance.mappingContext = mappingContext;
    }
}
