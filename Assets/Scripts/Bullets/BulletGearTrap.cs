using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGearTrap : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    public int damageOutput = 1;

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

    void Start()
    {
        // imposto un timeout nel caso i bullet non dovessero colpire il giocatore o fosse invulvenrabile
        Destroy(gameObject, 2f);
    }

    public DamageOutputInterface.DamageSource GetDamageSource()
    {
        return DamageOutputInterface.DamageSource.ENVIRONMENT_DAMAGE;
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
