using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using PyramidRecruitmentTask.Feedbacks;
using PyramidRecruitmentTask.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace PyramidRecruitmentTask
{
    public class PopupWindow : MonoBehaviour
    {
        [Header("Feedbacks")]
        [SerializeField] private FeedbacksPlayer _windowOpenFeedback;
        [SerializeField] private FeedbacksPlayer _optionSelectFeedback;

        [Space]
        [SerializeField] private RectTransform _moveableTransform;
        [SerializeField] private RectTransform   _interactionPopupContent;
        [SerializeField] private TextMeshProUGUI _titleTmp;
        [SerializeField] private Transform       _popupOptionsContainer;
        [SerializeField] private GameObject      _popupOptionPrefab;

        [Inject] private SignalBus _signalBus;

        private Camera                                        _mainCamera;
        private Canvas                                        _parentCanvas;
        private RectTransform                                 _canvasRect;
        private TweenerCore<Vector3, Vector3, VectorOptions> _currentTween;

        private void OnEnable()
        {
            _signalBus.Subscribe<ShowInteractionPopupSignal>(CreateInteractionPopup);
            _signalBus.Subscribe<HideInteractionPopupSignal>(ClosePopup);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<ShowInteractionPopupSignal>(CreateInteractionPopup);
            _signalBus.TryUnsubscribe<HideInteractionPopupSignal>(ClosePopup);
        }

        private void CreateInteractionPopup(ShowInteractionPopupSignal signal)
        {
            ResetCurrentPopup();
            _interactionPopupContent.gameObject.SetActive(true);

            _titleTmp.text = signal.P_PopupWindowName;
            
            _windowOpenFeedback?.Play();

            foreach (var popupOption in signal.P_Options)
            {
                var optionObj = Instantiate(_popupOptionPrefab, _popupOptionsContainer);

                var tmp = optionObj.GetComponentInChildren<TextMeshProUGUI>();
                var btn = optionObj.GetComponent<Button>();

                tmp.text = popupOption.P_OptionName;
                if (popupOption.P_OptionSelectionAction != null)
                {
                    btn.onClick.AddListener(popupOption.P_OptionSelectionAction);
                }

                btn.onClick.AddListener(ClosePopup);
            }
            
            RepositionPopup(signal.P_WorldPosition);

            if (_currentTween != null)
            {
                _currentTween.Kill();
            }

            _moveableTransform.localScale = Vector3.zero;
            _currentTween                 = _moveableTransform.DOScale(Vector3.one, 0.2f);
            _currentTween.SetEase(Ease.InOutQuint);
        }

        public void ClosePopup()
        {
            _optionSelectFeedback?.Play();
            
            if (_currentTween != null)
            {
                _currentTween.Kill();
            }

            _currentTween = _moveableTransform.DOScale(Vector3.zero, 0.2f);
            _currentTween.SetEase(Ease.InOutQuint);
            _currentTween.onComplete += () =>
            {
                _interactionPopupContent.gameObject.SetActive(false);
            };
        }

        private void RepositionPopup(Vector3 worldPosition)
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            if (_parentCanvas == null)
            {
                _parentCanvas = GetComponentInParent<Canvas>();
                _canvasRect   = _parentCanvas.GetComponent<RectTransform>();
            }

            var mousePos = Mouse.current.position.ReadValue();
            _moveableTransform.position = new Vector2(mousePos.x + 5f, mousePos.y + 5f);
            Vector3 newPosition = _moveableTransform.localPosition;

            Vector3 minPosition = _canvasRect.rect.min - _moveableTransform.rect.min;
            Vector3 maxPosition = _canvasRect.rect.max - _moveableTransform.rect.max;

            newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

            _moveableTransform.localPosition = newPosition;
        }

        private void ResetCurrentPopup()
        {
            foreach (Transform child in _popupOptionsContainer)
            {
                Destroy(child.gameObject);
            }
        }

        public class PopupOptionsInfo
        {
            public string      P_OptionName            { get; set; }
            public UnityAction P_OptionSelectionAction { get; set; }
        }
    }
}