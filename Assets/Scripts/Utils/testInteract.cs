using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteract : MonoBehaviour, Interactable
{
    public List<Renderer> renderers;
    public List<Renderer> GetInteractableMeshes() => renderers;

    public void Interact()
    {
        
    }

    
}
