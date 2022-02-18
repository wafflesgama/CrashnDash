using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using DG.Tweening;

public class Door : MonoBehaviour
{
    //public Animator doorLeft;
    //public Animator doorRight;
    public float openDistance = 4;

    public float openSpeed = 1;
    public float closeSpeed = 1;

    public Ease openAnimEase;
    public Ease closeAnimEase;

    public Transform doorLeft;
    public Transform doorRight;

    float rightInitPos;
    float leftInitPos;

    bool open = false;
    void Start()
    {
        rightInitPos = doorRight.localPosition.z;
        leftInitPos = doorLeft.localPosition.z;
        //doorLeft.SetBool("isLeft", true);
        //doorRight.SetBool("isLeft", false);
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
        doorLeft.DOLocalMoveZ(leftInitPos - openDistance, openSpeed).SetEase(openAnimEase);
        doorRight.DOLocalMoveZ(rightInitPos + openDistance, openSpeed).SetEase(openAnimEase);
        //doorLeft.SetTrigger("OpenDoor");
        //doorRight.SetTrigger("OpenDoor");
    }

    [Button("Close Door")]
    public void CloseDoor()
    {
        doorLeft.DOLocalMoveZ(leftInitPos, closeSpeed).SetEase(closeAnimEase);
        doorRight.DOLocalMoveZ(rightInitPos, closeSpeed).SetEase(closeAnimEase);
        //doorLeft.SetTrigger("CloseDoor");
        //doorRight.SetTrigger("CloseDoor");
    }

}
