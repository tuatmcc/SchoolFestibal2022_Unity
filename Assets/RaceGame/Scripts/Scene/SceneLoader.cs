using System.Collections;
using RaceGame.Extension;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RaceGame.Scene
{
    public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        [SerializeField] private Image loadingUI;
        [SerializeField] private Camera cam;
        [SerializeField] private Image loadFadeOutImage;
        [SerializeField] private Animator fadeOutAnimator;

        private CustomInputAction _customInput;

        private void Start()
        {
            _customInput = new CustomInputAction();
            _customInput.Enable();
            
            // UIをオフ
            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            loadFadeOutImage.gameObject.SetActive(false);

            if (SceneManager.GetSceneByName(SceneName.MainScene).isLoaded) return;
            if (SceneManager.GetSceneByName(SceneName.TitleScene).isLoaded) return;
            if (SceneManager.GetSceneByName(SceneName.ResultScene).isLoaded) return;

            var asyncLoad = SceneManager.LoadSceneAsync(SceneName.TitleScene, LoadSceneMode.Additive);
            asyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName.TitleScene));
        }

        public void ToMainScene()
        {
            var loadSceneName = SceneName.MainScene;
            var unloadSceneName = SceneName.TitleScene;
            StartCoroutine(LoadSceneWithTransition(loadSceneName, unloadSceneName));
        }

        public void ToResultScene()
        {
            SceneManager.LoadScene(SceneName.ResultScene, LoadSceneMode.Additive);
        }

        public void LoadSceneAdditive(string loadSceneName, string unloadSceneName="")
        {
            SceneManager.LoadScene(loadSceneName, LoadSceneMode.Additive);
        }

        private IEnumerator LoadSceneWithTransition(string loadSceneName, string unLoadSceneName)
        {
            // ローディングUIオン
            loadingUI.gameObject.SetActive(true);
            cam.gameObject.SetActive(true);
            loadFadeOutImage.gameObject.SetActive(false);

            // 前シーンをアンロード
            var unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
            yield return unloadAsync;
            yield return Resources.UnloadUnusedAssets();
            
            // 次シーンをロード
            var loadAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            yield return loadAsync;
            
            //　ローディングUIオフ
            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);


            // フェードアウト開始
            loadFadeOutImage.gameObject.SetActive(true);
            while (!fadeOutAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
            {
                yield return null;
            }
            loadFadeOutImage.gameObject.SetActive(false);

            // Done!
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadSceneName));
        }

        private IEnumerator LoadSceneWithoutTransition(string loadSceneName, string unLoadSceneName)
        {
            loadingUI.gameObject.SetActive(false);
            cam.gameObject.SetActive(false);
            loadFadeOutImage.gameObject.SetActive(false);

            // Unload and load scenes

            var loadAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
            yield return loadAsync;

            // Done!
            // SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadceneName));
        }
    }
}