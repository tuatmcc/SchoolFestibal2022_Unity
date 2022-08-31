using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RaceGame.Scripts
{
    public class SceneLoader : MonoBehaviour
    {

        [SerializeField] private Image loadingUI;
        [SerializeField] private Camera cam;
        [SerializeField] private Image loadFadingImage;
        [SerializeField] private float fadeOutAnimationDuration = 1;

        private CustomInputAction _customInput;

        public static SceneLoader Instance { get; private set; }
        private void Awake()
        {
            // シングルトン......のつもり
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }

            _customInput = new CustomInputAction();
            _customInput.Enable();
        }


        private void Start()
        {
            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            loadFadingImage.gameObject.SetActive(false);

            if (SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded) return;
            if (SceneManager.GetSceneByName(SceneNames.TitleScene).isLoaded) return;
            if (SceneManager.GetSceneByName(SceneNames.ResultScene).isLoaded) return;

            var asyncLoad = SceneManager.LoadSceneAsync(SceneNames.TitleScene.ToString(), LoadSceneMode.Additive);
            asyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.TitleScene));
        }

        public void ToMainScene()
        {
            string loadSceneName = SceneNames.MainScene;
            string unloadSceneName = SceneNames.TitleScene;
            StartCoroutine(LoadSceneWithTransition(loadSceneName, unloadSceneName));
        }

        public void ToResultScene()
        {
            SceneManager.LoadScene(SceneNames.ResultScene, LoadSceneMode.Additive);
        }

        public void LoadSceneAdditive(string loadSceneName, string unloadSceneName="")
        {
            SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
        }

        private IEnumerator LoadSceneWithTransition(string loadSceneName, string unLoadSceneName)
        {
            loadingUI.gameObject.SetActive(true);
            cam.gameObject.SetActive(true);
            loadFadingImage.gameObject.SetActive(false);

            // Unload and load scenes
            var unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
            yield return unloadAsync;
            var loadAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            yield return loadAsync;
            yield return Resources.UnloadUnusedAssets();


            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);


            // Fade out
            loadFadingImage.gameObject.SetActive(true);
            float animationTime = 0;
            while (animationTime < fadeOutAnimationDuration)
            {
                animationTime += Time.deltaTime;
                yield return null;
            }

            // Done!
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadSceneName));
        }

        private IEnumerator LoadSceneWithoutTransition(string loadSceneName, string unLoadSceneName)
        {
            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            loadFadingImage.gameObject.SetActive(false);

            // Unload and load scenes

            var loadAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            yield return loadAsync;

            // Done!
            // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
        }
    }
}