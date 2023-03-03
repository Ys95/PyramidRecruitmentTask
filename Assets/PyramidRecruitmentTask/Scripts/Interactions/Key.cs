using System.Collections.Generic;
using PyramidRecruitmentTask.Player;
using PyramidRecruitmentTask.Signals;

namespace PyramidRecruitmentTask.Interactions
{
    public class Key : InteractableObject
    {
        protected override void HandleInteraction(PlayerInteraction playerInteraction)
        {
            List<PopupWindow.PopupOptionsInfo> popupOptions = new List<PopupWindow.PopupOptionsInfo>
            {
                new()
                {
                    P_OptionName            = "Yes",
                    P_OptionSelectionAction = () => { PickupKey(playerInteraction); }
                },

                new()
                {
                    P_OptionName = "No"
                }
            };

            _signalBus.Fire(new ShowInteractionPopupSignal("Take?", popupOptions, transform.position));
        }

        private void PickupKey(PlayerInteraction playerInteraction)
        {
            playerInteraction.AddKey();
            gameObject.SetActive(false);
        }
    }
}