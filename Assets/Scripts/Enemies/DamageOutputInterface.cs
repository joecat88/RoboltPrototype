using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface DamageOutputInterface
{

    public enum DamageSource { ENEMY_DAMAGE, SHOCK_DAMAGE, ENVIRONMENT_DAMAGE }
    public enum ResetPositionSource { FALL_RESET, ENVIRONMENT_RESET }
    public int GetDamageOutputValue();

    // serve per stabilire se attivare l'animazione di hitted (ENEMY) oppure no (TRAP)
    public DamageSource GetDamageSource();

    // serve per stabilire che il reset position va fatto
    // prendendo l'ultima posizione valida (FALL) oppure una posizione prestabilita (ENVIRONMENT)
    public ResetPositionSource GetResetPositionSource();
    public bool LeaveAlive();

    // Se diverso da 0,0,0 allora è una sorgente di danno che rischiede il riposizionamento del player
    public Vector3 GetNewPlayerPosition();

    public void DestroyMe();
}
