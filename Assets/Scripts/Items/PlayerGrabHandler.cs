using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabHandler : MonoBehaviour
{

    [SerializeField]
    private PlayerInputManager playerInputManager;
    [SerializeField]
    private float followLerp = 1;
    [SerializeField]
    private float followLerp2 = 1;

    [SerializeField]
    private Transform viewDir;
    //[SerializeField]
    //private Transform grabPoint;
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float distance = 2;

    [SerializeField]
    private float comeTime;

    private Vector3 comeSpeed;

    private Transform objectToGrab;
    private Rigidbody objectBody;
    private UEventHandler eventHandler = new UEventHandler();

    private Vector3 viewDirLerped;
    private Vector3 damptest;


    private Vector3 objectVelocity;



    void Start()
    {
        Grabbable.OnGrabbed.Subscribe(eventHandler, GrabObject);
        playerInputManager.input_attack.Onreleased.Subscribe(eventHandler, ReleaseObject);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void GrabObject(Transform obj)
    {
        objectToGrab = obj;
        if (objectToGrab.TryGetComponent<Rigidbody>(out objectBody))
            objectBody.isKinematic = true;
    }
    private void ReleaseObject()
    {
        if (objectToGrab == null) return;

        Grabbable.OnReleased.TryInvoke();

        if (objectBody != null)
        {
            objectBody.isKinematic = false;
            objectBody.velocity = objectVelocity / 1.5f;
        }

        objectBody = null;
        objectToGrab = null;
    }
    private void Update()
    {
        if (objectToGrab == null) return;

        objectToGrab.forward = viewDir.forward;

        //viewDirLerped = Vector3.LerpUnclamped(viewDirLerped, viewDir.forward.normalized, Time.deltaTime * followLerp);
        viewDirLerped = Vector3.SmoothDamp(viewDirLerped, viewDir.forward.normalized, ref damptest, followLerp);

        //var pos = viewDir.position + (Vector3.forward * distance);
        //var pos = viewDir.position + (viewDir.forward * distance);
        var pos = viewDir.position + (viewDirLerped * distance);

        objectVelocity = (pos - objectToGrab.position) / Time.smoothDeltaTime;

        //objectToGrab.position = Vector3.SmoothDamp(objectToGrab.position, pos, ref comeSpeed, comeTime);
        objectToGrab.position = pos;

        //objectToGrab.position = Vector3.Lerp(objectToGrab.position, pos, Time.smoothDeltaTime * followLerp2);
    }
}
