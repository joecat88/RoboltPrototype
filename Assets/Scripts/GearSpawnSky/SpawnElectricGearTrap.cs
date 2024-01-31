using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnElectricGearTrap : MonoBehaviour
{
    public GameObject electricGearPrefab;
    public GameObject player;
    private Transform spawnPoint;
    private bool alreadyShot;

    void Start()
    {
        spawnPoint = transform;
        alreadyShot = false;
    }

    public void SpawnGear()
    {
        if (!alreadyShot)
        {
            alreadyShot = true;

            float playerX = player.transform.position.x;
            float playerZ = player.transform.position.z;

            transform.position = new Vector3(playerX, transform.position.y, playerZ + 0.7f);

            GameObject bullet = Instantiate(electricGearPrefab, spawnPoint.position, electricGearPrefab.transform.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            bullet.transform.parent = transform;
            rb.AddForce(transform.forward * 28f, ForceMode.Impulse);

            StartCoroutine(CoolDown());
        }
    }

    IEnumerator CoolDown() 
    {
        yield return new WaitForSeconds(1f);
        alreadyShot = false;
    }



    //public GameObject gear;

    //private Transform spawnPoint;
    //private float time;
    //public float spawnTime;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    time = 0f;
    //    spawnPoint = transform;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    time += Time.deltaTime;

    //    if (time >= spawnTime)
    //    {
    //        SpawnGear();
    //    }

    //    void SpawnGear()
    //    {
    //        GameObject bullet = Instantiate(gear, spawnPoint.position, gear.transform.rotation);
    //        Rigidbody rb = bullet.GetComponent<Rigidbody>();
    //        bullet.transform.parent = transform;
    //        rb.AddForce(transform.forward * 12f, ForceMode.Impulse);

    //        time = 0;
    //    }
    //}
}
