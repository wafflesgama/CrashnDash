using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;

public class CameraController : MonoBehaviour
{
    public enum ViewType
    {
        Unset,
        MainView,
        DialogueView
    }
    public Camera cameraBrain;
    public CinemachineVirtualCamera mainViewCamera;
    public CinemachineVirtualCamera dialogueCamera;

    Transform dialogueTarget;
    ViewType prevView = ViewType.Unset, currentView = ViewType.Unset;
    UEventHandler eventHandler = new UEventHandler();

    void Start()
    {
        SwitchView(ViewType.MainView);

        PlayerUIManager.OnStartedDialogue.Subscribe(eventHandler, StartDialogueCamera);
        PlayerUIManager.OnFinishedDialogue.Subscribe(eventHandler, EndDialogueCamera);
        PlayerCutsceneManager.OnEndingStarted.Subscribe(eventHandler, DisableCameras);
    }
    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    void Update()
    {

    }

    public async void DisableCameras()
    {
        await Task.Delay(1000);
        cameraBrain.gameObject.SetActive(false);
    }

    public void SwitchView(ViewType viewType)
    {
        if (prevView == ViewType.Unset)
            prevView = viewType;
        else
            prevView = currentView;

        currentView = viewType;

        mainViewCamera.Priority = 10;
        dialogueCamera.Priority = 10;

        switch (viewType)
        {
            case ViewType.DialogueView:
                dialogueCamera.Priority = 11;
                break;
            default:
                mainViewCamera.Priority = 11;
                break;
        }

    }
    public void SwitchToPrevView() => SwitchView(prevView);

    private void StartDialogueCamera(Transform t, string[] m)
    {
        dialogueTarget = t;
        dialogueCamera.LookAt = dialogueTarget;
        dialogueCamera.Follow = dialogueTarget;
        SwitchView(ViewType.DialogueView);
    }

    private void EndDialogueCamera()
    {
        SwitchToPrevView();
    }


}
