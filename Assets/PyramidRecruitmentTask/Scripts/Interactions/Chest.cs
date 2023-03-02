using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PyramidRecruitmentTask
{
    public class Chest : InteractableObject
    {
        [SerializeField] private GameObject _chestContent;

        private bool _isOpened;
        
        protected override void HandleInteraction()
        {
            var popupCon = FindObjectOfType<PopupWindow>();

            var popupOptions = new List<PopupWindow.PopupOptionsInfo>()
            {
                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName = "Yes",
                    P_OptionSelectionAction = () =>
                    {
                        popupCon.ClosePopup();
                        OpenChest();
                    }
                },
                
                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName = "No",
                    P_OptionSelectionAction = () =>
                    {
                        popupCon.ClosePopup();
                    }
                }
            };
            
            popupCon.CreateInteractionPopup("Open?", popupOptions, transform.position);
        }

        private void OpenChest()
        {
            Debug.Log("Chest opened!");
            HandlePointerExit();
            _isOpened             = true;
            _pointerEventsAllowed = false;
            _chestContent.gameObject.SetActive(true);
        }
    }
}