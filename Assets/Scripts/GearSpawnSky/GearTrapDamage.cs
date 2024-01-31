using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTrapDamage : MonoBehaviour, DamageOutputInterface
{
    private Vector3 resetPosition;
    public int damageOutput;

    private void Start()
    {
        resetPosition = new Vector3(0, 0, 0);
        Destroy(transform.parent.gameObject, 3);
    }

    public int GetDamageOutputValue()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(transform.parent.gameObject, 1);
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
        return DamageOutputInterface.ResetPositionSource.FALL_RESET;
    }

    public void DestroyMe()
    {
        Destroy(gameObject, 0);
    }
}
