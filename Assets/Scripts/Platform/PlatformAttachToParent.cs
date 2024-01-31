using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachToParent : MonoBehaviour
{

    // quando il giocatore salta sulla piattaforma, viene impostato come figlio in modo da muoversi con essa
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.parent = transform.parent;
        }
    }

    // quando il giocatore salta via dalla piattaforma, viene reimpostato nella gerarchia oggetti

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.transform.parent = null;
    }
}
