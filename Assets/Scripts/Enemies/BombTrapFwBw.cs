using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTrapFwBw : MovingFwBwAbstract
{
    private void Start()
    {
        timer = 0;
        isForward = true;
        if (speed == 0)
            throw new Exception("Non hai impostato la velocità");
        if (endTime == 0)
            throw new Exception("Non hai impostato la durata");
    }

    private void Update()
    {
        MoveFwBw();
    }
}
