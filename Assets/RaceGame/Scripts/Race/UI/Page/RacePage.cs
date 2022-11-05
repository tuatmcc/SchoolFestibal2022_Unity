using RaceGame.Core.UI;
using UnityEngine;

namespace RaceGame.Race.UI.Page
{
    public class RacePage : MonoBehaviour, IPage
    {
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
