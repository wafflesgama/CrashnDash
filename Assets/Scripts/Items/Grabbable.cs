using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class Grabbable : MonoBehaviour, Interactable
{
    public static UEvent<Transform> OnGrabbed = new UEvent<Transform>();
    public static UEvent OnReleased = new UEvent();
    public List<Renderer> renderers;
    public List<Renderer> GetInteractableMeshes() => renderers;

    [Button("Grab this")]
    public void Interact()
    {
        Debug.Log("Grabbing");
        OnGrabbed.TryInvoke(transform);
    }

}
