using RaceGame.Race.Interface;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.Network
{
    /// <summary>
    /// Enemyを動かす。
    /// </summary>
    [RequireComponent(typeof(Player))]
    public class EnemyPlayerController : MonoBehaviour
    {
        // Enemyの強さを決めるパラメーター
        /// <summary>
        /// 各フレームで加速する確率
        /// </summary>
        [Range(0f, 1f)] public float enemyTapFrequency = 0.01f;
        
        /// <summary>
        /// 最高速度
        /// </summary>
        public float maxSpeedLimit = 30f;

        [Inject] private IRaceManager _raceManager;
        [SerializeReference] private IPlayer _player;

        private void Update()
        {
            if (_raceManager.RaceState == RaceState.Racing)
            {
                // 0~1の乱数を生成し、enemyTapFrequency 以下の値であれば加速する
                var rand = Random.value;
                if (rand < enemyTapFrequency && _player.Speed < maxSpeedLimit)
                {
                    _player.CmdAccelerate();
                }
            }
        }
    }
}