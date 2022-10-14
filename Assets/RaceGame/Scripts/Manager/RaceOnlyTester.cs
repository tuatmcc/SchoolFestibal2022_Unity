using Mirror;
using UnityEngine;

namespace RaceGame.Manager
{
    public class RaceOnlyTester : MonoBehaviour
    {
        private void Start()
        {
        
            if (!ParrelSync.ClonesManager.IsClone())
            {
                NetworkManager.singleton.StartHost();
            }
            else
            {
                NetworkManager.singleton.StartClient();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
