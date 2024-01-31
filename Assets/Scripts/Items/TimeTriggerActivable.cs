using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTriggerActivable : TimeTrigger, ActivableObjectInterface
{

    private Collider triggerCollider;
    public GameObject hourglass;
    public GameObject ghostHourglass;

    // Start is called before the first frame update
    public void Awake()
    {
        base.Start();
        triggerCollider = GetComponent<Collider>();
        triggerCollider.enabled = false;
    }

    public void ActivateObject()
    {
        triggerCollider.enabled = true;
        hourglass.SetActive(true);
        ghostHourglass.SetActive(false);
    }

}
