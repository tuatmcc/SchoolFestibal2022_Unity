using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultDisplayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] rankTexts;

    private RaceManager _rManager;

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out _rManager);

        for (var i = 0; i < _rManager.Characters.Count; i++)
        {
            rankTexts[i].text = i + 1 +". " + _rManager.Characters[i].displayName;
            if (_rManager.Characters[i].IsPlayer)
            {
                rankTexts[i].color = Color.green;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
