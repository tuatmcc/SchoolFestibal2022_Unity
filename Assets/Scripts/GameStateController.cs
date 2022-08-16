using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject Player;
    public Texture PlayerTexture;

    [SerializeField] private GameObject[] Prefabs;
    [SerializeField] private string HorseTexturesFolderPath;
    private 
    
    void Start()
    {
        Material PlayerMat = Player.GetComponent<Renderer>().material;
        PlayerMat.mainTexture = PlayerTexture;

        
    }

    void Update()
    {
        
    }
}
