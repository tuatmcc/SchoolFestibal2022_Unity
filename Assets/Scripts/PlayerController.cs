using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineDollyCart))]

public class PlayerController : MonoBehaviour
{
    private PlayerInput CustomInput;
    private CinemachineDollyCart DollyCart;
    private float SpeedUpPerTap = 5f;
    private float SlowDownMultipler = 0.99f;

    private void Awake()
    {
        CustomInput = new PlayerInput();
        CustomInput.Enable();
    }

    private void Start()
    {
        DollyCart = GetComponent<CinemachineDollyCart>();
    }

    private void Update()
    {
        if (CustomInput.Player.Accelerate.WasPerformedThisFrame())
        {
            DollyCart.m_Speed += SpeedUpPerTap;
        }
        // Decrease speed every frame
        DollyCart.m_Speed *= SlowDownMultipler;
    }
}
