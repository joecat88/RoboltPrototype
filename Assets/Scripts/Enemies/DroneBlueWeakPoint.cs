using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBlueWeakPoint : WeakPointBase
{
    // Start is called before the first frame update
    void Start()
    {
        currentLife = 1;
        if (steamTankAmount == 0)
                throw new Exception("Non hai impostato il numero di SteamTank");
    }

    public override void HitEnemy()
    {
        currentLife--;
    }
}

