using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using static SoundUtils;

public class MainMenuManager : MonoBehaviour
{
    public Image fade;
    public float fadeSpeed = .5f;
    public AudioSource audioSource;
    public AudioClip clickSound;

    public Sound fadeInSound;
    public Sound fadeOutSound;

    public CanvasGroup mainGroup;
    public CanvasGroup creditsGroup;
    public CanvasGroup settingsGroup;

    public Animator fadeAnimator;

    public void GoToMain() => ChangeScreen(mainGroup);
    public void GoToSettings() => ChangeScreen(settingsGroup);
    public void GoToCredits() => ChangeScreen(creditsGroup);

    bool inTransition;
    CanvasGroup currentGroup;
    void Awake()
    {
        fade.color = Color.black;
        FadeScreen(false);
        currentGroup = mainGroup;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !inTransition)
        {
            //inTransition = true;
            audioSource.PlayOneShot(clickSound);
            //GoToGame();
        }
    }

    private async void GoToGame()
    {
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(600);
        SceneManager.LoadSceneAsync("MainGame");
    }

    private async void ChangeScreen(CanvasGroup outGroup)
    {
        audioSource.PlayOneShot(clickSound);
        audioSource.PlaySound(fadeOutSound);
        fadeAnimator.SetBool("FadeIn", false);
        await Task.Delay(800);
        currentGroup.gameObject.SetActive(false);
        outGroup.gameObject.SetActive(true);
        currentGroup = outGroup;
        audioSource.PlaySound(fadeInSound);
        fadeAnimator.SetBool("FadeIn", true);
    }
    private void FadeScreen(bool fadeIn) => fade.DOFade(fadeIn ? 1 : 0, fadeSpeed).SetEase(Ease.InOutQuad);
}
