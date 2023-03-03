using PyramidRecruitmentTask.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace PyramidRecruitmentTask
{
    public class PopupWindow : MonoBehaviour
    {
        [SerializeField] private GameObject      _interactionPopupObject;
        [SerializeField] private TextMeshProUGUI _titleTmp;
        [SerializeField] private Transform       _popupOptionsContainer;
        [SerializeField] private GameObject      _popupOptionPrefab;

        [Inject] private SignalBus _signalBus;

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
            RepositionPopup(signal.P_WorldPosition);
            _interactionPopupObject.SetActive(true);

            _titleTmp.text = signal.P_PopupWindowName;

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
        }

        public void ClosePopup()
        {
            _interactionPopupObject.SetActive(false);
        }

        private void RepositionPopup(Vector3 worldPosition)
        {
            var rect             = GetComponent<RectTransform>();
            var viewportPosition = Camera.main.WorldToViewportPoint(worldPosition);
            var sizeDelta        = rect.sizeDelta;

            var screenPosition = new Vector2(
                viewportPosition.x * sizeDelta.x - sizeDelta.x * 0.5f,
                viewportPosition.y * sizeDelta.y - sizeDelta.y * 0.5f);
            _interactionPopupObject.GetComponent<RectTransform>().anchoredPosition = screenPosition;
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