using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGreenBodyDamage : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    private int damageValue = 1;

    public int GetDamageOutputValue()
    {
        return damageValue;
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
        return DamageOutputInterface.DamageSource.ENEMY_DAMAGE;
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
