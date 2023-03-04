using System.Collections.Generic;
using PyramidRecruitmentTask.Feedbacks;
using PyramidRecruitmentTask.Player;
using PyramidRecruitmentTask.Signals;
using UnityEngine;

namespace PyramidRecruitmentTask.Interactions
{
    public class Chest : InteractableObject
    {
        [SerializeField] private FeedbacksPlayer _chestOpenedFeedback;
        
        [Space]
        [SerializeField] private GameObject _chestContent;

        private bool _isOpened;

        protected override void HandleInteraction(PlayerInteraction playerInteraction)
        {
            List<PopupWindow.PopupOptionsInfo> popupOptions = new()
            {
                new()
                {
                    P_OptionName            = "Yes",
                    P_OptionSelectionAction = OpenChest
                },

                new()
                {
                    P_OptionName = "No"
                }
            };

            _signalBus.Fire(new ShowInteractionPopupSignal("Open?", popupOptions, transform.position));
        }

        private void OpenChest()
        {
            HandlePointerExit();
            
            _isOpened             = true;
            _pointerEventsAllowed = false;
            _chestContent.gameObject.SetActive(true);
            _chestOpenedFeedback?.Play(transform.position);
        }
    }
}