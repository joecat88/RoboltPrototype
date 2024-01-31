using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    //Spawn this object
    public GameObject platformPrefab;

    private float time;
    public int spawnTime;
    public float platformTimeAlive;

    void Start()
    {
        time = 0f;
    }

    void Update()
    {
        time += Time.deltaTime;

        //Check if its the right time to spawn the object
        if (time >= spawnTime)
        {
            SpawnGear();
        }

    }

    void SpawnGear()
    {
        GameObject bullet = Instantiate(platformPrefab, transform.position, platformPrefab.transform.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.transform.parent = transform;
        rb.AddForce(transform.forward * 2f, ForceMode.Impulse);
        Destroy(bullet, platformTimeAlive);

        time = 0;
    }
}
