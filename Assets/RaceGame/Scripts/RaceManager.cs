using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class RaceManager : MonoBehaviour
{
    public string PlayerDisplayName { get; set; } = "Nameless";
    public float CountDownTimer { get; private set; } = 5f;
    public bool RaceStarted { get; private set; } = false;
    public bool RaceEnded { get; private set; } = false;
    public List<CharacterController> Characters { get; set; } = new List<CharacterController>();

    private SceneLoader _sceneLoadManager;
    private CinemachineSmoothPath _path;


    private void Start()
    {
        TryGetComponent(out _sceneLoadManager);
    }

    private void Update()
    {        
        if (SceneManager.GetActiveScene().name != SceneNames.MainScene) return;

        SceneManager.GetActiveScene().GetRootGameObjects()[0].TryGetComponent(out _path);

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
        Characters.Sort((a, b) => (b.position * 100).CompareTo(a.position * 100));

        for (var i = 0; i < Characters.Count; i++)
        {
            Characters[i].rank = i + 1;
        }

        if (Characters[Characters.Count-1].position == _path.PathLength)
        {
            RaceEnded = true;
            _sceneLoadManager.LoadSceneAdditive(SceneNames.ResultScene, SceneNames.MainScene);
        }
    }
}
