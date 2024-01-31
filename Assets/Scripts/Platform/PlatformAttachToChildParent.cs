using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachToChildParent : MonoBehaviour
{
    Transform targetParent;

    private void Start()
    {
        targetParent = transform.parent.GetChild(0);
    }

    // quando il giocatore salta sulla piattaforma, viene impostato come figlio in modo da muoversi con essa
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("TRIGGeGR");   
            other.transform.parent = targetParent;
        }
    }

    // quando il giocatore salta via dalla piattaforma, viene reimpostato nella gerarchia oggetti

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("EXXXIXTXX");
            other.transform.parent = null;
        }
    }
}
