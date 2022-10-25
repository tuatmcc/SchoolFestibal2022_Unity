using UnityEngine;

namespace RaceGame.Race.Network
{
    public class PlayerLook : MonoBehaviour
    {
        public PlayerLookType PlayerLookType => playerLookType;
        
        [SerializeField]
        public PlayerLookType playerLookType;

        [SerializeField]
        private Renderer customTextureRenderer;

        public void SetCustomTexture(Texture texture)
        {
            customTextureRenderer.material.mainTexture = texture;
        }
    }
}