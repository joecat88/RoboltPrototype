using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInSceneManager : MonoBehaviour
{
    public RoboltController roboltController;

    public void StartSession()
    {
        roboltController.SetIsStoppedPlayerSessions(false);
    }
}
