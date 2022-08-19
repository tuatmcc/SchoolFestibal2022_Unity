using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GlobalGameManager : MonoBehaviour
{
    public AsyncOperation AsyncLoad;
    private string MainSceneName = "MainScene";
    private string TitleSceneName = "TitleScene";

    private CustomInputAction CustomInput;

    public string PlayerName { get; set; } = "Default Name";

    private void Awake()
    {
        CustomInput = new CustomInputAction();
        CustomInput.Enable();
    }
    void Start()
    {
        if (SceneManager.GetSceneByName(MainSceneName).isLoaded) return;
        if (SceneManager.GetSceneByName(TitleSceneName).isLoaded) return;

        AsyncLoad =  SceneManager.LoadSceneAsync(TitleSceneName, LoadSceneMode.Additive);
        AsyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(TitleSceneName));
    }

    void Update()
    {
        if (CustomInput.UI.LoadMainScene.WasPerformedThisFrame() &&
            !SceneManager.GetSceneByName(MainSceneName).IsValid())
        {
            SceneManager.UnloadSceneAsync(TitleSceneName);
            AsyncLoad = SceneManager.LoadSceneAsync(MainSceneName, LoadSceneMode.Additive);
            AsyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainSceneName));
        }
    }


}
