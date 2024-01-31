using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTranslationManager : MonoBehaviour
{
    private PlatformMovingFwBw translateScript;
    // Start is called before the first frame update
    void Start()
    {
        translateScript = GetComponent<PlatformMovingFwBw>();
        translateScript.enabled = true;
    }

}
