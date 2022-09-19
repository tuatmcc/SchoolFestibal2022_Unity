using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterControll))]

public class PlayerController : MonoBehaviour
{
    private RaceManager RManager;
    private CustomInputAction CustomInput;
    private CharacterControll Character;

    private void Awake()
    {
        CustomInput = new CustomInputAction();
        CustomInput.Enable();
    }

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out RManager);
        TryGetComponent(out Character);

        // Set Player Name
        Character.DisplayName = RManager.PlayerDisplayName;
        Character.isPlayer = true;
    }

    private void Update()
    {
        if (RManager.RaceStarted)
        {
            if (CustomInput.Player.Accelerate.WasPerformedThisFrame())
            {
                Character.Accelerate();
            }
        }
    }
}
