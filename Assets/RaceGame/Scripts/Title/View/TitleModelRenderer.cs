using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace RaceGame.Title.View
{
    public class TitleModelRenderer : MonoBehaviour
    {
        [SerializeField] private ModelType modelType;
        [SerializeField] private Light spotLight;
        [SerializeField] private Transform targetSingle;
        [SerializeField] private Transform targetMultiple;

        public float speed = 1f;

        public enum ModelType
        {
            Solo,
            Multi
        }

        public ModelType CurrentModelType
        {
            set
            {
                modelType = value;
                ChangeSpotLightDirection();
            }
            get => modelType;
        }

        private void OnValidate()
        {
            ChangeSpotLightDirection();
        }
        
        private void ChangeSpotLightDirection()
        {
            switch (modelType)
            {
                case ModelType.Solo:
                    spotLight.transform.rotation = Quaternion.Euler(30f, -25f, 0f);
                    break;
                case ModelType.Multi:
                    spotLight.transform.rotation = Quaternion.Euler(30f, 25f, 0f);
                    break;
                default: break;
            }
        }

        private void Update()
        {
            switch (modelType)
            {
                case ModelType.Solo:
                    targetSingle.transform.Rotate(0f, 0.2f * speed, 0f);
                    break;
                case ModelType.Multi:
                    targetMultiple.transform.Rotate(0f, 0.2f * speed, 0f);
                    break;
                default: break;
            }
        }
    }
}