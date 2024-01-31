using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointActivable : MonoBehaviour, ActivableObjectInterface
{
    public GameObject globe;
    private CheckPoint checkPointScript;
    private Collider triggerCollider;


    // Start is called before the first frame update
    void Start()
    {
        checkPointScript = GetComponent<CheckPoint>();
        triggerCollider = GetComponent<Collider>();
        checkPointScript.enabled = false;
        triggerCollider.enabled = false;
        globe.SetActive(false);
    }

    public void ActivateObject()
    {
        globe.SetActive(true);
        checkPointScript.enabled = true;
        triggerCollider.enabled = true;
    }
}

