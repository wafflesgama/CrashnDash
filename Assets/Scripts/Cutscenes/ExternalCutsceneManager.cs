using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class ExternalCutsceneManager : MonoBehaviour
{
    public static ExternalCutsceneManager instance;
    public PlayableDirector initialDirector;
    public PlayableDirector endingDirector;
    UEventHandler eventHandler = new UEventHandler();
    void Start()
    {
        PlayerCutsceneManager.OnEndingStarted.Subscribe(eventHandler, StartEnding);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableInitCutscene()
    {
        initialDirector.gameObject.SetActive(false);

    } 

    [Button("Start ending")]
    public async void StartEnding()
    {
        await Task.Delay(1000);
        endingDirector.gameObject.SetActive(true);
        endingDirector.Play();
    }

    public void FadeInEnding()
    {
        PlayerCutsceneManager.OnEndingFadeIn.TryInvoke();

    }
    public void StartCredits()
    {
        PlayerCutsceneManager.OnCreditsStarted.TryInvoke();
    }
}
