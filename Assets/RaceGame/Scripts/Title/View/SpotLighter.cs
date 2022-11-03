using System;
using UnityEngine;

namespace RaceGame.Title.View
{
    public class SpotLighter : MonoBehaviour
    {
        [SerializeField] private Mode currentMode;
        [SerializeField] private Transform targetSingle;
        [SerializeField] private Transform targetMultiple;

        public enum Mode
        {
            SingleMode,
            MultipleMode
        }

        public Mode CurrentMode
        {
            set
            {
                currentMode = value;
                ChangeSpotLightDirection();
            }
        }

        private void OnValidate()
        {
            ChangeSpotLightDirection();
        }
        
        private void ChangeSpotLightDirection()
        {
            switch (currentMode)
            {
                case Mode.SingleMode:
                    transform.rotation = Quaternion.Euler(30f, -25f, 0f);
                    break;
                case Mode.MultipleMode:
                    transform.rotation = Quaternion.Euler(30f, 25f, 0f);
                    break;
                default: break;
            }
        }

        private void Update()
        {
            switch (currentMode)
            {
                case Mode.SingleMode:
                    targetSingle.transform.Rotate(0f, 0.2f, 0f);
                    break;
                case Mode.MultipleMode:
                    targetMultiple.transform.Rotate(0f, 0.2f, 0f);
                    break;
                default: break;
            }
        }
    }
}