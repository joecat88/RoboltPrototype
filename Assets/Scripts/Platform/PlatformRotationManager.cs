using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotationManager : MonoBehaviour
{
    private RotateY rotateScript;
    // Start is called before the first frame update
    void Start()
    {
        rotateScript = GetComponent<RotateY>();
        rotateScript.enabled = true;
    }

}
