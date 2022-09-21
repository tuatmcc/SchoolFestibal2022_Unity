using Cinemachine;
using UnityEngine;
using TMPro;

using RaceGame.Manager;
using RaceGame.Constant;


namespace RaceGame.Players
{
    /// <summary>
    /// 各キャラクターの情報を持つクラス。PlayerまたはEnemyから操作する。
    /// </summary>
    [RequireComponent(typeof(CinemachineDollyCart))]
    [RequireComponent(typeof(Animator))]
    public class Character : MonoBehaviour
    {
        
        public string displayName = "Horse (CP)";
        public float position = 0;
        public int rank = 0;
        public Texture customTexture;
        public Shader customShader;

        /// <summary>
        /// カスタムテクスチャに変える部分のオブジェクト
        /// </summary>
        [SerializeField] private GameObject customTextureBase;
        [SerializeField] private Canvas statusPlate;
        [SerializeField] private TMP_Text nameTextField;

        private CinemachineDollyCart _dollyCart;
        private Transform _mainCamera;

        private readonly float _speedUpPerTap = 3f;
        private readonly float _slowDownMultiplier = 0.99f;
        
        public bool IsPlayer { get; set; } = false;

        private void Start()
        {
            _dollyCart = GetComponent<CinemachineDollyCart>();
            _mainCamera = Camera.main.transform;

            if (_dollyCart.m_Path == null)
            {
                _dollyCart.m_Path = Transform.FindObjectOfType<CinemachineSmoothPath>();
            }

            if (customTextureBase != null && customTextureBase != null && customTexture != null)
            {
                // 敵はデフォルトのテクスチャでもよい. テクスチャが指定されているCharacterのみ実行
                SetCustomTexture(customTexture, customShader);
            }

            statusPlate.transform.forward = _mainCamera.forward;
            nameTextField.text = displayName;
        }

        private void Update()
        {
            if (RaceManager.Instance.CurrentRaceState == RaceState.Started)
            {
                // 毎フレーム一定割合で減速する
                _dollyCart.m_Speed *= _slowDownMultiplier;
                position = _dollyCart.m_Position;

                SetStatusPlate();
            }
        }

        private void SetStatusPlate()
        {
            statusPlate.transform.forward = _mainCamera.forward;
            nameTextField.text = rank + ". " + displayName;
        }

        public void SetCustomTexture(Texture customTexture, Shader customShader)
        {
            var mat = customTextureBase.GetComponent<Renderer>().material;
            mat.shader = customShader;
            mat.mainTexture = customTexture;
        }

        public void Accelerate()
        {
            if (RaceManager.Instance.CurrentRaceState != RaceState.Started) return;
            _dollyCart.m_Speed += _speedUpPerTap;
        }
    }
}