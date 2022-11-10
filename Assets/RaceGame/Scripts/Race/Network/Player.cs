using System;
using System.Linq;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Mirror;
using RaceGame.Race.Interface;
using RaceGame.Race.Misc;
using RaceGame.Race.UI.Parts;
using RaceGame.Title;
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
    // CinemachineDollyCartより後に実行したい
    [DefaultExecutionOrder(1)]
    public class Player : NetworkBehaviour, IPlayer
    {
        [SerializeField] private NamePlate namePlate;

        public TextureData TextureData;
        
        public float Position => _position;
        public TimeSpan GoalTime => new(_goalTime - _startTime);
        
        [SyncVar] [NonSerialized]
        private long _startTime;
        [SyncVar] [NonSerialized]
        private long _goalTime;
        [SyncVar] [NonSerialized] public bool IsGoal;

        public int ClickCount { get; private set; }

        public void Goal()
        {
            if (IsGoal) return;
            _goalTime = DateTime.Now.Ticks;
            IsGoal = true;
        }

        public void OnStart()
        {
            _startTime = DateTime.Now.Ticks;
        }

        [SyncVar(hook = nameof(OnPositionChanged))]
        private float _position;

        [SyncVar]
        public int laneNumber;

        [SyncVar(hook = nameof(OnPlayerIDChanged))]
        [NonSerialized]
        public long PlayerID = -1;

        public bool IsLocalPlayer => isLocalPlayer;
        public float Speed { get; private set; }
        
        /// <summary>
        /// 順位
        /// </summary>
        [SyncVar]
        public int rank;

        [SerializeField]
        public PlayerLookManager playerLookManager;
        
        [Inject] private IRaceManager _raceManager;
        
        private void OnPlayerIDChanged(long _, long newPlayerID)
        {
            Debug.Log($"{nameof(OnPlayerIDChanged)} : {_} -> {newPlayerID}");
            DownloadTextures(newPlayerID, this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        private async UniTaskVoid DownloadTextures(long localPlayerID, CancellationToken cancellationToken)
        {
            TextureData = await TextureDownloader.DownloadPlayerTexture(localPlayerID, cancellationToken);
            playerLookManager.SetCustomTexture(TextureData.Texture, TextureData.PlayerLookType);
            namePlate.SetTexture(TextureData.Texture);
        }

        [SerializeField] private Canvas statusPlate;

        public CinemachineDollyCart Cart => _cart;

        private CinemachineDollyCart _cart;
        private Transform _mainCamera;

        private readonly float _speedUpPerTap = 2f;
        private readonly float _slowDownMultiplier = 1 - 3 / 100f;
        
        public int xPosition;

        private void Start()
        {
            _cart = GetComponent<CinemachineDollyCart>();

            if (_cart.m_Path == null)
            {
                _cart.m_Path = FindObjectOfType<LaneEdgeGenerator>().GetComponent<CinemachineSmoothPath>();
            }

            _mainCamera = Camera.main.transform;

            // 急ぎで雑なやり方
            // 本来であればFactoryPattern等で対応する
            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }
            
            _raceManager.OnRaceStart += OnStart;
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            
            if (_raceManager == null)
            {
                _raceManager = FindObjectOfType<RaceManager>();
            }

            _raceManager.AddPlayer(this);
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
            _raceManager.Players.OrderBy(x => x.netId).Select((x, index) => x.xPosition = index).ToArray();
            transform.position += transform.right * (xPosition * 1.5f + 0.5f);
        }

        private void FixedUpdate()
        {
            UpdateSpeed();
        }

        private void SetStatusPlate()
        {
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
            if (!IsGoal)
            {
                ClickCount++;
            }
        }

        [Server]
        private void UpdatePosition()
        {
            _position += Speed * Time.deltaTime;
        }

        [Server]
        private void UpdateSpeed()
        {
            // 毎フレーム一定割合で減速する
            Speed *= _slowDownMultiplier;
        }
    }
}