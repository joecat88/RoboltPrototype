using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GearAbstract : CountDownAbstract
{
    //private Transform objectParent;
    private Transform teeth;
    private static float DEFAULT_SEC = 0f;
    private Transform myParent;

    protected abstract void SetGearType();
    private void Awake()
    {
        //objectParent = gameObject.transform.parent;
        teeth = gameObject.transform.GetChild(0);
        SetTotSec(DEFAULT_SEC);
        SetGearType();
        myParent = transform.parent;
    }

    private void Start()
    {
        
        switch(gearType)
        {
            case GearType.HOR_CW:

                originalColor = new Color(0.745283f, 0.5514196f, 0.1504627f, 1.0f);
                pulsingColor = new Color(0.68f, 0.28f, 0.23f);
                break;

            case GearType.HOR_CCW:

                originalColor = new Color(0.8301887f, 0.7788221f, 0.1284442f, 1.0f);
                pulsingColor = new Color(0.68f, 0.28f, 0.23f);
                break;
        }
    }

    public GearType GetGearType()
    {
        return gearType;
    }

    // implementazione personalizzata dell'abstract RedPulse, per prendere i Mesh Renderer del singolo oggetto
    protected override IEnumerator RedPulse()
    {
        
        while (isPulsing)
        {

            GetComponent<MeshRenderer>().materials[0].color = pulsingColor;
            teeth.GetComponent<MeshRenderer>().materials[0].color = pulsingColor;

            yield return new WaitForSeconds(PULSING_INTERVAL);

            GetComponent<MeshRenderer>().materials[0].color = originalColor;
            teeth.GetComponent<MeshRenderer>().materials[0].color = originalColor;
            yield return new WaitForSeconds(PULSING_INTERVAL);

        }

       // controllo se il player è nella gerachia, lo sposto fuori prima di distruggere l'oggetto
        Transform player = myParent.Find("Player");
        if(player != null)
            player.parent = null;

        if (gearType == GearType.VERTICAL)
            Destroy(transform.parent.parent.gameObject, 0);
        else
            Destroy(transform.parent.gameObject, 0);
    }

}
