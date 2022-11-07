using UnityEngine;
using UnityEngine.UI;

namespace RaceGame.Race.UI.Parts
{
    public class RealTimeRankingView : MonoBehaviour
    {
        [SerializeField] private RawImage rankImage;
        [SerializeField] private RawImage rawImage;
        
        [SerializeField] private Texture[] rankTextures;
        
        public void SetRank(int rank)
        {
            if (rank == 0) rank = 1;
            rankImage.texture = rankTextures[rank - 1];
        }
        
        public void SetNamePlateTexture(Texture texture)
        {
            // rawImage.texture = texture;
        }
    }
}
