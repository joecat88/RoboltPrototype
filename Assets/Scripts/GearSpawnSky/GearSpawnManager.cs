using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSpawnManager : MonoBehaviour
{
    //Spawn this object
    public GameObject [] gearList;
    public GameObject[] spawnPointsList;

    // contenitore glocable per tutti i gears
    public Transform gearsParent;

    private int gearListSize;
    private int spawnPointsListSize;

    private int maxTime = 12;
    private int minTime = 5;

    private float time;
    private int spawnTime;

    void Start()
    {
        SetRandomTime();
        time = minTime;
        gearListSize = gearList.Length;
        spawnPointsListSize = spawnPointsList.Length;
    }

    void Update()
    {
        time += Time.deltaTime;

        //Check if its the right time to spawn the object
        if (time >= spawnTime)
        {
            SpawnGear();
            SetRandomTime();
        }

    }

    void SpawnGear()
    {
        Transform spawnPoint = spawnPointsList[Random.Range(0, spawnPointsListSize - 1)].transform;
        GameObject gear = gearList[Random.Range(0, gearListSize-1)];
        GameObject bullet = Instantiate(gear, spawnPoint.position, gear.transform.rotation);
        bullet.transform.parent = gearsParent;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 8f, ForceMode.Impulse);

        time = 0;
    }

    //Sets the random time between minTime and maxTime
    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
