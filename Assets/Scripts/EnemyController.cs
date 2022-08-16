using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineDollyCart))]

public class EnemyController : MonoBehaviour
{
    // This paramater is related to an enemy level
    [SerializeField] private float EnemyTapFrequency = 0.01f;
    [SerializeField] private float MaxSpeedLimit = 20f;

    private CinemachineDollyCart DollyCart;
    private float SpeedUpPerTap = 5f;
    private float SlowDownMultipler = 0.99f;

    private void Start()
    {
        DollyCart = GetComponent<CinemachineDollyCart>();
    }

    private void Update()
    {
        // Tap randomly
        float Rand = Random.value;
        if (Rand < EnemyTapFrequency && DollyCart.m_Speed < MaxSpeedLimit)
        {
            DollyCart.m_Speed += SpeedUpPerTap;
        }
        DollyCart.m_Speed *= SlowDownMultipler;
    }
}
