using UnityEngine;
using UnityEngine.SceneManagement;

namespace RaceGame.Scene
{
    /// <summary>
    /// ManagerSceneを常に初めに読み込む。
    /// </summary>
    
    //以下テスト用に無効化
    /*
    public class ManagerSceneAutoLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            if (SceneManager.GetSceneByName(SceneName.ManagerScene).IsValid()) return;
            SceneManager.LoadSceneAsync(SceneName.ManagerScene, LoadSceneMode.Additive).allowSceneActivation = false;
        }
    }
    */
}