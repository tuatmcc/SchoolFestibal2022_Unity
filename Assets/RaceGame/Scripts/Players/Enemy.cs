using Cinemachine;
using UnityEngine;

using RaceGame.Manager;
using RaceGame.Constant;

namespace RaceGame.Players
{
    /// <summary>
    /// Enemyを動かす。
    /// </summary>
    [RequireComponent(typeof(CinemachineDollyCart))]
    [RequireComponent(typeof(Character))]
    public class Enemy : MonoBehaviour
    {
        // Enemyの強さを決めるパラメーター
        /// <summary>
        /// 各フレームで加速する確率
        /// </summary>
        [Range(0f, 1f)] public float enemyTapFrequency = 0.01f;
        public float maxSpeedLimit = 30f;

        private Character _character;
        private CinemachineDollyCart _dollyCart;

        private void Start()
        {
            _dollyCart = GetComponent<CinemachineDollyCart>();
            _character = GetComponent<Character>();
        }

        private void FixedUpdate()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Started)
            {
                // 0~1の乱数を生成し、enemyTapFrequency 以下の値であれば加速する
                var rand = Random.value;
                if (rand < enemyTapFrequency && _dollyCart.m_Speed < maxSpeedLimit)
                {
                    _character.Accelerate();
                }
            }
        }
    }
}