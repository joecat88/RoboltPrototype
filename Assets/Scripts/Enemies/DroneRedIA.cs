using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRedIA : EnemyNavMeshIA
{
    Vector3 targetPostition;
    public Transform bulletSpawnPoint;
    public Transform bulletSpawnPoint2;
    public GameObject bulletPrefab;

    public override void SetRange()
    {
        //defaultViewAngle = 360;
        //aggroViewAngle = 360;
        //sightRange = 7;
        //attackRange = 4;
        UpdateCurrentViewAngle(viewAngle);
        navSpeed = 2f;
    }

    protected override void EnemyNavMeshStart()
    {

    }

    protected override void ExecuteAttack()
    {
        targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(targetPostition);

        if (!coolDownAttack)
        {
            coolDownAttack = true;
            agent.speed = 25f;
            StartCoroutine(Charge());
        }
        UpdateCurrentViewAngle(aggroViewAngle);
    }

    protected override void CancelAttack()
    {
        
    }

    protected IEnumerator Charge()
    {
        agent.SetDestination(targetPostition);
 
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Destroy(bullet, 2f);
        yield return new WaitForSeconds(0.1f);
        bullet = Instantiate(bulletPrefab, bulletSpawnPoint2.position, Quaternion.identity);
        Destroy(bullet, 2f);
        yield return new WaitForSeconds(0.4f);
        coolDownAttack = false;
    }
}
