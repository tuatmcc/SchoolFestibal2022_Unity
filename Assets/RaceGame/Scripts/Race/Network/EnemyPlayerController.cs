using RaceGame.Race.Interface;
using RaceGame.Race.Sound;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

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
        [Inject] private IPlayer _player;

        private SePlayer _sePlayer;
        private void Start()
        {
            // 急ぎで雑なやり方
            // 本来であればFactoryPattern等で対応する
            if (_player == null)
            {
                _player = GetComponent<Player>();
            }

            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }

            _sePlayer = GetComponent<SePlayer>();
        }

        private void FixedUpdate()
        {
            if (_raceManager.RaceState == RaceState.Racing)
            {
                // 0~1の乱数を生成し、enemyTapFrequency 以下の値であれば加速する
                var rand = Random.value;
                if (rand < enemyTapFrequency && _player.Speed < maxSpeedLimit)
                {
                    _player.CmdAccelerate();
                    if(_player.Speed != 0) _sePlayer.PlayFootStep();
                }
            }
        }
    }
}