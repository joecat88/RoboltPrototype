using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGreenWeakPoint : WeakPointBase
{
    // Start is called before the first frame update
    void Start()
    {
        currentLife = 1;
    }

    public override void HitEnemy()
    {
        currentLife--;
        //StartCoroutine(DisableCollider());
    }

    //// TODO mettere a fattor comune e comunque funziona solo se sei sicuro che il primo collider sia quello giusto
    //private IEnumerator DisableCollider()
    //{
    //    GetComponent<BoxCollider>().enabled = false;
    //    GetComponent<BoxCollider>().isTrigger = false;

    //    yield return new WaitForSeconds(1f);

    //    GetComponent<BoxCollider>().enabled = true;
    //    GetComponent<BoxCollider>().isTrigger = true;
    //}
}
