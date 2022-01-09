using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSimple : MonoBehaviour
{
    public Transform followTarget;
    public float followFactor = 1;
    public bool offsetEnabled = true;
    public bool followLerp=true;
    public bool followRotation;

    Vector3 offset;
    void Start()
    {
        offset = offsetEnabled ? transform.position - followTarget.transform.position : Vector3.zero;
    }

    private void LateUpdate()
    {
        if(followLerp)
        transform.position = Vector3.Lerp(transform.position, followTarget.position + offset, Time.deltaTime * followFactor);
        else
         transform.position = followTarget.position + offset;

        if(followRotation)
        transform.rotation=followTarget.rotation;
    }
}
