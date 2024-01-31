using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingFwBwAbstract : MonoBehaviour
{
    public float endTime;
    public float speed;

    protected float timer;
    protected bool isForward;

    // muove avanti e indietro l'oggetto ad intervalli regolari
    protected void MoveFwBw()
    {
        timer += Time.deltaTime;
        if (timer < endTime)
        {
            if (isForward)
                transform.Translate(Vector3.forward * (Time.deltaTime * speed));
            else
                transform.Translate(Vector3.back * (Time.deltaTime * speed));
        }
        else
        {
            timer = 0;
            isForward = !isForward;
        }
    }

}
