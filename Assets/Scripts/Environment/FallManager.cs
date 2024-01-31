using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour, DamageOutputInterface
{
    public Vector3 resetPosition;
    public int damageOutput;

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
        return resetPosition;
    }

    public DamageOutputInterface.DamageSource GetDamageSource()
    {
        return DamageOutputInterface.DamageSource.ENVIRONMENT_DAMAGE;
    }

    public DamageOutputInterface.ResetPositionSource GetResetPositionSource()
    {
        return DamageOutputInterface.ResetPositionSource.FALL_RESET;
    }

    public void DestroyMe()
    {
        Destroy(gameObject, 0);
    }
}
