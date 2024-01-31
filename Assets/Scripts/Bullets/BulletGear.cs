using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGear : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    public int damageOutput = 1;

    public GameObject physicCollider;
    private GameObject parentObject;


    public int GetDamageOutputValue()
    {
        return damageOutput;
    }
    public bool LeaveAlive()
    {
        return false;
    }

    public Vector3 GetNewPlayerPosition()
    {
        return position;
    }

    void Start()
    {
        parentObject = gameObject.transform.parent.gameObject;
        // imposto un timeout nel caso i bullet non dovessero colpire il giocatore o fosse invulvenrabile
        Destroy(parentObject, 4f);
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
        Destroy(parentObject, 0);
    }

}
