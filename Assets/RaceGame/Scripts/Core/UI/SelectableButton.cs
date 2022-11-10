using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RaceGame.Core.UI
{
    public class SelectableButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private bool selectOnAwake;
        
        [SerializeField] private Button button;
        [SerializeField] private Image selectedImage;
        
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip selectSound;
        [SerializeField] private AudioClip clickSound;

        public event Action OnClicked;
        public event Action OnSelected;

        private bool _isSelected;
        private bool _isFirstSelect = true;

        private void Awake()
        {
            if (selectOnAwake)
            {
                button.Select();
            }

            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            OnClicked?.Invoke();
            audioSource.PlayOneShot(clickSound);
            OnClickAnimation(this.GetCancellationTokenOnDestroy()).Forget();
            button.interactable = false;
        }
        
        private async UniTaskVoid OnClickAnimation(CancellationToken cancellationToken)
        {
            selectedImage.gameObject.SetActive(true);
            var minSize = 0.9f;
            var maxSize = 1.0f;
            
            for (var i = 0; i < 10; i++)
            {
                await UniTask.Delay(10, cancellationToken: cancellationToken);
                selectedImage.transform.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, i / 10f);
            }
            selectedImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            selectedImage.gameObject.SetActive(_isSelected);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _isSelected = true;
            if (!_isFirstSelect)
            {
                audioSource.PlayOneShot(selectSound);
            }
            _isFirstSelect = false;
            OnSelected?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _isSelected = false;
        }
    }
}
