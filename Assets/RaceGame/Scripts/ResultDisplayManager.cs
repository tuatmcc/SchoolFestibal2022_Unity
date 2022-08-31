using RaceGame.Scripts;
using UnityEngine;
using TMPro;

namespace RaceGame.Scripts
{
    public class ResultDisplayManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] rankTexts;

        private void Start()
        {
            for (var i = 0; i < RaceManager.Instance.Characters.Count; i++)
            {
                rankTexts[i].text = i + 1 + ". " + RaceManager.Instance.Characters[i].displayName;
                if (RaceManager.Instance.Characters[i].IsPlayer)
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
}