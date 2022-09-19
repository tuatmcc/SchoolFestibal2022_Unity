using UnityEngine;
using UnityEngine.SceneManagement;

namespace RaceGame.Scene
{
    public class ManagerSceneAutoLoader : MonoBehaviour
    {
        // This script always runs before any scene loaded without attaching any object
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            if (SceneManager.GetSceneByName(SceneName.ManagerScene).IsValid()) return;
            SceneManager.LoadSceneAsync(SceneName.ManagerScene, LoadSceneMode.Additive).allowSceneActivation = false;
        }
    }
}