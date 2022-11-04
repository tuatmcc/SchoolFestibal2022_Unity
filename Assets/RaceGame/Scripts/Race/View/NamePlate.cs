using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Race.View
{
    [RequireComponent(typeof(RawImage))]
    public class NamePlate : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;

        public void SetTexture(Texture texture)
        {
            rawImage.texture = texture;
        }
    }
}
