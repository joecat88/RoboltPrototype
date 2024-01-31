using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamTankPopupManager : MonoBehaviour, PopUpInterface
{
    //public RoboltInteractions interaction;
    public SteamTankPopup steamTankPopup;

    public void PopUpOn() 
    {
        steamTankPopup.ActivatePopupAnimation(true);
    }

    public void PopUpOff()
    {
        steamTankPopup.ActivatePopupAnimation(false);
    }
}
