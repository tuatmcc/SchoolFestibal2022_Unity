using Cinemachine;
using Mirror;
using UnityEngine;
using TMPro;
using RaceGame.Manager;

namespace RaceGame.Players
{
    /// <summary>
    /// プレイヤーの情報を管理するクラス
    /// PlayerまたはEnemyから操作する
    /// Positionの計算を行うのはサーバーのみ
    /// </summary>
    [RequireComponent(typeof(CinemachineDollyCart))]
    [RequireComponent(typeof(Animator))]
    public class Player : NetworkBehaviour
    {
        public float Position => _position;
        
        [SyncVar]
        public string playerName = "CPU";

        [SyncVar(hook = nameof(OnLookTypeChanged))]
        private PlayerLookType _lookType;

        [SyncVar(hook = nameof(OnPositionChanged))]
        private float _position;

        [SyncVar]
        public int laneNumber;

        private float _speed;
        
        /// <summary>
        /// 順位
        /// </summary>
        [SyncVar]
        public int rank;

        [SerializeField]
        private PlayerLookManager playerLookManager;

        private void OnLookTypeChanged(PlayerLookType _, PlayerLookType newLookType)
        {
            playerLookManager.ChangeLookType(newLookType);
        }

        [SerializeField] private Canvas statusPlate;
        [SerializeField] private TMP_Text nameTextField;

        public CinemachineDollyCart Cart => _cart;

        private CinemachineDollyCart _cart;
        private Transform _mainCamera;

        private readonly float _speedUpPerTap = 3f;
        private readonly float _slowDownMultiplier = 0.99f;

        private void Start()
        {
            _cart = GetComponent<CinemachineDollyCart>();

            if (_cart.m_Path == null)
            {
                _cart.m_Path = FindObjectOfType<CinemachineSmoothPath>();
            }

            statusPlate.transform.forward = _mainCamera.forward;
            nameTextField.text = playerName;
        }
        
        private void OnPositionChanged(float _, float newPosition)
        {
            _cart.m_Position = newPosition;
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Racing)
            {
                UpdatePosition();
                SetStatusPlate();
            }
        }

        private void SetStatusPlate()
        {
            nameTextField.text = $"{rank}. {playerName}";
        }
        
        /// <summary>
        /// 加速させる
        /// </summary>
        [Command(requiresAuthority = false)]
        public void CmdAccelerate()
        {
            RpcAccelerate();
        }
        
        /// <summary>
        /// サーバーでのみ実行
        /// </summary>
        [Server]
        [ClientRpc]
        private void RpcAccelerate()
        {
            _speed += _speedUpPerTap;
        }

        [Server]
        private void UpdatePosition()
        {
            // 毎フレーム一定割合で減速する
            _speed *= _slowDownMultiplier;
            _position += _speed * Time.deltaTime;
        }
    }
}