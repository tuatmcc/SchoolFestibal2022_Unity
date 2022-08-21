using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GlobalDataManager : MonoBehaviour
{
    public AsyncOperation AsyncLoad;

    private CustomInputAction CustomInput;

    public string PlayerName { get; set; } = "Default Name";
    public List<CharacterController> Characters { get; set; }

    private void Awake()
    {
        CustomInput = new CustomInputAction();
        CustomInput.Enable();
    }
    void Start()
    {
        if (SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded) return;
        if (SceneManager.GetSceneByName(SceneNames.TitleScene).isLoaded) return;

        AsyncLoad =  SceneManager.LoadSceneAsync(SceneNames.TitleScene.ToString(), LoadSceneMode.Additive);
        AsyncLoad.completed += e => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames.TitleScene));
    }

    void Update()
    {
        
    }
}
