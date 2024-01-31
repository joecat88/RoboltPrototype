using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearVertical : GearAbstract
{
    /* Classe personalizzata per ingranaggi verticali, che hanno un'ulteriore parent P a cui va collegato il player,
     * quindi la gestione di OnTriggerEnter/Exit viene annullata e gestita da uno script attaccato a P */

    protected override void SetGearType()
    {
        this.gearType = GearType.VERTICAL;
    }

    //public override void OnTriggerEnter(Collider other)
    //{

    //}

    //public override void OnTriggerExit(Collider other)
    //{

    //}
}
