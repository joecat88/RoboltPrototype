using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformShockInterval : MonoBehaviour
{

    public GameObject[] particlesList;
    public GameObject smoke;
    public GameObject damageCollider;
    private Animator animator;
    private AudioSource audio;

    private float time;
    private bool condition;
    public float shockInterval;

    private static readonly int shockHashID = Animator.StringToHash("shock");

    void Start()
    {
        time = 0f;
        condition = false;
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= shockInterval)
        {
            // ad intervalli regolari, inverte true/false per attivare/disattivare lo shock
            condition = !condition;
            TriggerShock(condition);
        }

    }

    void TriggerShock(bool condition)
    {
        // controllo animazione
        animator.SetBool(shockHashID, condition);

        // audio
        audio.enabled = condition;

        // controllo particellari elettricità
        foreach (GameObject particle in particlesList)
        {
            particle.SetActive(condition);
        }

        //controllo particellari fumo
        //smoke.SetActive(!condition);

        // controllo collider danni
        damageCollider.SetActive(condition);


        time = 0;
    }

}
