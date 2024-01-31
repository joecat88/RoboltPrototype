using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Classe per gestire il count down dei prefab Gear verticali */
public class GearManagerVert : MonoBehaviour
{
    public void SetTotSec(float totSec)
    {

        transform.GetChild(0).GetChild(0).GetComponent<GearVertical>().SetTotSec(totSec);
    }

    public void TriggerCountDown()
    {
        transform.GetChild(0).GetChild(0).GetComponent<GearVertical>().TriggerCountDown();
    }
}
