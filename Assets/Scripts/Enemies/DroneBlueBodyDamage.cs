using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Classe che serve solo per notificare al player quanti danni causa entare in collisione con il corpo del nemico */
public class DroneBlueBodyDamage : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    private int damageValue = 2;

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
