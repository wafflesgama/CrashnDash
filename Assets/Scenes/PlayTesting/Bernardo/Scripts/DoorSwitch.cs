using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour, Interactable
{
    public List<Renderer> renderers;
    public List<Renderer> GetInteractableMeshes() => renderers;

    public Door door;

    public void Interact()
    {
        door.OpenClose();
    }
}
