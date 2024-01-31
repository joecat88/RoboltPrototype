using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyNavMeshIA : EnemyDetectionIA
{
    // TODO rendere la modifica della agent.speed con metodo
    
    // TODO bulletPrefab non serve a tutte le sottoclassi, tipo il RobotHeadGreen 
    //public GameObject bulletPrefab;
    protected NavMeshAgent agent;
    
    // TODO special per ora è usato per il robot green che si fermo dopo l'attacco
    protected enum Status { PATROL, AGGRO, ATTACK, SPECIAL }
    [SerializeField] private Status status;

    // Enemy setting
    protected float navSpeed;

    // Patroling
    public Transform[] patrolPoints;
    private int pointsIndex = 1;
    [SerializeField] private Vector3 targetPoint;
    private bool stopStatus;

    // Attacking
    public bool coolDownAttack;
    protected abstract void ExecuteAttack();
    protected abstract void CancelAttack();

    // TODO considerare di utilizzare invece virtual per non obbligare ad implementarla a tutti
    protected abstract void EnemyNavMeshStart();

    protected override void EnemyDetectionStart()
    {
        //player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        UpdateCurrentViewAngle(viewAngle);
        status = Status.PATROL;

        // setto il primo punto da raggiungere
        targetPoint = patrolPoints[pointsIndex].position;
        agent.SetDestination(targetPoint);

        EnemyNavMeshStart();
    }

    // Metodo richiamata da Update della classe padre
    protected override void ManageActions()
    {
        if (!playerInSightRange && !playerInAttackRange && !stopStatus)
            Patroling();
        if (playerInSightRange && !playerInAttackRange && !stopStatus)
            AggroPlayer();
        if (playerInSightRange && playerInAttackRange && !stopStatus)
            AttackPlayer();
    }

    private void Patroling()
    {
        CancelAttack();

        SetStatus(Status.PATROL);

        UpdateCurrentViewAngle(viewAngle);
        agent.speed = navSpeed;

        if (Vector3.Distance(transform.position, targetPoint) < 1)
        {
            // aggiorno il prossimo punto da raggiungere e resetto se ho completato un giro
            pointsIndex++;
            if (pointsIndex == patrolPoints.Length)
                pointsIndex = 0;

            targetPoint = patrolPoints[pointsIndex].position;
        }
        // TODO migliorare il set che non serve sempre
        agent.SetDestination(targetPoint);
        agent.isStopped = false;
    }

    private void AggroPlayer()
    {
        CancelAttack();

        SetStatus(Status.AGGRO);

        // TODO non serve reimpostarlo sempre
        agent.isStopped = false;
        UpdateCurrentViewAngle(aggroViewAngle);
        agent.speed = 2;

        agent.SetDestination(player.position);

        //Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
        //transform.LookAt(targetPosition);
    }

    private void AttackPlayer()
    {
        SetStatus(Status.ATTACK);

        // metodo implementato custom da ogni tipo di nemico
        if (!isDying)
            ExecuteAttack();
    }

    protected void SetStatus(Status status)
    {
        this.status = status;
    }

    protected Status GetStatus()
    {
        return status;
    }

    protected void StopStatus(bool condition)
    {
        if (condition)
            SetStatus(Status.SPECIAL);
        stopStatus = condition;
    }

}

//// Metodo che controlla se il player è visibile dal nemico e se rientra nella distanza di attacco
//private void CheckPlayerInRange()
//{
//    // controllo se il player è nel raggio di aggro del nemico
//    bool isPlayerInRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

//    // calcolo la distanza tra nemico e player
//    float dstToPlayer = Vector3.Distance(transform.position, player.position);

//    if (isPlayerInRange)
//    {
//        // calcolo la direzione verso il player
//        Vector3 dirToPlayer = (player.position - transform.position).normalized;

//        // se il player è nel campo visivo (la meta del totale campo visivo rispetto a dove sta puntando il nemico)
//        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
//        {

//            // se il player non è nascosto da ostacoli
//            // TODO migliorare caso in cui il raycast intercetti il giocatore attraverso oggetti senza layer Obstacles (pavimenti, ..)
//            if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
//                playerInSightRange = true;
//            else
//                playerInSightRange = false;
//        }
//        else
//            playerInSightRange = false;
//    }
//    else
//        playerInSightRange = false;

//    // se il player è nel raggio di aggro del nemico e rientra nella distanza di attacco
//    if (playerInSightRange && (dstToPlayer <= attackRange))
//        playerInAttackRange = true;
//    else
//        playerInAttackRange = false;

//}

