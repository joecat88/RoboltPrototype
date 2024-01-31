using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRedWeakPoint : WeakPointBase
{

    public GameObject boltKey;
    private Collider boltKeyCollider;
    private bool isBoltKeyActive;

    void Start()
    {
        currentLife = 1;
        // controllo se il boltKey esiste o è già stato preso
        isBoltKeyActive = boltKey.activeSelf;

        if (isBoltKeyActive)
        {
            boltKeyCollider = boltKey.GetComponent<Collider>();
            boltKeyCollider.enabled = false;
        }
    }

    public override void HitEnemy()
    {
        currentLife--;
        StartCoroutine(DisableCollider());
        //animator.SetTrigger("DroneHitted");
    }

    // TODO mettere a fattor comune e comunque funziona solo se sei sicuro che il primo collider sia quello giusto
    private IEnumerator DisableCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = false;

        yield return new WaitForSeconds(1f);

        GetComponent<BoxCollider>().enabled = true;
        GetComponent<BoxCollider>().isTrigger = true;
    }

    public override void DestroyObj()
    {
        if (isBoltKeyActive)
        {
            boltKeyCollider.enabled = true;
            boltKey.transform.parent = transform.parent;
        }
        base.DestroyObj();
    }
}
