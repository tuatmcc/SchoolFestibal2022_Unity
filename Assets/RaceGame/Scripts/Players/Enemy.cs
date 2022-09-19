using Cinemachine;
using RaceGame.Constant;
using RaceGame.Manager;
using UnityEngine;

namespace RaceGame.Players
{
    [RequireComponent(typeof(CinemachineDollyCart))]
    [RequireComponent(typeof(Character))]
    public class Enemy : MonoBehaviour
    {
        // Enemyの強さに影響するパラメータ
        [Range(0f, 1f)] public float enemyTapFrequency = 0.01f;
        public float maxSpeedLimit = 30f;

        private Character _character;
        private CinemachineDollyCart _dollyCart;

        private void Start()
        {
            _dollyCart = GetComponent<CinemachineDollyCart>();
            _character = GetComponent<Character>();
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Started)
            {
                // 確率で加速
                var rand = Random.value;
                if (rand < enemyTapFrequency && _dollyCart.m_Speed < maxSpeedLimit)
                {
                    _character.Accelerate();
                }
            }
        }
    }
}