using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Classe per gestire il count down dei prefab Gear orizzionatali */
public class GearManagerHorz : MonoBehaviour
{

    public void SetTotSec(float totSec) {

        transform.GetChild(0).GetComponent<GearAbstract>().SetTotSec(totSec);
    }

    public void TriggerCountDown()
    {
        transform.GetChild(0).GetComponent<GearAbstract>().TriggerCountDown();
    }


}
