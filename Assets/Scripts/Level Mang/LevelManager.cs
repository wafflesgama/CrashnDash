using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UEventHandler;

public class LevelManager : MonoBehaviour
{
    public static LevelManager currentManager;
    public static UEvent OnExitScreen = new UEvent();
    public static UEvent OnRestartLevel = new UEvent();

    public int resetFreezeDurationMs = 800;

    private bool restarFlag;

    private UEventHandler eventHandler = new UEventHandler();

    private void Awake()
    {
        currentManager = this;
    }

    private void OnDestroy()
    {
        if (currentManager == this)
            currentManager = null;
    }


    #region Level & Scene Mang

    public async void ResetPlayer()
    {
        OnRestartLevel.TryInvoke();
        await Task.Delay(700);

    }

    public async void RestartScene()
    {
        if (restarFlag) return;
        restarFlag = true;

        OnExitScreen.TryInvoke();
        await Task.Delay(700);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public async void GoToMainMenu()
    {
        OnExitScreen.TryInvoke();
        //OnExitScreen?.Invoke();
        await Task.Delay(700);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public async void ExitGame()
    {
        OnExitScreen.TryInvoke();
        await Task.Delay(700);
        Application.Quit();
    }

    #endregion Level & Scene Mang

}
