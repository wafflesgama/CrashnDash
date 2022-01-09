using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;


public class Door : MonoBehaviour
{
    public Animator doorLeft;
    public Animator doorRight;


    bool open = false;
    void Start()
    {
        doorLeft.SetBool("isLeft", true);
        doorRight.SetBool("isLeft", false);
    }

    public void OpenClose()
    {
        open = !open;

        if (open)
            OpenDoor();
        else
            CloseDoor();
    }

    [Button("Open Door")]
    public void OpenDoor()
    {
        doorLeft.SetTrigger("OpenDoor");
        doorRight.SetTrigger("OpenDoor");
    }

    [Button("Close Door")]
    public void CloseDoor()
    {
        doorLeft.SetTrigger("CloseDoor");
        doorRight.SetTrigger("CloseDoor");
    }

}
