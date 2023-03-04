using System.Collections.Generic;
using PyramidRecruitmentTask.Player;
using PyramidRecruitmentTask.Signals;
using UnityEngine;

namespace PyramidRecruitmentTask.Interactions
{
    public class Doors : InteractableObject
    {
        protected override void HandleInteraction(PlayerInteraction playerInteraction)
        {
            if (playerInteraction.P_OwnedKeys <= 0)
            {
                ShowNoKeyMessage();
                return;
            }
            
            List<PopupWindow.PopupOptionsInfo> popupOptions = new()
            {
                new()
                {
                    P_OptionName            = "Yes",
                    P_OptionSelectionAction = OpenDoors
                },

                new()
                {
                    P_OptionName = "No"
                }
            };

            _signalBus.Fire(new ShowInteractionPopupSignal("Open?", popupOptions, transform.position));
        }

        private void ShowNoKeyMessage()
        {
            var popupCon = FindObjectOfType<PopupWindow>();
            List<PopupWindow.PopupOptionsInfo> popupOptions = new()
            {
                new()
                {
                    P_OptionName            = "Ok",
                    P_OptionSelectionAction = () => { popupCon.ClosePopup(); }
                }
            };

            _signalBus.Fire(new ShowInteractionPopupSignal("You need a key!", popupOptions, transform.position));
        }

        private void OpenDoors()
        {
            _signalBus.Fire<DoorOpenedSignal>();
        }
    }
}