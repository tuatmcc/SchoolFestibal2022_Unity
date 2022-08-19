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

    private GlobalGameManager GameManager;
    private CustomInputAction CustomInput;
    private TMP_InputField PlayerNameInputField;

    void Start()
    {
        SceneManager.GetSceneByName("ManagerScene").GetRootGameObjects()[0].TryGetComponent(out GameManager);
        TryGetComponent(out PlayerNameInputField);

        CustomInput = new CustomInputAction();
        CustomInput.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (CustomInput.UI.Confirm.WasPerformedThisFrame())
        {
            GameManager.PlayerName = PlayerNameInputField.text;
            ConfirmPlayerNameButton.SetActive(false);
        }
    }
}
