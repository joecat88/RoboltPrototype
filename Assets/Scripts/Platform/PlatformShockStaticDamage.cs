using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShockStaticDamage : MonoBehaviour, DamageOutputInterface
{
    public Transform resetTransformPosition;
    private Vector3 resetPosition;
    public int damageOutput;

    private void Start()
    {
        resetPosition = resetTransformPosition.position;
    }

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
