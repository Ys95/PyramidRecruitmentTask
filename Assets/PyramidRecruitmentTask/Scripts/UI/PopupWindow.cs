using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PyramidRecruitmentTask
{
    public class PopupWindow : MonoBehaviour
    {
        public class PopupOptionsInfo
        {
            public string      P_OptionName            { get; set; }
            public UnityAction P_OptionSelectionAction { get; set; }
        }

        [SerializeField] private GameObject      _interactionPopupObject;
        [SerializeField] private TextMeshProUGUI _titleTmp;
        [SerializeField] private Transform       _popupOptionsContainer;
        [SerializeField] private GameObject      _popupOptionPrefab;
        
        public void CreateInteractionPopup(string titleText, List<PopupOptionsInfo> popupOptions, Vector3 objectWorldPosition)
        {
            ResetCurrentPopup();
            RepositionPopup(objectWorldPosition);
            _interactionPopupObject.SetActive(true);

            _titleTmp.text = titleText;

            foreach (var popupOption in popupOptions)
            {
                var optionObj = Instantiate(_popupOptionPrefab, _popupOptionsContainer);
                
                var tmp       = optionObj.GetComponentInChildren<TextMeshProUGUI>();
                var btn       = optionObj.GetComponent<Button>();

                tmp.text = popupOption.P_OptionName;
                btn.onClick.AddListener(popupOption.P_OptionSelectionAction);
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
                ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
                ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));
            _interactionPopupObject.GetComponent<RectTransform>().anchoredPosition = screenPosition;
        }

        private void ResetCurrentPopup()
        {
            foreach (Transform child in _popupOptionsContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}