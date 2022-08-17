using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(Animator))]

public class EnemyController : MonoBehaviour
{
    public string DisplayName = "‚¨‚¤‚Ü‚³‚ñ";

    // Enemy Strength is dependent on these two parematers
    [SerializeField] private float EnemyTapFrequency = 0.01f;
    [SerializeField] private float MaxSpeedLimit = 20f;

    private CinemachineDollyCart DollyCart;
    private float SpeedUpPerTap = 5f;
    private float SlowDownMultipler = 0.99f;

    private void Start()
    {
        TryGetComponent(out DollyCart);
        DollyCart.m_Path = GameObject.FindGameObjectWithTag("Path").GetComponent<CinemachineSmoothPath>();
    }

    private void Update()
    {
        // Tap randomly
        float Rand = Random.value;
        if (Rand < EnemyTapFrequency && DollyCart.m_Speed < MaxSpeedLimit)
        {
            DollyCart.m_Speed += SpeedUpPerTap;
        }

        // Decrease speed every frame
        DollyCart.m_Speed *= SlowDownMultipler;
    }
}
