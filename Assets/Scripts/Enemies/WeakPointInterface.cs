using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface WeakPointInterface
{
    public void HitEnemy();

    public int GetEnemyCurrentLife();

    public bool IsHeadWeakPoint();

    public void DestroyAnimation();

    //public void DropSteamTank();
}
