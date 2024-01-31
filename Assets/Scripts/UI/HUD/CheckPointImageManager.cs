using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointImageManager : MonoBehaviour
{
    // references
    public Image checkPointImage;

    public void EnableCheckPointImage()
    {
        checkPointImage.enabled = true;
    }

    public void DisableCheckPointImage()
    {
        Debug.Log("false");
        checkPointImage.enabled = false;
    }


}
