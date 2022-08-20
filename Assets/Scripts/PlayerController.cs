using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private GlobalGameManager GameMemanager;
    private CustomInputAction CustomInput;
    private CharacterController Character;

    private void Awake()
    {
        CustomInput = new CustomInputAction();
        CustomInput.Enable();
    }

    private void Start()
    {
        SceneManager.GetSceneByName("ManagerScene").GetRootGameObjects()[0].TryGetComponent(out GameMemanager);
        TryGetComponent(out Character);

        // Set Player Name
        Character.DisplayName = GameMemanager.PlayerName;
    }

    private void Update()
    {
        if (RaceManger.IsRaceStarted)
        {
            if (CustomInput.Player.Accelerate.WasPerformedThisFrame())
            {
                Character.Accelerate();
            }
        }
    }
}
