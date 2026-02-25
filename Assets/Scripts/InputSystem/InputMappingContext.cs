using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Input Mapping Context", menuName = "InputSystem/InputMappingContext", order = 1)]
public class InputMappingContext : ScriptableObject
{
    [SerializeField] public KeyNameMapping[] keyNameMapping;
}

[System.Serializable] 
public class KeyNameMapping
{
    public KeyCode Key;
    public string Name;

    public KeyNameMapping(KeyCode key, string name)
    {
        Key = key;
        Name = name;
    }
}

