using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using System;
using static UEventHandler;
using Sirenix.OdinInspector;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager currentManager;

    public static UEvent<Transform, string[]> OnStartedDialogue = new UEvent<Transform, string[]>();
    public static UEvent OnFinishedDialogue = new UEvent();
    public static UEvent<bool> OnFadeScreen = new UEvent<bool>();

    public PlayerAimController aimController;
    public PlayerInputManager inputManager;

    [Header("Dialogue")]
    public DialogueWriter dialogueWriter;
    public RectTransform dialogueContainer;
    public float dialogueAnimDuration = .5f;
    public Ease dialogueInEase;
    public Ease dialogueOutEase;
    public Image continueIcon;

    [Header("Fade")]
    public Animator fadeAnimator;

    [Header("Interactable")]
    public TextMeshProUGUI interactableText;
    public float showInteractAnimDuration = .5f;
    public Ease showInteractEase;


    [Header("Pause Menu")]
    public CanvasGroup pauseGroup;
    public UnityEngine.EventSystems.EventSystem eventSystem;
    public float pauseAnimDuration = .15f;

    [Header("Mentor Dialgoue")]
    public DialogueWriter mentorDialogueWriter;
    public RectTransform mentorDialogueContainer;
    public float mentorDialogueAnimDuration = .5f;
    public Ease mentorDialogueInEase;
    public Ease mentorDialogueOutEase;


    [Header("Credits")]
    public DialogueWriter creditsWriter;
    [Multiline]
    public string[] creditsMessage;
    public CanvasGroup creditsGroup;

    public UEventHandler eventHandler = new UEventHandler();
    int lockArrowShowing = -1;
    bool isInCrewDialogue;

    bool trackInteractable;
    Transform interactableRef;
    Vector3 interactableOffset;
    void Start()
    {
        fadeAnimator.gameObject.SetActive(true);
        OnFadeScreen.TryInvoke(true);

        currentManager = this;

        OnStartedDialogue.Subscribe(eventHandler, RegisterDialogue);
        inputManager.input_attack.Onpressed.Subscribe(eventHandler, TryNextCrewMessage);

        LevelManager.OnExitScreen.Subscribe(eventHandler, () => FadeScreen(fadeIn: false));
        LevelManager.OnRestartLevel.Subscribe(eventHandler, () => FadeInOutScreen());

        PauseHandler.OnPause.Subscribe(eventHandler, () => FadePauseMenu(fadeIn: true));
        PauseHandler.OnUnpause.Subscribe(eventHandler, () => FadePauseMenu(fadeIn: false));

        PlayerCutsceneManager.OnEndingStarted.Subscribe(eventHandler, () => FadeScreen(fadeIn: false));
        PlayerCutsceneManager.OnEndingFadeIn.Subscribe(eventHandler, () => FadeScreen(fadeIn: true));
        PlayerCutsceneManager.OnCreditsStarted.Subscribe(eventHandler, ShowCredits);

        DisplayMentorMessage();

    }


    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }



    void Update()
    {

        if (trackInteractable)
            interactableText.transform.position = interactableRef.position + interactableOffset;

    }



    private void FadeScreen(bool fadeIn)
    {
        OnFadeScreen.TryInvoke(fadeIn);
        fadeAnimator.SetBool("FadeIn", fadeIn);
    }


    private void ShowInteractable(Transform source, Vector3 offset)
    {
        trackInteractable = true;
        interactableRef = source;
        interactableOffset = offset;
        interactableText.transform.DOScale(Vector3.one, showInteractAnimDuration).SetEase(showInteractEase);
    }

    private void HideInteractable()
    {
        trackInteractable = false;
        interactableText.transform.DOScale(Vector3.zero, showInteractAnimDuration).SetEase(showInteractEase);
    }

    private async void RegisterDialogue(Transform t, string[] dialogue)
    {
        dialogueContainer.DOScale(1, dialogueAnimDuration).SetEase(dialogueInEase);
        await Task.Delay((int)dialogueAnimDuration * 1000 / 2);
        dialogueWriter.RegisterMessages(dialogue);
        NextMessage();
        await Task.Delay(500);
        isInCrewDialogue = true;
    }



    [Button("Playe Mentor Message")]
    public async void DisplayMentorMessage(string[] dial = null)
    {
        if (dial == null)
            await Task.Delay(3100);

        var dialogue = new string[] {
            "You have to destroy this ship before it's too late for us",
            "Your best option will be to try to destroy the engine",
            "Look for records on how to do this and good luck buddy, you're on your own" };
        mentorDialogueContainer.DOScale(1, dialogueAnimDuration).SetEase(dialogueInEase);
        await Task.Delay((int)dialogueAnimDuration * 1000 / 2);
        //isInCrewDialogue = true;
        await mentorDialogueWriter.WriteAllMessages(dial != null ? dial : dialogue, 2000);
        mentorDialogueContainer.DOScale(0, dialogueAnimDuration).SetEase(dialogueOutEase);
        //await Task.Delay(500);

        //NextMessage();
    }

    private void TryNextCrewMessage()
    {
        if (!isInCrewDialogue) return;

        if (!NextMessage())
        {
            isInCrewDialogue = false;
            dialogueContainer.DOScale(0, dialogueAnimDuration).SetEase(dialogueOutEase);
            OnFinishedDialogue.TryInvoke();
        }
    }

    private bool NextMessage() => dialogueWriter.WriteNextMessage();


    private async void FadeInOutScreen(int? delay = null)
    {
        FadeScreen(false);
        await Task.Delay(delay.HasValue ? delay.Value : LevelManager.currentManager.resetFreezeDurationMs);
        FadeScreen(true);
    }

    private void FadePauseMenu(bool fadeIn)
    {
        if (fadeIn)
            eventSystem.SetSelectedGameObject(null);

        pauseGroup.interactable = fadeIn;
        pauseGroup.blocksRaycasts = fadeIn;
        pauseGroup.DOFade(fadeIn ? 1 : 0, pauseAnimDuration).SetEase(fadeIn ? Ease.OutQuart : Ease.InQuart);
    }



    private void ShowEnding()
    {
        FadeInOutScreen(5000);
    }

    private async void ShowCredits()
    {
        creditsGroup.DOFade(1, 1.4f).SetEase(Ease.InQuad);
        await Task.Delay(1500);
        await creditsWriter.WriteAllMessages(creditsMessage, 4000);
        await Task.Delay(3000);
        LevelManager.currentManager.GoToMainMenu();
    }


}
