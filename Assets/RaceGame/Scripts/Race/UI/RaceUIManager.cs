using RaceGame.Race.Interface;
using RaceGame.Race.UI.Page;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI
{
    /// <summary>
    /// Raceで使うUIのアクティブ非アクティブを切り替える
    /// </summary>
    public class RaceUIManager : MonoBehaviour
    {
        [SerializeField] private CountDownPage countDownPage;
        [SerializeField] private RacePage racePage;
        [SerializeField] private ResultPage resultPage;
        
        private IRaceManager _raceManager;
        
        [Inject]
        private void Construct(IRaceManager raceManager)
        {
            _raceManager = raceManager;
            _raceManager.OnRaceStart += OnRaceStart;
            _raceManager.OnRaceFinish += OnRaceFinish;
        }

        private void Start()
        {
            SetActivePages(true, true, false);
        }

        private void OnRaceStart()
        {
            SetActivePages(false, true, false);
        }

        private void OnRaceFinish()
        {
            SetActivePages(false, false, true);
        }
        
        private void SetActivePages(bool countDown, bool race, bool result)
        {
            countDownPage.SetActive(countDown);
            racePage.SetActive(race);
            resultPage.SetActive(result);
        }
    }
}
