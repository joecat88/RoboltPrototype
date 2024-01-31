using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDetectionIA : MonoBehaviour, EnemyDetectionInterfaceIA
{
    public Transform player;
    private RoboltController roboltController;
    public LayerMask ignoreSightLayerMask;
    public LayerMask playerMask;
    protected bool isDying;

    // Condition
    [SerializeField] protected bool playerInSightRange;
    [SerializeField] protected bool playerInAttackRange;
    public float sightRange;
    public float attackRange;
    public float viewAngle;
    public float aggroViewAngle;
    [SerializeField] private float currentViewAngle;

    [SerializeField] private bool isPlayerInRange;

    // Audio
    public AudioSource shoot;

    // set dei parametri di detection
    public abstract void SetRange();

    // Start customizzata per ogni sottoclasse
    protected abstract void EnemyDetectionStart();

    // azioni di intraprendere in base alla detection, ogni sottoclasse implementa la sua versione
    protected abstract void ManageActions();

    protected void Start() 
    {
        SetRange();
        EnemyDetectionStart();
        isDying = false;
        roboltController = player.GetComponent<RoboltController>();
    }

    protected void Update()
    {
        if (roboltController.GetIsDead())
        {
            playerInSightRange = false;
            playerInAttackRange = false;
        }
        else
            // controllo la posizione del player
            CheckPlayerInRange();

        // in base alla posizione del player, stabilisco l'azione
        ManageActions();
    }

    // Metodo che controlla se il player è visibile dal nemico e se rientra nella distanza di attacco
    private void CheckPlayerInRange()
    {
        // controllo se il player è nel raggio di aggro del nemico
        //bool isPlayerInRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        isPlayerInRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        // calcolo la distanza tra nemico e player
        float dstToPlayer = Vector3.Distance(transform.position, player.position);

        if (isPlayerInRange)
        {
            // calcolo la direzione verso il player
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            // se il player è nel campo visivo (la meta del totale campo visivo rispetto a dove sta puntando il nemico)
            if (Vector3.Angle(transform.forward, dirToPlayer) < currentViewAngle / 2)
            {

                // se il player non è nascosto da ostacoli
                // TODO migliorare caso in cui il raycast intercetti il giocatore attraverso oggetti senza layer Obstacles (pavimenti, ..)

                //Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
                Vector3 origin = transform.position;

                if (!Physics.Raycast(origin, dirToPlayer, dstToPlayer, ignoreSightLayerMask))
                    playerInSightRange = true;
                else
                    playerInSightRange = false;
            }
            else
                playerInSightRange = false;
        }
        else
            playerInSightRange = false;

        // se il player è nel raggio di aggro del nemico e rientra nella distanza di attacco
        if (playerInSightRange && (dstToPlayer <= attackRange))
            playerInAttackRange = true;
        else
            playerInAttackRange = false;

    }

    protected void UpdateCurrentViewAngle(float viewAngle)
    {
        this.currentViewAngle = viewAngle;
    }

    public void SetIsDying(bool condition)
    {
        isDying = condition;
    }

    // TODO esiste già SetRange dell'interfaccia, migliorare
    //public void setRange(float defaultViewAngle, float aggroViewAngle, float sightRange, float attackRange)
    //{
    //    this.defaultViewAngle = defaultViewAngle;
    //    this.aggroViewAngle = aggroViewAngle;
    //    this.sightRange = sightRange;
    //    this.attackRange = attackRange;
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        float totalFOV = viewAngle;
        float rayRange = sightRange;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

    }
}
