using System.Linq;
using Cinemachine;
using Mirror;
using RaceGame.Race.Interface;
using TMPro;
using UnityEngine;
using Zenject;

namespace RaceGame.Race.Network
{
    /// <summary>
    /// プレイヤーの情報を管理するクラス
    /// PlayerまたはEnemyから操作する
    /// Positionの計算を行うのはサーバーのみ
    /// </summary>
    [RequireComponent(typeof(CinemachineDollyCart))]
    [RequireComponent(typeof(Animator))]
    public class Player : NetworkBehaviour, IPlayer
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

        public bool IsLocalPlayer => isLocalPlayer;
        public float Speed { get; private set; }
        
        /// <summary>
        /// 順位
        /// </summary>
        [SyncVar]
        public int rank;

        [SerializeField]
        private PlayerLookManager playerLookManager;
        
        [Inject] private IRaceManager _raceManager;

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
        
        public int xPosition;

        private void Start()
        {
            _cart = GetComponent<CinemachineDollyCart>();

            if (_cart.m_Path == null)
            {
                _cart.m_Path = FindObjectOfType<CinemachineSmoothPath>();
            }

            _lookType = PlayerLookType.Horse;

            _mainCamera = Camera.main.transform;
            OnLookTypeChanged(_lookType, _lookType);
            nameTextField.text = playerName;

            // 急ぎで雑なやり方
            // 本来であればFactoryPattern等で対応する
            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }
            _raceManager.AddPlayer(this);
            playerName = $"{playerName} ID : {netId}";
        }

        private void OnPositionChanged(float _, float newPosition)
        {
            _position = newPosition;
        }

        private void Update()
        {
            _cart.m_Position = _position;
            SetStatusPlate();
            if (_raceManager.RaceState == RaceState.Racing)
            {
                UpdatePosition();
            }
        }

        private void LateUpdate()
        {
            _raceManager.Players.OrderBy(x => x.netId).Select((x, index) => x.xPosition = index).ToArray();
            transform.position += transform.right * xPosition;
        }

        private void SetStatusPlate()
        {
            nameTextField.text = $"{rank}. {playerName}";
            statusPlate.transform.forward = _mainCamera.forward;
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
            Speed += _speedUpPerTap;
        }

        [Server]
        private void UpdatePosition()
        {
            // 毎フレーム一定割合で減速する
            Speed *= _slowDownMultiplier;
            _position += Speed * Time.deltaTime;
        }
    }
}