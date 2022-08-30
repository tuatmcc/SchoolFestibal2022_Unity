using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSceneManager : MonoBehaviour
{
    public GameObject startGameButton;
    public GameObject confirmPlayerNameButton;

    private SceneLoader _sceneLoadManager;
    private RaceManager _rManager;
    private CustomInputAction _customInput;
    private TMP_InputField _playerNameInputField;

    private void Start()
    {
        var rootGo = SceneManager.GetSceneByName(SceneNames.ManagerScene).GetRootGameObjects()[0];
        rootGo.TryGetComponent(out _sceneLoadManager);
        rootGo.TryGetComponent(out _rManager);
        TryGetComponent(out _playerNameInputField);

        _customInput = new CustomInputAction();
        _customInput.Enable();

    }

    private void Update()
    {
        if (_customInput.UI.Confirm.WasPerformedThisFrame())
        {
            _rManager.PlayerDisplayName = _playerNameInputField.text;
            confirmPlayerNameButton.SetActive(false);
        }

        if (_customInput.UI.LoadMainScene.WasPerformedThisFrame() &&
            !SceneManager.GetSceneByName(SceneNames.MainScene).isLoaded)
        {
            _sceneLoadManager.LoadScene(SceneNames.MainScene, SceneNames.TitleScene);
        }
    }
}
