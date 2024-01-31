using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ElectricZone : MonoBehaviour, DamageOutputInterface
{
    public Vector3 position;
    private float PULSE_TIME = 0.4f;
    private int damageValue = 7;
    private MeshRenderer meshR;

    public int GetDamageOutputValue() {
        return damageValue;
    }

    public bool LeaveAlive()
    {
        return true;
    }

    void Start() {

        StartCoroutine(pulseYellow());
    }

    IEnumerator pulseYellow() {

        meshR = this.gameObject.transform.GetComponent<MeshRenderer>();

        while (true) {


            meshR.material.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(PULSE_TIME);

            meshR.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            yield return new WaitForSeconds(PULSE_TIME);


        }
    }

    public Vector3 GetNewPlayerPosition()
    {
        return position;
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
