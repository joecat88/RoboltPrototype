using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGearShooter : MonoBehaviour
{
    public Transform spawnGearPosition;
    public GameObject gearBulletPrefab;

    public float speed;
    public float interval;
    public float coolDown;

    private bool isCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoolDown)
            StartCoroutine(ShootAndWait());
    }

    IEnumerator ShootAndWait()
    {
        isCoolDown = true;
        // Quaternion.Euler(90, 0, 0)
        GameObject bullet = Instantiate(gearBulletPrefab, spawnGearPosition.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<RotateX>().setDirection(Rotation.Direction.counterclockwise);
        bullet.GetComponent<RotateX>().setSpeed(20f);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(bullet, 0.8f);

        yield return new WaitForSeconds(interval);

        bullet = Instantiate(gearBulletPrefab, spawnGearPosition.position, Quaternion.identity);
        bullet.transform.parent = transform.parent;
        rb = bullet.GetComponent<Rigidbody>();
        bullet.GetComponent<RotateX>().setDirection(Rotation.Direction.counterclockwise);
        bullet.GetComponent<RotateX>().setSpeed(20f);
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(bullet, 0.8f);

        yield return new WaitForSeconds(coolDown);

        isCoolDown = false;
    }
}
