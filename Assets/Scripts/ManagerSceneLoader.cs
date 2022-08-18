using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneLoader : MonoBehaviour
{
    // This script always runs before any scene loaded without attaching any object
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void LoadManagerScene()
    {
        string managerSceneName = "ManagerScene";
        if (SceneManager.GetSceneByName(managerSceneName).IsValid()) return;
        SceneManager.LoadScene(managerSceneName, LoadSceneMode.Additive);
    }
}
