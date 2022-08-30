using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(Animator))]

// This Component needs to be controlled by PlayerController or EnemyController

public class CharacterController : MonoBehaviour
{
    public string displayName = "Horse (CP)";
    public float position = 0;
    public int rank = 0;
    public Texture customTexture;
    public Shader customShader;

    [SerializeField] private GameObject customTextureBase;
    [SerializeField] private Canvas statusPlate;
    [SerializeField] private TMP_Text nameTextField;

    private CinemachineDollyCart _dollyCart;
    private Transform _mainCamera;
    
    private float _speedUpPerTap = 3f;
    private float _slowDownMultiplier = 0.99f;

    public RaceManager raceManager;

    public bool IsPlayer { get; set; } = false;

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out raceManager);
        raceManager.Characters.Add(this);

        TryGetComponent(out _dollyCart);
        _mainCamera = Camera.main.transform;

        if (customTextureBase != null && customTextureBase != null && customTexture != null)
        {
            SetCustomTexture(customTexture, customShader);
        }

        statusPlate.transform.forward = _mainCamera.forward;
        nameTextField.text = displayName;
    }

    private void Update()
    {
        if (raceManager.RaceStarted)
        {
            // Decrease speed every frame
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
        _dollyCart.m_Speed += _speedUpPerTap;
    }
}
