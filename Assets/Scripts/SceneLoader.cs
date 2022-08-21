using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image LoadingUI;
    [SerializeField] private Camera Cam;
    [SerializeField] private Image FadingImage;

    private CustomInputAction CustomInput;
    private float FadeOutPerFrame = 0.01f;
    private float Progress;

    private void Awake()
    {
        CustomInput = new CustomInputAction();
        CustomInput.Enable();
    }

    void Start()
    {
        LoadingUI.gameObject.SetActive(false);
        Cam.gameObject.SetActive(false);
        FadingImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (CustomInput.UI.LoadMainScene.WasPerformedThisFrame() &&
            !SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded)
        {
            StartCoroutine(LoadAndUnloadScene(SceneNames.MainScene, SceneNames.TitleScene));
        }
    }


    private IEnumerator LoadAndUnloadScene(string loadceneName, string unLoadSceneName)
    {
        LoadingUI.gameObject.SetActive(true);
        Cam.gameObject.SetActive(true);
        FadingImage.gameObject.SetActive(false);

        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(loadceneName, LoadSceneMode.Additive);

        while (!unloadAsync.isDone || !loadAsync.isDone)
        {
            Progress = (unloadAsync.progress + loadAsync.progress) * 0.5f;
            Debug.Log(Progress);
            yield return null;
        }

        yield return Resources.UnloadUnusedAssets();

        LoadingUI.gameObject.SetActive(false);
        Cam.gameObject.SetActive(false);

        FadingImage.gameObject.SetActive(true);
        FadingImage.color = Color.black;
        while (FadingImage.color.a - FadeOutPerFrame >= 0)
        {
            FadingImage.color = new Color(0, 0, 0, FadingImage.color.a - FadeOutPerFrame);
            yield return null;
        }
        yield return null;
        FadingImage.gameObject.SetActive(false);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
    }


}
