using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class RaceManger : MonoBehaviour
{
    [SerializeField] private CharacterController[] Characters;
    [SerializeField] private CinemachineSmoothPath Path;

    public float StartTime { get; private set; } = 4f;
    public static bool IsRaceStarted { get; private set; } = false;

    private class Chara
    {
        public CharacterController chara { get; set; }
    }

    private List<Chara> Charas;

    void Start()
    {
        Charas = new List<Chara>();
        foreach (CharacterController character in Characters)
        {
            Charas.Add(new Chara() { chara = character });
        }
    }

    void Update()
    {
        StartTime -= Time.deltaTime;
        if (StartTime <= 0f) IsRaceStarted = true;
        if (IsRaceStarted) UpdateCurrentRanking();
    }

    private void UpdateCurrentRanking()
    {
        Charas.Sort((a, b) => (b.chara.Position * 100).CompareTo(a.chara.Position * 100));

        for (int i = 0; i < Charas.Count; i++)
        {
            Charas[i].chara.Rank = i + 1;
        }
    }
}
