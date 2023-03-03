using System.Collections.Generic;
using PyramidRecruitmentTask.Managers;
using PyramidRecruitmentTask.Player;
using UnityEngine;

namespace PyramidRecruitmentTask.Signals
{
    public class PlayerInteractionAttemptSignal
    {
        public PlayerInteractionAttemptSignal(PlayerInteraction playerInteraction)
        {
            P_PlayerInteraction = playerInteraction;
        }

        public PlayerInteraction P_PlayerInteraction { get; }
    }

    public class ShowInteractionPopupSignal
    {
        public ShowInteractionPopupSignal(string popupWindowName, List<PopupWindow.PopupOptionsInfo> popupOptionsInfos, Vector3 worldPosition)
        {
            P_PopupWindowName = popupWindowName;
            P_Options         = popupOptionsInfos;
            P_WorldPosition   = worldPosition;
        }

        public string                             P_PopupWindowName { get; }
        public List<PopupWindow.PopupOptionsInfo> P_Options         { get; }
        public Vector3                            P_WorldPosition   { get; }
    }

    public class HideInteractionPopupSignal
    {
    }

    public class DoorOpenedSignal
    {
    }

    public class TimerStartSignal
    {
        public TimerStartSignal(Timer timer)
        {
            P_Timer = timer;
        }

        public Timer P_Timer { get; }
    }

    public class TimerStopSignal
    {
        public TimerStopSignal(Timer timer)
        {
            P_Timer = timer;
        }

        public Timer P_Timer { get; }
    }

    public class UISignal
    {
        public enum SignalType
        {
            GameStartBTNClick = 0,
            TryAgainBTNClick  = 1,
            MainMenuBTNClick  = 2
        }

        public UISignal(SignalType signalType)
        {
            P_SignalType = signalType;
        }

        public SignalType P_SignalType { get; }
    }

    public class PlayAudioSignal
    {
        public PlayAudioSignal(AudioClip audioClip, Vector3 position)
        {
            P_AudioClip = audioClip;
            P_Position  = position;
        }

        public AudioClip P_AudioClip { get; }
        public Vector3   P_Position  { get; }
    }
}