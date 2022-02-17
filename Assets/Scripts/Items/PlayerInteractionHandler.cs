using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class PlayerInteractionHandler : MonoBehaviour
{
    public static UEvent<Interactable, Transform> OnInteractableAppeared = new UEvent<Interactable, Transform>();
    public static UEvent OnInteractableDisappeared = new UEvent();
    public static UEvent<Vector3> OnSplash = new UEvent<Vector3>();
    static GameObject objectToInteract = null;

    public PlayerAimController aimController;
    public PlayerInputManager inputManager;
    public float castWidth = .5f;
    public float castLength = 5;
    public LayerMask castMask;

    public  bool isInteracting;

    private UEventHandler eventHandler = new UEventHandler();

    private void Start()
    {
        inputManager.input_attack.Onpressed.Subscribe(eventHandler, TryInteract);
        Grabbable.OnGrabbed.Subscribe(eventHandler, (x) => isInteracting = true);
        Grabbable.OnReleased.Subscribe(eventHandler, () => isInteracting = false);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }
    //public static bool IsInteractableNearby() => objectToInteract != null;


    

    private void FixedUpdate()
    {
        if (isInteracting)
        {
            objectToInteract = null;
            return;
        }

        var hasHit = Physics.SphereCast(aimController.aimTarget.position, castWidth, aimController.aimTarget.forward, out RaycastHit hit, castLength, castMask);

        if (!hasHit)
        {
            if(objectToInteract != null)
                OnInteractableDisappeared.TryInvoke();

            objectToInteract = null;
            return;
        };
        //if (hit.transform.parent == null) return;

        if (hit.transform.tag == "Interactable" && hit.transform.TryGetComponent<Interactable>(out Interactable interactable))
        {
            if (objectToInteract == null || objectToInteract.transform.GetInstanceID() != hit.transform.GetInstanceID())
            {
                if (objectToInteract != null)                           //It was a dif object so must first make disappear the last one 
                    OnInteractableDisappeared.TryInvoke();

                //if ()
                {
                    objectToInteract = hit.transform.gameObject;
                    OnInteractableAppeared.TryInvoke(interactable, hit.transform);
                }
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.parent == null) return;


    //    if (other.transform.parent.tag == "Interactable")
    //    {
    //        if (objectToInteract == null || objectToInteract.transform.position != other.transform.position)
    //        {
    //            objectToInteract = other.gameObject;
    //            if (objectToInteract.transform.parent.TryGetComponent<Interactable>(out Interactable interactable))
    //                OnInteractableAppeared.TryInvoke(interactable,other.transform, interactable.GetOffset());
    //        }
    //    }
    //    else if (other.transform.parent.tag == "KillBound")
    //    {
    //        LevelManager.currentManager.ResetPlayer();
    //    }

    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.transform.parent == null) return;

    //    if (other.transform.parent.tag == "Interactable")
    //    {
    //        if (objectToInteract != null && objectToInteract.transform.position == other.transform.position)
    //        {
    //            objectToInteract = null;
    //            OnInteractableDisappeared.TryInvoke();
    //        }
    //    }
    //}

    public void TryInteract()
    {
        if (objectToInteract == null || objectToInteract.transform.tag != "Interactable") return;
        //if (PlayerCutsceneManager.isInCutscene) return;

        objectToInteract.transform.GetComponent<Interactable>().Interact();
        OnInteractableDisappeared.TryInvoke();
    }

}
