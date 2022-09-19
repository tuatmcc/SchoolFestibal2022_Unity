using UnityEngine;
using UnityEngine.SceneManagement;

namespace RaceGame.Scripts
{
    public class ManagerSceneAutoLoader : MonoBehaviour
    {
        // This script always runs before any scene loaded without attaching any object
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadManagerScene()
        {
            if (SceneManager.GetSceneByName(SceneNames.ManagerScene).IsValid()) return;
            SceneManager.LoadSceneAsync(SceneNames.ManagerScene, LoadSceneMode.Additive).allowSceneActivation = false;
        }
    }
}