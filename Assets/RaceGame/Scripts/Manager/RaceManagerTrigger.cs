using System;
using Cinemachine;
using RaceGame.Players;
using UnityEngine;

namespace RaceGame.Manager
{
    /// <summary>
    /// MainSceneがロードされると、RaceMangerにシーン上のオブジェクトを渡す
    /// </summary>
    public class RaceManagerTrigger: MonoBehaviour
    {
        private void Awake()
        {
            RaceManager.Instance.Characters = FindObjectsOfType<Character>();
            RaceManager.Instance.Path = GameObject.FindGameObjectWithTag("Path").GetComponent<CinemachineSmoothPath>();
            RaceManager.Instance.StandByForRace();
        }
    }
}