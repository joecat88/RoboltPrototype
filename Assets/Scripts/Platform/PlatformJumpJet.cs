using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformJumpJet : MonoBehaviour, ActivableObjectInterface
{
    public float jumpHeight;
    public float maxJumpTime;

    public float speedUp;
    public float speedDown;
    private bool isUp;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isAlreadyJumping;
    [SerializeField] private bool isAlreadyFireOn;
    [SerializeField] private bool isAlreadyFireOff;

    [SerializeField] private Vector3 startPosition;
    //private Vector3 endPosition;
    public float offSetY;
    [SerializeField] private Vector3 endPosition;

    private ParticleSystem.MainModule engineOneFlameMainPS;
    private ParticleSystem.MainModule engineOneSmokeMainPS;
    private ParticleSystem.MainModule engineTwoFlameMainPS;
    private ParticleSystem.MainModule engineTwoSmokeMainPS;
    private ParticleSystem.MainModule engineThreeFlameMainPS;
    private ParticleSystem.MainModule engineThreeSmokeMainPS;
    private ParticleSystem.MainModule engineFourFlameMainPS;
    private ParticleSystem.MainModule engineFourSmokeMainPS;

    // reference
    public ParticleSystem engineOneFlamePS;
    public ParticleSystem engineOneSkomePS;
    public ParticleSystem engineTwoFlamePS;
    public ParticleSystem engineTwoSmokePS;
    public ParticleSystem engineThreeFlamePS;
    public ParticleSystem engineThreeSkomePS;
    public ParticleSystem engineFourFlamePS;
    public ParticleSystem engineFourSomokePS;

    private new Collider collider;

    // Start is called before the first frame update
    void Awake()
    {
        if (jumpHeight == 0)
            throw new Exception("Non hai impostato l'altezza del salto");

        // inizializzo le variabili per modicare i parametri dei particellari 
        engineOneFlameMainPS = engineOneFlamePS.main;
        engineOneSmokeMainPS = engineOneSkomePS.main;

        engineTwoFlameMainPS = engineTwoFlamePS.main;
        engineTwoSmokeMainPS = engineTwoSmokePS.main;

        engineThreeFlameMainPS = engineThreeFlamePS.main;
        engineThreeSmokeMainPS = engineThreeSkomePS.main;

        engineFourFlameMainPS = engineFourFlamePS.main;
        engineFourSmokeMainPS = engineFourSomokePS.main;

        collider = GetComponent<Collider>();

        isAlreadyFireOff = false;
        isAlreadyFireOff = false;

        isJumping = false;
        isAlreadyJumping = false;
        isUp = true;

        startPosition = transform.position;
        endPosition = startPosition;
        endPosition.y = startPosition.y + offSetY;

    }



    void Update()
    {
        // se il player ha saltato
        if (isJumping)
        {
            // per impedire che possa essere attivato più volte contemporaneamente
            isAlreadyJumping = true;

            // muovo la piattaforma in su
            if (isUp)
            {
                if (!isAlreadyFireOn) 
                    FireOn();

                if (transform.position.y < endPosition.y)
                    transform.position = Vector3.MoveTowards(transform.position, endPosition, (Time.deltaTime * speedUp));
                else
                    isUp = false;
            }

            // quando ha raggiunto l'apice impostato esternamente da offSetY, torna giù
            else
            {
                if (!isAlreadyFireOff)
                    FireOff();
                if (transform.position.y > startPosition.y)
                    transform.position = Vector3.MoveTowards(transform.position, startPosition, (Time.deltaTime * speedDown));

                // quando ha terminato il ciclo di salita/discesa. resetto tutto
                else
                {
                    isAlreadyJumping = false;
                    SetIsJumping(false);
                    isUp = true;
                    isAlreadyFireOff = true;
                    isAlreadyFireOn = false;
                }
            }
        }
    }

    public void ActivateObject()
    {
        engineOneFlamePS.Play();
        engineOneSkomePS.Play();
        engineTwoFlamePS.Play();
        engineTwoSmokePS.Play();
        engineThreeFlamePS.Play();
        engineThreeSkomePS.Play();
        engineFourFlamePS.Play();
        engineFourSomokePS.Play();

        FireOff();

        collider.enabled = true;
    }

    public float GetJumpHeight()
    {
        return jumpHeight;
    }

    public float GetMaxJumpTime()
    {
        return maxJumpTime;
    }

    public void SetIsJumping(bool condition)
    {
        if (!isAlreadyJumping)
            isJumping = condition;
    }

    
    public void FireOn()
    {
        engineOneFlameMainPS.startLifetime = 0.28f;
        engineOneSmokeMainPS.startSpeed = 0.15f;
        engineOneSmokeMainPS.startSize = 0.38f;

        engineTwoFlameMainPS.startLifetime = 0.28f;
        engineTwoSmokeMainPS.startSpeed = 0.15f;
        engineTwoSmokeMainPS.startSize = 0.38f;

        engineThreeFlameMainPS.startLifetime = 0.28f;
        engineThreeSmokeMainPS.startSpeed = 0.15f;
        engineThreeSmokeMainPS.startSize = 0.38f;

        engineFourFlameMainPS.startLifetime = 0.28f;
        engineFourSmokeMainPS.startSpeed = 0.15f;
        engineFourSmokeMainPS.startSize = 0.38f;

        isAlreadyFireOn = true;
        isAlreadyFireOff = false;
    }

    public void FireOff()
    {
        engineOneFlameMainPS.startLifetime = 0.18f;
        engineOneSmokeMainPS.startSpeed = 0.1f;
        engineOneSmokeMainPS.startSize = 0.25f;

        engineTwoFlameMainPS.startLifetime = 0.18f;
        engineTwoSmokeMainPS.startSpeed = 0.1f;
        engineTwoSmokeMainPS.startSize = 0.25f;

        engineThreeFlameMainPS.startLifetime = 0.18f;
        engineThreeSmokeMainPS.startSpeed = 0.1f;
        engineThreeSmokeMainPS.startSize = 0.25f;

        engineFourFlameMainPS.startLifetime = 0.18f;
        engineFourSmokeMainPS.startSpeed = 0.1f;
        engineFourSmokeMainPS.startSize = 0.25f;

        isAlreadyFireOff = true;
        isAlreadyFireOn = false;
    }

}

//IEnumerator TurnOnEngine()
//{
//    FireOn();
//    engineOneFlamePS.Play();
//    engineOneSkomePS.Play();
//    engineTwoFlamePS.Play();
//    engineTwoSmokePS.Play();
//    engineThreeFlamePS.Play();
//    engineThreeSkomePS.Play();
//    engineFourFlamePS.Play();
//    engineFourSomokePS.Play();

//    yield return new WaitForSeconds(0.5f);

//    FireOff();

//    collider.enabled = true;
//}

//void Update()
//{
//    if (isJumpTriggered)
//    {
//        timer += Time.deltaTime;

//        // salita
//        if (timer < timeUp)
//            transform.Translate(Vector3.up * (Time.deltaTime * speedUp));

//        // discesa
//        else if (timer >= timeUp && timer < timeDown)
//            transform.Translate(Vector3.down * (Time.deltaTime * speedDown));

//        // fine animazione
//        else
//        {
//            timer = 0;
//            isJumpTriggered = false;
//        }       
//    }
//}
