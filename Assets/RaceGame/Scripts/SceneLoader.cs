using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image loadingUI;
    [SerializeField] private Camera cam;
    [SerializeField] private Image fadingImage;
    [SerializeField] private float fadeOutAnimationDuration = 1;

    private CustomInputAction _customInput;

    private void Awake()
    {
        _customInput = new CustomInputAction();
        _customInput.Enable();
    }

    private void Start()
    {
        loadingUI.gameObject.SetActive(false);
        cam.gameObject.SetActive(false);
        fadingImage.gameObject.SetActive(false);

        if (SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded) return;
        if (SceneManager.GetSceneByName(SceneNames.TitleScene).isLoaded) return;
        if (SceneManager.GetSceneByName(SceneNames.ResultScene).isLoaded) return;

        var asyncLoad = SceneManager.LoadSceneAsync(SceneNames.TitleScene.ToString(), LoadSceneMode.Additive);
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
        loadingUI.gameObject.SetActive(true);
        cam.gameObject.SetActive(true);
        fadingImage.gameObject.SetActive(false);
        
        // Unload and load scenes
        var unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
        yield return unloadAsync;
        var loadAsync = SceneManager.LoadSceneAsync(loadceneName, LoadSceneMode.Additive);
        yield return loadAsync;
        yield return Resources.UnloadUnusedAssets();


        loadingUI.gameObject.SetActive(false);
        cam.gameObject.SetActive(false);


        // Fade out
        fadingImage.gameObject.SetActive(true);
        float animationTime = 0;
        while (animationTime < fadeOutAnimationDuration)
        {
            animationTime += Time.deltaTime;
            yield return null;
        }
        
        // Done!
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
    }

    private IEnumerator LoadSceneWithoutTransition(string loadceneName, string unLoadSceneName)
    {
        loadingUI.gameObject.SetActive(false);
        cam.gameObject.SetActive(false);
        fadingImage.gameObject.SetActive(false);

        // Unload and load scenes

        var loadAsync = SceneManager.LoadSceneAsync(loadceneName, LoadSceneMode.Additive);
        yield return loadAsync;

        // Done!
        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
    }
}
