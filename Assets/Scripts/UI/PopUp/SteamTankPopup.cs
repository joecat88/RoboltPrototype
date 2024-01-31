using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SteamTankPopup : MonoBehaviour
{
    public RoboltController roboltController;
    public SteamTankPopupManager popUpManager;
    public Image popup;
    public Image fillCircle;
    public Animator popupAnimator;
    private bool isActivePopUp;

    [SerializeField] protected InputActionReference triangle;

    private static readonly int popUpOnHashID = Animator.StringToHash("PopUpOn");

    // Start is called before the first frame update
    void Start()
    {
        isActivePopUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivePopUp)
        {
            // se il player sta premendo il bottone, inizio a riempiere il cerchio
            if (triangle.action.ReadValue<float>() > 0)
            {
                fillCircle.fillAmount += 1.0f / Const.FILL_BUTTON_DURATION * Time.deltaTime;
            }

            // altimenti inizio a svuotare il cerchio
            else
            {
                fillCircle.fillAmount -= 1.0f / Const.FILL_BUTTON_DURATION * Time.deltaTime;
            }

            // quando il cerchio è stato riempito
            if (fillCircle.fillAmount >= 1f)
            {
                roboltController.SetIsStoppedPlayerSessions(false);
                popUpManager.PopUpOff();
            }
        }
    }

    public void SetPopUpActive()
    {
        roboltController.SetIsStoppedPlayerSessions(true);
        isActivePopUp = true;
    }

    public void ActivatePopupAnimation(bool condition)
    {
        isActivePopUp = false;
        popup.enabled = condition;
        fillCircle.enabled = condition;
        popupAnimator.SetBool(popUpOnHashID, condition);
    }
}
