using System.Collections.Generic;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class Key : InteractableObject
    {
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
                        PickupKey();
                    }
                },

                new PopupWindow.PopupOptionsInfo()
                {
                    P_OptionName            = "No",
                    P_OptionSelectionAction = () => { popupCon.ClosePopup(); }
                }
            };

            popupCon.CreateInteractionPopup("Take?", popupOptions, transform.position);
        }

        private void PickupKey()
        {
            var player = FindObjectOfType<Player>();
            player.AddKey();
            Destroy(gameObject);
        }
    }
}