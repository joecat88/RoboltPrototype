using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFade : MonoBehaviour
{
    public float timeToStart;
    private float timer = 0;
    private bool isReady;
    private Animator animator;
    private GameObject platform;
    public Collider collider; 


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        platform = transform.GetChild(0).gameObject;
        platform.SetActive(false);
        isReady = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) 
        {
            timer += Time.deltaTime;
            if (timer > timeToStart) {
                platform.SetActive(true);
                animator.enabled = true;
                isReady = true;
            }
        }
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void EnableCollider()
    {
        collider.enabled = true;
    }
}
