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
    public float CountDownTimer { get; private set; } = 5f;
    public bool RaceStarted { get; private set; } = false;
    public bool RaceEnded { get; private set; } = false;
    public List<CharacterControll> Characters { get; set; } = new List<CharacterControll>();

    private SceneLoader SceneLoadManager;

    private void Start()
    {
        TryGetComponent(out SceneLoadManager);
    }

    void Update()
    {        
        if (SceneManager.GetActiveScene().name != SceneNames.MainScene) return;

        if (!RaceStarted)
        {
            CountDownTimer -= Time.deltaTime;
            RaceStarted = CountDownTimer <= 0 ? true : false;
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

        if (Characters[Characters.Count-1].Position == Path.PathLength)
        {
            RaceEnded = true;
            SceneLoadManager.LoadSceneAdditive(SceneNames.ResultScene, SceneNames.MainScene);
        }
    }
}
