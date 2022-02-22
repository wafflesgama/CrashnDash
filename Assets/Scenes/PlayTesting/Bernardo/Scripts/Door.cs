using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using DG.Tweening;
using static SoundUtils;
using System.Threading.Tasks;

public class Door : MonoBehaviour
{
    //public Animator doorLeft;
    //public Animator doorRight;
    public Sound openSound;
    public Sound closeSound;
    public AudioSource audioSource;
    public float openDistance = 4;

    public float autoCloseTime;
    public int freezeTimeMs;
    public float openSpeed = 1;
    public float closeSpeed = 1;

    public Ease openAnimEase;
    public Ease closeAnimEase;

    public Transform doorLeft;
    public Transform doorRight;

    float rightInitPos;
    float leftInitPos;

    Coroutine autoCloseRoutine;

    public bool isOpen { get; private set; }

    private bool canOperate = true;
    void Start()
    {
        rightInitPos = doorRight.localPosition.z;
        leftInitPos = doorLeft.localPosition.z;
        //doorLeft.SetBool("isLeft", true);
        //doorRight.SetBool("isLeft", false);
    }

    public async void OpenClose()
    {
        if (!canOperate) return;

        if (!isOpen)
            OpenDoor();
        else
            CloseDoor();

        canOperate = false;
        await Task.Delay(freezeTimeMs);
        canOperate = true;
    }

    [Button("Open Door")]
    public void OpenDoor()
    {
        isOpen = true;
        audioSource.PlaySound(openSound);
        doorLeft.DOLocalMoveZ(leftInitPos - openDistance, openSpeed).SetEase(openAnimEase);
        doorRight.DOLocalMoveZ(rightInitPos + openDistance, openSpeed).SetEase(openAnimEase);
        autoCloseRoutine = StartCoroutine(WaitForClose());
        //doorLeft.SetTrigger("OpenDoor");
        //doorRight.SetTrigger("OpenDoor");
    }

    [Button("Close Door")]
    public void CloseDoor()
    {
        isOpen = false;
        audioSource?.PlaySound(closeSound);
        doorLeft.DOLocalMoveZ(leftInitPos, closeSpeed).SetEase(closeAnimEase);
        doorRight.DOLocalMoveZ(rightInitPos, closeSpeed).SetEase(closeAnimEase);

        if (autoCloseRoutine != null)
            StopCoroutine(autoCloseRoutine);
        //doorLeft.SetTrigger("CloseDoor");
        //doorRight.SetTrigger("CloseDoor");
    }

    IEnumerator WaitForClose()
    {
        yield return new WaitForSeconds(autoCloseTime);
        CloseDoor();
        autoCloseRoutine = null;
    }

}
