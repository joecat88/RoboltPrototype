using System;
using System.Collections;
using UnityEngine;


public class DroneBlueIA : EnemyNavMeshIA
{
    public float bulletSpeed;
    public float fireRate;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    // chiamata in Awake dalla classe padre EnemyDetectionIA
    public override void SetRange()
    {
        if (bulletSpeed == 0 || fireRate ==0)
            throw new Exception("Non hai impostato i parametri di speed e rate dei proiettili");

        //defaultViewAngle = 150;
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
        // il nemico non si muove
        agent.isStopped = true;

        Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        //transform.LookAt(player);

        if (!coolDownAttack)
        {
            coolDownAttack = true;
            StartCoroutine(FireAndWait());
        }
        UpdateCurrentViewAngle(aggroViewAngle);
    }

    protected override void CancelAttack()
    {

    }

    // TODO rendere metodo comune per tutti
    private IEnumerator FireAndWait()
    {

        shoot.Play();

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(90,0,0));
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * bulletSpeed, ForceMode.Impulse);
        //rb.AddForce(transform.up * 1f, ForceMode.Impulse);

        yield return new WaitForSeconds(fireRate);

        coolDownAttack = false;
    }

}

