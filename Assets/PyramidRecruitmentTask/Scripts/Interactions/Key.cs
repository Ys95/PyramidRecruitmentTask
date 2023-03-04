using System.Collections.Generic;
using PyramidRecruitmentTask.Feedbacks;
using PyramidRecruitmentTask.Player;
using PyramidRecruitmentTask.Signals;
using UnityEngine;

namespace PyramidRecruitmentTask.Interactions
{
    public class Key : InteractableObject
    {
        [SerializeField] private FeedbacksPlayer _keyPickedUpFeedbacks;
        [SerializeField] private GameObject      _keyModel;
        
        protected override void HandleInteraction(PlayerInteraction playerInteraction)
        {
            List<PopupWindow.PopupOptionsInfo> popupOptions = new()
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
            _keyModel.SetActive(false);
            _collider.enabled = false;
            _keyPickedUpFeedbacks?.Play();
        }
    }
}