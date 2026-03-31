using System;
using UnityEngine;

public interface IInteractable
{
    public void BeginInteract(object Interactor);
    public void EndInteract(object Interactor);
    
}
