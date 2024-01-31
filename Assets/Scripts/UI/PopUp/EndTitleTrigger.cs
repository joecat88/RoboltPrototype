using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTitleTrigger : MonoBehaviour
{
    public EndTitleManager endTitleManager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            endTitleManager.ActivateEndTitle();
        }
    }
}
