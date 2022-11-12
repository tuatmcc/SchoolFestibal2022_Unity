using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.UI.Parts
{
    public class GoalEffectView : MonoBehaviour
    {
        [SerializeField] private RealTimeRankingView realTimeRankingView;
        [Inject] private IRaceManager _raceManager;

        private void Start()
        {
            _raceManager.OnCountDownStart += Set;
        }

        private void Set()
        {
            _raceManager.LocalPlayer.OnGoaled += ShowGoalView;
        }

        private void ShowGoalView()
        {
            realTimeRankingView.gameObject.SetActive(true);
            Inactivate(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTask Inactivate(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            realTimeRankingView.gameObject.SetActive(false);
        }
    }
}