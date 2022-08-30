using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CinemachineDollyCart))]
[RequireComponent(typeof(CharacterController))]

public class EnemyController : MonoBehaviour
{
    // Enemy Strength is dependent on these two parematers
    [Range(0f, 1f)] public float enemyTapFrequency = 0.01f;
    public float maxSpeedLimit = 30f;

    private RaceManager _rManager;
    private CharacterController _character;
    private CinemachineDollyCart _dollyCart;

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out _rManager);
        TryGetComponent(out _dollyCart);
        TryGetComponent(out _character);

    }

    private void Update()
    {
        if (_rManager.RaceStarted)
        {
            // Accelerate randomly
            var rand = Random.value;
            if (rand < enemyTapFrequency && _dollyCart.m_Speed < maxSpeedLimit)
            {
                _character.Accelerate();
            }
        }
    }
}
