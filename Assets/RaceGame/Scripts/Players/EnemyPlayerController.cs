using UnityEngine;
using RaceGame.Manager;

namespace RaceGame.Players
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

        private Player _player;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Racing)
            {
                // 0~1の乱数を生成し、enemyTapFrequency 以下の値であれば加速する
                var rand = Random.value;
                if (rand < enemyTapFrequency && _player.Cart.m_Speed < maxSpeedLimit)
                {
                    _player.CmdAccelerate();
                }
            }
        }
    }
}