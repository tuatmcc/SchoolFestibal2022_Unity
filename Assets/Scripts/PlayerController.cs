using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    public string DisplayName;
    // 0: Horese, 1: Car, 2: Human
    public int ModelTypeIndex = 0;
    public Texture CustomTexture;
    public Shader CustomShader;
    [SerializeField] private GameObject[] CustomTextureBases;
    [SerializeField] private GameObject[] Models;

    private PlayerInputAction PlayerInput;
    private CinemachineDollyCart DollyCart;
    private float SpeedUpPerTap = 5f;
    private float SlowDownMultipler = 0.99f;

    private void Awake()
    {
        PlayerInput = new PlayerInputAction();
        PlayerInput.Enable();
    }

    private void Start()
    {
        TryGetComponent(out DollyCart);

        // Make only the Selected Model Type active
        for (int i=0; i < Models.Length; i++)
        {
            Models[i].SetActive(i == ModelTypeIndex);
        }
        SetCustomTexture(CustomTexture, CustomShader);
    }

    private void Update()
    {
        if (PlayerInput.Player.Accelerate.WasPerformedThisFrame())
        {
            DollyCart.m_Speed += SpeedUpPerTap;
        }

        // Decrease speed every frame
        DollyCart.m_Speed *= SlowDownMultipler;
    }

    public void SetCustomTexture(Texture customTexture, Shader customShader)
    {
        Material mat = CustomTextureBases[ModelTypeIndex].GetComponent<Renderer>().material;
        mat.shader = customShader;
        mat.mainTexture = customTexture;
    }
}
