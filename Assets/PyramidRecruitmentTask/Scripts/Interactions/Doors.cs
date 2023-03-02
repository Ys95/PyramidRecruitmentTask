using System.Collections.Generic;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class Doors : InteractableObject
    {
        protected override void HandleInteraction()
        {
            if (FindObjectOfType<Player>().OwnedKeys <= 0)
            {
                ShowNoKeyMessage();
                return;
            }
            
            var popupCon = FindObjectOfType<PopupWindow>();

            var popupOptions = new List<PopupWindow.PopupOptionsInfo>()
            {
                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName = "Yes",
                    P_OptionSelectionAction = () =>
                    {
                        popupCon.ClosePopup();
                        OpenDoors();
                    }
                },

                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName            = "No",
                    P_OptionSelectionAction = () => { popupCon.ClosePopup(); }
                }
            };

            popupCon.CreateInteractionPopup("Open?", popupOptions, transform.position);
        }

        private void ShowNoKeyMessage()
        {
            var popupCon = FindObjectOfType<PopupWindow>();
            var popupOptions = new List<PopupWindow.PopupOptionsInfo>()
            {
                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName = "Ok",
                    P_OptionSelectionAction = () =>
                    {
                        popupCon.ClosePopup();
                    }
                }
            };
            
            popupCon.CreateInteractionPopup("You need a key!", popupOptions, transform.position);
        }
        
        private void OpenDoors()
        {
            Debug.Log("Doors opened!");
            Destroy(gameObject);
        }
    }
}