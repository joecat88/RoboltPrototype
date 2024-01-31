using UnityEngine.EventSystems;
using UnityEngine;

public class DisableFirstSelectedSound : MonoBehaviour, ISelectHandler
{
    bool firstSelected;

    void Awake()
    {
        //AudioListener.pause = true;
        //firstSelected = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //if (firstSelected)
        //    firstSelected = false;   
        //else
        //    AudioListener.pause = false;
    }

    // da chimare quando viene chiuso un menu, per evitare il primo suono alla prossima riapertura
    public void ResetAudioListener()
    {
        //AudioListener.pause = true;
        //firstSelected = true;
    }
}
