using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UEventHandler;

public class PlayerAimController : MonoBehaviour
{

    public static UEvent OnLockTarget = new UEvent();
    public static UEvent OnUnlockTarget = new UEvent();

    public PlayerInputManager playerInputManager;
    public Transform aimTarget;
    public Transform playerSource;

    public float xSensitivity = 1;
    public float ySensitivity = 1;

    UEventHandler eventHandler = new UEventHandler();
    bool isAimFrozen;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PauseHandler.OnPause.Subscribe(eventHandler, () => PauseHandle(pause: true));
        PauseHandler.OnUnpause.Subscribe(eventHandler, () => PauseHandle(pause: false));
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }



    void PauseHandle(bool pause)
    {
        isAimFrozen = pause;
        Cursor.visible = pause;
        Cursor.lockState = !pause ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        if (isAimFrozen) return;

        var newRotation = Quaternion.Euler(aimTarget.eulerAngles.x + (playerInputManager.input_look.value.y * ySensitivity * -.08f),
                                               aimTarget.eulerAngles.y + (playerInputManager.input_look.value.x * xSensitivity * .08f),
                                               aimTarget.eulerAngles.z);

        aimTarget.rotation = newRotation;
    }
}
