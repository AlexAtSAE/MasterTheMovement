using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//InputBuffer stores inputs between frames for physics updates
public static class InputBuffer
{ 
    private static Dictionary<string, bool> InputsDown = new Dictionary<string, bool>();
    public static bool GetKeyDown(string input)
    {
        try
        {
            return InputsDown[input];
        }
        catch
        {
            return false;
        }
    }

    public static void SetKeyDown(string input, bool state)
    {
        if(InputsDown.ContainsKey(input))
            InputsDown[input] = state;
        else InputsDown.Add(input,state);
    }

    public static void Reset()
    {
        InputsDown.Clear();
    }
}


