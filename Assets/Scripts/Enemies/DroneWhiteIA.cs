using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneWhiteIA : EnemyDetectionIA
{
    public float bulletSpeed;
    public float fireRate;
    public GameObject bulletPrefab;
    public List<Transform> bulletSpawnPointList;
    private Vector3 targetPostition;

    private bool coolDownAttack;

    public override void SetRange()
    {
        //defaultViewAngle = 100;
        //aggroViewAngle = 360;
        //sightRange = 8;
        //attackRange = 6;
        UpdateCurrentViewAngle(viewAngle);
    }

    protected override void EnemyDetectionStart()
    {

    }

    protected override void ManageActions()
    {
        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            AggroPlayer();
        if (playerInSightRange && playerInAttackRange)
            AttackPlayer();
    }

    private void Patroling()
    {
        UpdateCurrentViewAngle(viewAngle);
    }

    private void AggroPlayer() 
    {
        targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(targetPostition);
        UpdateCurrentViewAngle(aggroViewAngle);

        // eventuale animazione
    }

    private void AttackPlayer()
    {
        targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(targetPostition);

        //float dstToPlayer = Vector3.Distance(transform.position, player.position);
        //Vector3 dirToPlayer = (player.position - transform.position).normalized;

        //bool isPlayerProtectedByLayer = !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, ignoreSightLayerMask);

        if (!coolDownAttack)
        {
            coolDownAttack = true;
            StartCoroutine(FireAndWait());
            shoot.Play();
        }
        UpdateCurrentViewAngle(aggroViewAngle);
    }

    // TODO rendere metodo comune per tutti
    private IEnumerator FireAndWait()
    {


        foreach (Transform tr in bulletSpawnPointList)
        {
            GameObject bullet = Instantiate(bulletPrefab, tr.position, Quaternion.Euler(90, 0, 0));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            rb.AddForce(tr.forward * bulletSpeed, ForceMode.Impulse);
            //rb.AddForce(transform.up * 1f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(fireRate);

        coolDownAttack = false;
    }

}
