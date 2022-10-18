// 対応が大変だったので一旦無効にしています

// using System.Collections;
// using Mirror;
// using RaceGame.Scene.Core;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;
//
// namespace RaceGame.Scene.Title.View
// {
//     /// <summary>
//     /// シーンの遷移とその間のアニメーションを担う
//     /// </summary>
//     public class LoadingView : NetworkManager
//     {
//         [SerializeField] private Image loadingUI;
//         [SerializeField] private Camera cam;
//         [SerializeField] private Image loadFadeOutImage;
//         [SerializeField] private Animator fadeOutAnimator;
//
//         private void Start()
//         {
//             // UIをすべてオフ
//             loadingUI.gameObject.SetActive(false);
//             cam.gameObject.SetActive(false);
//             loadFadeOutImage.gameObject.SetActive(false);
//             
//             // 他のシーンがロードされていなければタイトルシーンをロードする
//             if (SceneManager.GetSceneByName(SceneName.MainScene).isLoaded) return;
//             if (SceneManager.GetSceneByName(SceneName.TitleScene).isLoaded) return;
//             if (SceneManager.GetSceneByName(SceneName.ResultScene).isLoaded) return;
//
//             var asyncLoad = SceneManager.LoadSceneAsync(SceneName.TitleScene, LoadSceneMode.Additive);
//             asyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName.TitleScene));
//         }
//
//         public void ToMainScene()
//         {
//             var loadSceneName = SceneName.MainScene;
//             var unloadSceneName = SceneName.TitleScene;
//             StartCoroutine(LoadSceneWithTransition(loadSceneName, unloadSceneName));
//         }
//
//         public void ToResultScene()
//         {
//             SceneManager.LoadScene(SceneName.ResultScene, LoadSceneMode.Additive);
//         }
//
//         private IEnumerator LoadSceneWithTransition(string loadSceneName, string unLoadSceneName)
//         {
//             // ローディングUIオン
//             loadingUI.gameObject.SetActive(true);
//             cam.gameObject.SetActive(true);
//             loadFadeOutImage.gameObject.SetActive(false);
//
//             // 前シーンをアンロード
//             var unloadAsync = SceneManager.UnloadSceneAsync(unLoadSceneName);
//             yield return unloadAsync;
//             yield return Resources.UnloadUnusedAssets();
//             
//             // 次シーンをロード
//             var loadAsync = SceneManager.LoadSceneAsync(loadSceneName, LoadSceneMode.Additive);
//             yield return loadAsync;
//             
//             //　ローディングUIオフ
//             loadingUI.gameObject.SetActive(false);
//             cam.gameObject.SetActive(false);
//
//
//             // フェードアウト開始
//             loadFadeOutImage.gameObject.SetActive(true);
//             while (!fadeOutAnimator.GetCurrentAnimatorStateInfo(0).IsName("Do Nothing"))
//             {
//                 yield return null;
//             }
//             loadFadeOutImage.gameObject.SetActive(false);
//
//             // Done!
//             SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadSceneName));
//         }
//     }
// }