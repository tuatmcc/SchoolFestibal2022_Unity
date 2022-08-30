using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    private RaceManager _rManager;
    private CustomInputAction _customInput;
    private CharacterController _character;

    private void Awake()
    {
        _customInput = new CustomInputAction();
        _customInput.Enable();
    }

    private void Start()
    {
        SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0].TryGetComponent(out _rManager);
        TryGetComponent(out _character);

        // Set Player Name
        _character.displayName = _rManager.PlayerDisplayName;
        _character.IsPlayer = true;
    }

    private void Update()
    {
        if (_rManager.RaceStarted)
        {
            if (_customInput.Player.Accelerate.WasPerformedThisFrame())
            {
                _character.Accelerate();
            }
        }
    }
}
