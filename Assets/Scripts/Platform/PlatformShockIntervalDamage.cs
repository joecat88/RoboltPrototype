using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShockIntervalDamage : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    public int damageOutput = 2;
    public int GetDamageOutputValue()
    {
        return damageOutput;
    }

    public bool LeaveAlive()
    {
        return true;
    }

    public Vector3 GetNewPlayerPosition()
    {
        return position;
    }

    public DamageOutputInterface.DamageSource GetDamageSource()
    {
        return DamageOutputInterface.DamageSource.SHOCK_DAMAGE;
    }

    public DamageOutputInterface.ResetPositionSource GetResetPositionSource()
    {
        return DamageOutputInterface.ResetPositionSource.ENVIRONMENT_RESET;
    }

    public void DestroyMe()
    {
        Destroy(gameObject, 0);
    }
}
