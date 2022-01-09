using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public List<Renderer> GetInteractableMeshes();
    public void Interact();
}
