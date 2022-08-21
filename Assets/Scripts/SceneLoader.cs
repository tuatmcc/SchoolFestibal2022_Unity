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

        if (SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded) return;
        if (SceneManager.GetSceneByName(SceneNames.TitleScene).isLoaded) return;
        if (SceneManager.GetSceneByName(SceneNames.ResultScene).isLoaded) return;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNames.TitleScene.ToString(), LoadSceneMode.Additive);
        asyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.TitleScene));
    }

    public void LoadScene(string loadSceneName, string unloadSceneName)
    {
        StartCoroutine(LoadSceneWithTransition(loadSceneName, unloadSceneName));
    }

    public void LoadSceneAdditive(string loadSceneName, string unloadSceneName)
    {
        SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
    }

    private IEnumerator LoadSceneWithTransition(string loadceneName, string unLoadSceneName)
    {
        LoadingUI.gameObject.SetActive(true);
        Cam.gameObject.SetActive(true);
        FadingImage.gameObject.SetActive(false);
        
        // Unload and load scenes
        AsyncOperation unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return unloadAsync;
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(loadceneName, LoadSceneMode.Additive);
        yield return loadAsync;
        yield return Resources.UnloadUnusedAssets();


        LoadingUI.gameObject.SetActive(false);
        Cam.gameObject.SetActive(false);


        // Fade out
        FadingImage.gameObject.SetActive(true);
        FadingImage.color = Color.black;
        while (FadingImage.color.a - FadeOutPerFrame >= 0)
        {
            FadingImage.color = new Color(0, 0, 0, FadingImage.color.a - FadeOutPerFrame);
            yield return null;
        }
        yield return null;
        FadingImage.gameObject.SetActive(false);
        
        // Done!
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
    }

    private IEnumerator LoadSceneWithoutTransition(string loadceneName, string unLoadSceneName)
    {
        LoadingUI.gameObject.SetActive(false);
        Cam.gameObject.SetActive(false);
        FadingImage.gameObject.SetActive(false);

        // Unload and load scenes

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(loadceneName, LoadSceneMode.Additive);
        yield return loadAsync;

        // Done!
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
    }
}
