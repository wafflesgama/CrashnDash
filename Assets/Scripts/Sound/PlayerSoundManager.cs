using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static SoundUtils;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    

    public static PlayerSoundManager currentManager { get; private set; }

    public PlayerMovementController playerMovementController;
    public AudioSource[] externalAudioSources;

    [Header("Fading AudioSources")]
    public float fadeDuration = 2;
    public Ease fadeEase;

    [Header("Interaction Sounds")]
    public Sound interactInSound;
    public Sound interactOutSound;
    public Sound chestOpenSound;
    public Sound[] hitSounds;

    [Header("Dialogue Sounds")]
    public int numOfSkips = 1;
    public Sound dialogueTypeSound;

    [Header("Footstep Sounds")]
    public Sound[] grassFootstepSounds;
    public Sound[] sandFootstepSounds;
    public Sound[] stoneFootstepSounds;

    [Header("Other Movement Sounds")]
    public Sound[] jumpSounds;
    public Sound landSound;
    public Sound splashSound;

    AudioSource mainSource;
    int typeSkipCounter = 0;
    float mainSourceVol;
    float[] externalSourcesVol;
    UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        typeSkipCounter = 0;
        currentManager = this;
        mainSource = GetComponent<AudioSource>();
        SaveAllVolumes();
        SetAllSources(0);
        FadeAllSources(fadeIn: true);
    }

    private void Start()
    {
        PlayerMovementController.OnJumped.Subscribe(eventHandler, PlayJumpSound);
        PlayerMovementController.OnLanded.Subscribe(eventHandler, PlayLandSound);
        //PlayerInteractionHandler.OnInteractableAppeared.Subscribe(eventHandler, (x, y) => PlaySound(interactInSound));
        //PlayerInteractionHandler.OnInteractableDisappeared.Subscribe(eventHandler, () => PlaySound(interactOutSound));
        PlayerInteractionHandler.OnInteractableAppeared.Subscribe(eventHandler, PlayInteractIn);
        PlayerInteractionHandler.OnInteractableDisappeared.Subscribe(eventHandler, PlayInteractOut);
        PlayerUIManager.OnFadeScreen.Subscribe(eventHandler, PlayFadeSound);
        PlayerCutsceneManager.OnEndingStarted.Subscribe(eventHandler, () => FadeAllSources(fadeIn: false));
        PlayerInteractionHandler.OnSplash.Subscribe(eventHandler, (x) => PlaySound(splashSound));
    }

    private void OnDestroy()
    {
        if (currentManager == this)
            currentManager = null;


        eventHandler.UnsubcribeAll();
    }

    public void PlayInteractIn(Interactable x, Transform y)
    {
        PlaySound(interactInSound);
    }

    public void PlayInteractOut()
    {
        PlaySound(interactOutSound);
    }
    public void PlayFootStep()
    {
        PlaySound(PickRandomClip(grassFootstepSounds));
    }

    private void SetAllSources(float volume)
    {
        mainSource.volume = volume;
        foreach (var externalSource in externalAudioSources)
            externalSource.volume = volume;
    }

    private void SaveAllVolumes()
    {
        mainSourceVol = mainSource.volume;
        externalSourcesVol = new float[externalAudioSources.Length];
        for (int i = 0; i < externalSourcesVol.Length; i++)
            externalSourcesVol[i] = externalAudioSources[i].volume;
    }

    private void FadeAllSources(bool fadeIn)
    {
        mainSource.DOFade(fadeIn ? mainSourceVol : 0, fadeDuration).SetEase(fadeEase);
        for (int i = 0; i < externalAudioSources.Length; i++)
            externalAudioSources[i].DOFade(fadeIn ? externalSourcesVol[i] : 0, fadeDuration).SetEase(fadeEase);
    }

    public void PlayDialogueTypeSound()
    {
        if (typeSkipCounter == 0)
            PlaySound(dialogueTypeSound);

        typeSkipCounter++;
        if (typeSkipCounter > numOfSkips)
            typeSkipCounter = 0;
    }


    public void PlayUIClick()
    {
        PlaySound(dialogueTypeSound);
    }

    private void PlayFadeSound(bool fadeIn)
    {
        if (fadeIn)
            PlaySound(interactInSound);
        else
            PlaySound(interactOutSound);
    }


    private void PlayJumpSound()
    {
        PlaySound(PickRandomClip(jumpSounds));

    }

    private void PlayLandSound()
    {
        PlaySound(landSound);
    }


    private void PlaySoundCustomVol(Sound sound, float volumeFactor) => mainSource.PlayOneShot(sound.clip, sound.volume * volumeFactor);
    private void PlaySound(Sound sound) => mainSource.PlayOneShot(sound.clip, sound.volume);
    private Sound PickRandomClip(Sound[] source) => source[UnityEngine.Random.Range(0, source.Length)];
}
