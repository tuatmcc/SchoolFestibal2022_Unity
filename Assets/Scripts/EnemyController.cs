using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(CharacterControll))]

public class EnemyController : MonoBehaviour
{
    // Enemy Strength is dependent on these two parematers
    [Range(0f, 1f)] public float EnemyTapFrequency = 0.01f;
    public float MaxSpeedLimit = 30f;

    private RaceManager RManager;
    private CharacterControll Character;
    private CinemachineDollyCart DollyCart;

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out RManager);
        TryGetComponent(out DollyCart);
        TryGetComponent(out Character);

    }

    private void Update()
    {
        if (RManager.RaceStarted)
        {
            // Accelerate randomly
            float Rand = Random.value;
            if (Rand < EnemyTapFrequency && DollyCart.m_Speed < MaxSpeedLimit)
            {
                Character.Accelerate();
            }
        }
    }
}
