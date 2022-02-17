using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UEventHandler;

public class PlayerGrabHandler : MonoBehaviour
{

    public UEvent<GameObject> OnObjectGrabbed= new UEvent<GameObject>();
    public UEvent OnObjectReleased= new UEvent();

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

    public  Transform grabbingObject;
    private Rigidbody objectBody;
    private UEventHandler eventHandler = new UEventHandler();

    private Vector3 viewDirLerped;
    private Vector3 damptest;


    private int objectInitLayer;
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
        OnObjectGrabbed.TryInvoke(obj.gameObject);
        grabbingObject = obj;
        if (grabbingObject.TryGetComponent<Rigidbody>(out objectBody))
            objectBody.isKinematic = true;

        objectInitLayer = grabbingObject.gameObject.layer;
        grabbingObject.gameObject.layer = 7;  //Set to grabbing layer
    }
    private void ReleaseObject()
    {
        if (grabbingObject == null) return;

        Grabbable.OnReleased.TryInvoke();

        grabbingObject.gameObject.layer = objectInitLayer;

        if (objectBody != null)
        {
            objectBody.isKinematic = false;
            objectBody.velocity = objectVelocity / 1.5f;
        }

        objectBody = null;
        grabbingObject = null;
    }

    //public List<string> GetGrabbingObjectQualifiers()
    //{
    //   var qualifiers= grabbingObject.GetComponents<Qualifier>();
    //    return qualifiers.Select(x=> nameof(x)).ToList();
    //}
    private void Update()
    {
        if (grabbingObject == null) return;

        grabbingObject.forward = viewDir.forward;

        //viewDirLerped = Vector3.LerpUnclamped(viewDirLerped, viewDir.forward.normalized, Time.deltaTime * followLerp);
        viewDirLerped = Vector3.SmoothDamp(viewDirLerped, viewDir.forward.normalized, ref damptest, followLerp);

        //var pos = viewDir.position + (Vector3.forward * distance);
        //var pos = viewDir.position + (viewDir.forward * distance);
        var pos = viewDir.position + (viewDirLerped * distance);

        objectVelocity = (pos - grabbingObject.position) / Time.smoothDeltaTime;

        //objectToGrab.position = Vector3.SmoothDamp(objectToGrab.position, pos, ref comeSpeed, comeTime);
        grabbingObject.position = pos;

        //objectToGrab.position = Vector3.Lerp(objectToGrab.position, pos, Time.smoothDeltaTime * followLerp2);
    }
}
