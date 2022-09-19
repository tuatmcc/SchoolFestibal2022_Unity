using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultDisplayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] RankTexts;

    private RaceManager RManager;

    void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out RManager);

        for (int i = 0; i < RManager.Characters.Count; i++)
        {
            RankTexts[i].text = i + 1 +". " + RManager.Characters[i].DisplayName;
            if (RManager.Characters[i].isPlayer)
            {
                RankTexts[i].color = Color.green;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
