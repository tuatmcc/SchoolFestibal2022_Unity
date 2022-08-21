using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] private CinemachineSmoothPath Path;

    public string PlayerDisplayName { get; set; } = "Nameless";
    public float StartTime { get; private set; } = 5f;
    public bool RaceStarted { get; private set; } = false;
    public bool RaceEnded { get; private set; } = false;
    public List<CharacterController> Characters { get; set; } = new List<CharacterController>();


    void Update()
    {        
        if (SceneManager.GetActiveScene().name != SceneNames.MainScene) return;

        if (!RaceStarted)
        {
            StartTime -= Time.deltaTime;
            RaceStarted = StartTime <= 0 ? true : false;
        }
        else if (!RaceEnded)
        {
            UpdateCurrentRanking();
        }
        else
        {
            
        }
    }

    private void UpdateCurrentRanking()
    {
        Characters.Sort((a, b) => (b.Position * 100).CompareTo(a.Position * 100));

        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].Rank = i + 1;
        }

        if (Characters[Characters.Count-1].Position == Path.MaxPos)
        {
            RaceEnded = true;
        }
    }
}
