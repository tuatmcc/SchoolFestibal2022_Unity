using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingView : MonoBehaviour
{
    private GlobalGameManager GameManager;
    private AsyncOperation AsyncLoad;

    void Start()
    {
        SceneManager.GetSceneByName("ManagerScene").GetRootGameObjects()[0].TryGetComponent(out GameManager);
    }

    void Update()
    {
    }
}
