using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class RankingManger : MonoBehaviour
{
    [SerializeField] private CharacterController[] Characters;
    [SerializeField] private CinemachineSmoothPath Path;

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
        Charas.Sort((a, b) => (b.chara.Position).CompareTo(a.chara.Position));

        for (int i = 0; i < Charas.Count; i++)
        {
            Charas[i].chara.Rank = i + 1;
        }
    }
}
