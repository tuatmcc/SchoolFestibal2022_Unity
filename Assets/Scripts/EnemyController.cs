using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(CharacterController))]

public class EnemyController : MonoBehaviour
{
    // Enemy Strength is dependent on these two parematers
    public float EnemyTapFrequency = 0.01f;
    public float MaxSpeedLimit = 20f;

    private CharacterController Character;
    private CinemachineDollyCart DollyCart;

    private void Start()
    {
        TryGetComponent(out DollyCart);
        TryGetComponent(out Character);

    }

    private void Update()
    {
        // Accelerate randomly
        float Rand = Random.value;
        if (Rand < EnemyTapFrequency && DollyCart.m_Speed < MaxSpeedLimit)
        {
            Character.Accelerate();
        }
    }
}
