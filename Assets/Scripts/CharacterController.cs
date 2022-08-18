using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(Animator))]

// This Component needs to be controlled by PlayerController or EnemyController

public class CharacterController : MonoBehaviour
{
    public string DisplayName = "UMA";
    public float Position = 0;
    public int Rank = 0;
    public Texture CustomTexture;
    public Shader CustomShader;

    [SerializeField] private GameObject CustomTextureBase;
    [SerializeField] private Canvas StatusPlate;
    [SerializeField] private TMP_Text NameTextField;

    private CinemachineDollyCart DollyCart;
    private Transform MainCam;
    private float SpeedUpPerTap = 5f;
    private float SlowDownMultipler = 0.99f;

    private void Start()
    {
        TryGetComponent(out DollyCart);
        MainCam = Camera.main.transform;

        if (CustomTexture && CustomTextureBase && CustomTextureBase)
        {
            SetCustomTexture(CustomTexture, CustomShader);
        }
    }

    private void Update()
    {
        // Decrease speed every frame
        DollyCart.m_Speed *= SlowDownMultipler;
        Position = DollyCart.m_Position;

        SetStatusPlate();
    }

    private void SetStatusPlate()
    {
        StatusPlate.transform.forward = MainCam.forward;
        NameTextField.text = Rank + ". " + DisplayName;
    }

    public void SetCustomTexture(Texture customTexture, Shader customShader)
    {
        Material mat = CustomTextureBase.GetComponent<Renderer>().material;
        mat.shader = customShader;
        mat.mainTexture = customTexture;
    }

    public void Accelerate()
    {
        DollyCart.m_Speed += SpeedUpPerTap;
    }
}
