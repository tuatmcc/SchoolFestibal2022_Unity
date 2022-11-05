using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Race.UI.Parts
{
    public class RealTimeRankingView : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private TMP_Text rank;
        
        public void SetText(string text)
        {
            rank.text = text;
        }
        
        public void SetTextColor(Color color)
        {
            rank.faceColor = color;
        }
        
        public void SetTexture(Texture texture)
        {
            rawImage.texture = texture;
        }
    }
}
