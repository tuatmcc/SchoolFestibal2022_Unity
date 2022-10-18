using RaceGame.Core.Interface;
using UnityEngine.SceneManagement;

namespace RaceGame.Core
{
    public class RaceGameSceneManager : ISceneManager
    {
        public bool StartFromTitle { get; private set; }

        public void ToRace()
        {
            StartFromTitle = true;
            SceneManager.LoadScene(SceneName.Race.ToString());
        }

        public void ToTitle()
        {
            SceneManager.LoadScene(SceneName.Title.ToString());
        }
    }
}