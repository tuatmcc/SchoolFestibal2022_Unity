using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public GameObject StartGameButton;
    public GameObject ConfirmPlayerNameButton;

    private SceneLoader SceneLoadManager;
    private RaceManager RManager;
    private CustomInputAction CustomInput;
    private TMP_InputField PlayerNameInputField;

    void Start()
    {
        GameObject rootGO = SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0];
        rootGO.TryGetComponent(out SceneLoadManager);
        rootGO.TryGetComponent(out RManager);
        TryGetComponent(out PlayerNameInputField);

        CustomInput = new CustomInputAction();
        CustomInput.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (CustomInput.UI.Confirm.WasPerformedThisFrame())
        {
            RManager.PlayerDisplayName = PlayerNameInputField.text;
            ConfirmPlayerNameButton.SetActive(false);
        }

        if (CustomInput.UI.LoadMainScene.WasPerformedThisFrame() &&
            !SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded)
        {
            SceneLoadManager.LoadScene(SceneNames.MainScene, SceneNames.TitleScene);
        }
    }
}
