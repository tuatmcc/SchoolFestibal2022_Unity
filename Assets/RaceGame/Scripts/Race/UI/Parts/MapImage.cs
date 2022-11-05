using System;
using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Race.View
{
    public class MapImage : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private RawImage maskImage;

        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }

        public void SetTexture(Texture texture)
        {
            rawImage.texture = texture;
        }
        
        public void SetMaskTexture(Texture texture)
        {
            maskImage.texture = texture;
        }
    }
}
