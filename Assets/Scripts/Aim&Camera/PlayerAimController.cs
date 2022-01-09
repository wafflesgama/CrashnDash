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
    public float dampTime;

    public int test = 0;

    private float dampVelocity;
    private Vector3 dampVelocity3;

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

        if (test == 1)
        {

            float delta = Quaternion.Angle(transform.rotation, newRotation);
            if (delta > 0f)
            {
                float t = Mathf.SmoothDampAngle(delta, 0.0f, ref dampVelocity, dampTime * Time.deltaTime);
                t = 1.0f - (t / delta);
                aimTarget.rotation = Quaternion.Slerp(aimTarget.rotation, newRotation, t);
            }
        }
            else if (test == 2)
            {

                aimTarget.rotation = Quaternion.Euler(Vector3.SmoothDamp(aimTarget.rotation.eulerAngles, newRotation.eulerAngles, ref dampVelocity3, dampTime * Time.deltaTime));

            }
            else
            {

                aimTarget.rotation = newRotation;
            }
    }
}
