using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        //QualitySettings.vSyncCount = 0; // sovrascrive l'opzione VSync Count nel project setting, per limitare a 60 fps
        //Application.targetFrameRate = 60;
    }


}
