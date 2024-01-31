using UnityEngine.EventSystems;
using UnityEngine;

public class EnableAudioListener : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        AudioListener.pause = false;
    }
}
