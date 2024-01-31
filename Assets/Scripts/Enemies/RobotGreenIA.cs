using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotGreenIA : EnemyNavMeshIA
{
    //private Vector3 targetPostition;
    private Animator animator;
    public AudioSource shockAudioSource;

    //private Transform body;
    //private Transform head;

    public GameObject leftHand;
    public GameObject rightHand;

    private static readonly int shockAttackHashID = Animator.StringToHash("ShockAttack");
    private static readonly int crounchingHashID = Animator.StringToHash("crouching");

    public override void SetRange()
    {
        //defaultViewAngle = 120;
        //aggroViewAngle = 360;
        //sightRange = 7;
        //attackRange = 4;
        UpdateCurrentViewAngle(viewAngle);
        navSpeed = 1f;
    }

    protected override void EnemyNavMeshStart()
    {
        animator = GetComponent<Animator>();
        //body = transform.GetChild(0);
        //head = body.GetChild(4);
    }

    protected override void ExecuteAttack()
    {
        shockAudioSource.enabled = true;
 
        agent.SetDestination(player.position);

        Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        transform.LookAt(targetPosition);

        // attivazione attacco
        animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 1);
        animator.SetBool(shockAttackHashID, true);
        leftHand.SetActive(true);
        rightHand.SetActive(true);




        //Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        //transform.LookAt(targetPosition);

        //if (!coolDownAttack)
        //{
        //    coolDownAttack = true;
        //    StartCoroutine(ShockAttack());
        //}

        //UpdateViewAngle(aggroViewAngle);
    }


    protected override void CancelAttack()
    {
        shockAudioSource.enabled = false;

        animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0);
        animator.SetBool(shockAttackHashID, false);
        leftHand.SetActive(false);
        rightHand.SetActive(false);
    }

    //private IEnumerator ShockAttack()
    //{   

    //    // attivazine attacco
    //    animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 1);
    //    animator.SetBool(shockAttackHashID, true);

    //    //////attivazione particellari

    //    yield return new WaitForSeconds(3f);

    //    StopStatus(true);
    //    animator.SetLayerWeight(animator.GetLayerIndex("Arms"), 0);
    //    animator.SetBool(shockAttackHashID, false);
    //    animator.SetBool(crounchingHashID, true);

    //    yield return new WaitForSeconds(2f);

    //    animator.SetBool(crounchingHashID, false);

    //    StopStatus(false);

    //    coolDownAttack = false;
    //}
    //protected IEnumerator RotationKick()
    //{
    //    agent.SetDestination(targetPostition);

    //    // CALCIO ROTANTE

    //    // abilito il collider per colpire il player // TODO assurdo che il trigger vada anche se il collider non è attivo
    //    body.GetComponent<BoxCollider>().enabled = true;
    //    body.GetComponent<BoxCollider>().isTrigger = true;

    //    animator.SetBool("RotationKick", true);

    //    // fine attacco calcio rotante
    //    yield return new WaitForSeconds(4f);

    //    // FASE IDLE VULNERABILE

    //    StopStatus(true);

    //    // disabilito il collider per non colpire più il player
    //    body.GetComponent<BoxCollider>().enabled = false;
    //    body.GetComponent<BoxCollider>().isTrigger = false;

    //    // abilito il coolider in modo che in nemico possa essere colpito
    //    GetComponent<BoxCollider>().enabled = true;
    //    GetComponent<BoxCollider>().isTrigger = true;

    //    agent.isStopped = true;
    //    animator.SetBool("RotationKick", false);

    //    // cambio il materiale per renderlo rosso, come feedback visivo di vulnerabilità
    //    Material matRed = Resources.Load("Materials/M_Capsule_Robot_2", typeof(Material)) as Material;
    //    Material matGreen = Resources.Load("Materials/M_Capsule_Robot_1", typeof(Material)) as Material;
    //    Material[] materials = head.GetComponent<SkinnedMeshRenderer>().materials;
    //    materials[0] = matRed;
    //    head.GetComponent<SkinnedMeshRenderer>().materials = materials;

    //    // fine fase vulnerabilità
    //    yield return new WaitForSeconds(2f);

    //    // disabilito il coolider in modo che in nemico possa essere colpito
    //    GetComponent<BoxCollider>().enabled = false;
    //    GetComponent<BoxCollider>().isTrigger = false;

    //    materials[0] = matGreen;
    //    head.GetComponent<SkinnedMeshRenderer>().materials = materials;
    //    animator.SetTrigger("RestorePatrol");

    //    StopStatus(false);
    //    agent.isStopped = false;

    //    coolDownAttack = false;
    //}

    // TODO sostituire con fuoco particellare



}
