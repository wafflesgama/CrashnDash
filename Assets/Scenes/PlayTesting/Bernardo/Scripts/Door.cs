using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;


public class Door : MonoBehaviour
{
    public Animator doorLeft;
    public Animator doorRight;

    // Start is called before the first frame update
    void Start()
    {
        doorLeft.SetBool("isLeft", true);
        doorRight.SetBool("isLeft", false);
    }
    
    [Button("Open Door")]
    public void openDoor ()
    {
        doorLeft.SetTrigger("OpenDoor");
        doorRight.SetTrigger("OpenDoor");
    }
    
    [Button("Close Door")]
    public void closeDoor ()
    {
        doorLeft.SetTrigger("CloseDoor");
        doorRight.SetTrigger("CloseDoor");
    }

}
