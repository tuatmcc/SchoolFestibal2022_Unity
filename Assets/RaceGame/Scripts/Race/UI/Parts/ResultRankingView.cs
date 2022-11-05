using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Race.UI.Parts
{
    public class ResultRankingView : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private Text time;
        [SerializeField] private Text clickCount;

        public void SetTexture(Texture texture)
        {
            rawImage.texture = texture;
        }

        public void SetTimeText(string text)
        {
            time.text = text;
        }
        
        public void SetClickCountText(string text)
        {
            clickCount.text = text;
        }
    }
}