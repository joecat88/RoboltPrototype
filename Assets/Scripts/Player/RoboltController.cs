using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* Controllerpersonaggio in terza persona, finalmente quello adatto */
// {"lastChkPtData":[{"xPos":-128.8000030517578,"yPos":9.100000381469727,"zPos":-6.130000114440918}]}
// coordinate partenza Vector3(-128.701996,9.08876228,-6.13399982)
// coordinate 5th Vector3(-70.3499985,91.2699966,6.6500001)

public class RoboltController : JetPackManager
{
    // variabili di controllo
    [SerializeField] private float currentJumpHeight;
    [SerializeField] private float currentMaxJumpTime;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isHitted;
    [SerializeField] private bool isShocked;
    [SerializeField] private bool isFalling;
    [SerializeField] private bool isFallingToDeath;
    [SerializeField] private bool isDead;
    [SerializeField] private bool isJumpAllowed;
    [SerializeField] private bool isJumpingFromPlatformJumpJet;
    [SerializeField] private bool isHorizMovementAllowed;
    [SerializeField] private bool isVertMovementAllowed;
    [SerializeField] private bool isApplyNewPlayerPosition;
    [SerializeField] private LastChkPtDataListJson lastChkPtDataListJson;
    private string jsonPath;
    public UniqueItemsManagerZone1 itemsManager;

    int count = 0;

    // variabili di stato
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 charMove;
    [SerializeField] private Vector3 newPlayerPosition;
    [SerializeField] private float moveSpeed;
    private float groundedRadius = 0.30f;
    private float groundedOffset = -0.05f;
    private float timer = 0;
    private Vector3[] lastGroundPositionList;
    private float stepOffsetGround = Const.STEP_OFFSET_GROUND;
    private float stepOffsetAir = Const.STEP_OFFSET_AIR;

    // stringhe di accesso Animator
    private static readonly int fallingHashID = Animator.StringToHash("Falling");
    private static readonly int moveSpeedHashID = Animator.StringToHash("MoveSpeed");
    private static readonly int jumpingHashID = Animator.StringToHash("Jumping");
    private static readonly int legsJumpHashID = Animator.StringToHash("LegsJump");
    private static readonly int hittedHashID = Animator.StringToHash("Hitted");
    private static readonly int shockedHashID = Animator.StringToHash("Shocked");
    private static readonly int deathHashID = Animator.StringToHash("Death");
    private static readonly int jumpGearHashID = Animator.StringToHash("JumpGear");
    private static readonly int blackScreenHashID = Animator.StringToHash("blackScreen");

    // reference
    [SerializeField] private PlatformJumpJet currentPlatformJumpJet = null;

    public ParticleSystem[] shockedListPS;

    protected override void Start()
    {
        base.Start();
        currentJumpHeight = MAX_JUMP_HEIGHT;
        currentMaxJumpTime = MAX_JUMP_TIME;
        isGroundedPlayer = true;
        isHitted = false;
        isShocked = false;
        isFalling = false;
        isDead = false;
        isJumpAllowed = true;
        isHandleAnimationBlocked = false;
        isStoppedPlayerSessions = true;
        isHorizMovementAllowed = true;
        isVertMovementAllowed = true;
        isJumpingFromPlatformJumpJet = false;
        lastGroundPositionList = new Vector3[8];
        jsonPath = Application.streamingAssetsPath + "/LastCheckPoint.json";

        // leggo la posizione iniziale dal file json
        Vector3 startPosition = GetStartPositionFromJson();

        InitializeLastPositionList(startPosition);

        // TODO rimuovere commento
        transform.position = startPosition;

        // attivo l'eventuale checkpoint presso il quale viene ricaricata la scena da game over
        CheckActivateCheckPoint();

        // impostazioni gravità utlizzata nelle sessioni grounded
        SetGroundGravity();
    }

    private void Update()
    {
        if (isApplyNewPlayerPosition)
        {
            ApplyNewPlayerPosition();

            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                SetIsApplyNewPlayerPosition(false);
            }
        }
        else
        {
            if (isJetPackSession)
                ManageJetPackSession();

            else
                ManageGroundSession();
        }
    }

    public void SetGroundGravity() 
    {
        float timeToApex = currentMaxJumpTime / 2;
        gravityGround = (-2 * currentJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * currentJumpHeight) / timeToApex;
    }

    private void ManageGroundSession()
    {
        if (!isStoppedPlayerSessions)
        {
            isGroundedPlayer = charController.isGrounded;

            // aggiorna l'ultima posizione grounded per il ripristino della caduta nel vuoto
            if (isGroundedPlayer)
            {
                isJumping = false;
                charController.stepOffset = stepOffsetGround;

                // se il player stava saltano da una PlatformJumpJet, riporto la gravità default
                if (isJumpingFromPlatformJumpJet)
                {
                    SetGroundGravity();
                    isJumpingFromPlatformJumpJet = false;
                }

                // controllo se sto collidendo con un layer diverso da DynamicPlatorm allora aggiorno la posizione per il ripristino
                // in questo modo evito di ripristinare la posizione su un oggetto in movimento
                Ray ray = new Ray(transform.position, -transform.up);
                if (Physics.Raycast(ray, out RaycastHit hit, 0.2f))
                {
                    if (hit.collider.gameObject.layer != LayerMask.NameToLayer("DynamicPlatform"))
                        UpdateLastPositionList(transform.position);
                }
            }
            else
                charController.stepOffset = stepOffsetAir;

            // reset gravità
            if (isGroundedPlayer /*&& currentVelocity.y < 0*/)
            {
                currentVelocity.y = 0f;
            }

            // imposto velocità di spostamento in avanti (Z)
            // sei il player è stato colpito, rallenta a prescindere che sia a terra o in salto
            if (isHitted)
                moveSpeed = MOVE_SPEED_HITTED;
            else
                moveSpeed = !isJumping ? MOVE_SPEED_DEFAULT : MOVE_SPEED_JUMP;

            // MOVIMENTO ORIZZIONTALE X,Y
            if (isHorizMovementAllowed)
            {
                moveInput = movementControl.action.ReadValue<Vector2>();

                //if (moveInput.x > 0f && moveInput.x < 0.2f)
                //    moveInput.x = 0.2f;
                //if (moveInput.x < 0f && moveInput.x > -0.2f)
                //    moveInput.x = -0.2f;

                //if (moveInput.y > 0f && moveInput.y < 0.2f)
                //    moveInput.y = 0.2f;
                //if (moveInput.y < 0f && moveInput.y > -0.2f)
                //    moveInput.y = -0.2f;

                charMove = new Vector3(moveInput.x, 0, moveInput.y);
                charMove = cameraMain.forward * charMove.z + cameraMain.right * charMove.x;
                charMove.y = 0f; //per sicurezza annullo y

                // MOVIMENTO IN X/Z
                charController.Move(charMove * (Time.deltaTime * moveSpeed));

                // gestione rotazione del personaggio quando si muove
                if (moveInput != Vector2.zero)
                {
                    float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
                    Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * ROTATION_SPEED);
                }
            }
            // SALTO
            if (jumpControl.action.triggered && !isJumping && IsJumpPossible())
            {
                Jump();
            }

            // GRAVITÀ
            ManageGravity();

            // MOVIMENTO VERTICALE Y
            if (isVertMovementAllowed)
                charController.Move(currentVelocity * Time.deltaTime);


            // ANIMAZIONI
            HandleAnimation();

            // OMBRA
            HandleShadowIndicator();
        }

    }

    /////////////////////////////////// GESTIONE POSIZIONAMENTO

    // gestione gravità
    private void ManageGravity()
    {
        if (isGroundedPlayer)
            currentVelocity.y += gravityGround * Time.deltaTime;
        else if (jumpControl.action.ReadValue<float>() != 0 && currentVelocity.y > 0f)
        {
            float previousVelocity = currentVelocity.y;
            float newVeclocity = currentVelocity.y + (gravityGround * Time.deltaTime);
            float nextVelocity = (previousVelocity + newVeclocity) * 0.5f;
            currentVelocity.y = nextVelocity;
        }
        else
        {
            float previousVelocity = currentVelocity.y;
            float newVeclocity = currentVelocity.y + (gravityGround * FALL_MULTIPLIER * Time.deltaTime);
            float nextVelocity = (previousVelocity + newVeclocity) * 0.5f;
            currentVelocity.y = nextVelocity;
        }
    }

    //crea una sfera ai piedi del player e controlla se è in collisione per poter saltare
    private bool IsJumpPossible()
    {
        if (isJumpAllowed)
        {
            Vector3 position = transform.position;

            Vector3 spherePosition = new Vector3(position.x, position.y - groundedOffset,
                position.z);
            return Physics.CheckSphere(spherePosition, groundedRadius, groundLayers,
                QueryTriggerInteraction.Ignore);
        }
        else
            return false;

    }

    public void Jump()
    {
        // se sto effetuando un salto da una PlatformJumpJet
        if (currentPlatformJumpJet != null)
        {
            // ricalcolo la gravità con la nuova currentJumpHeight letta dalla PlatformJumpJet e modificata da RoboltInteraction
            SetGroundGravity();
            isJumpingFromPlatformJumpJet = true;

            // attiva lo script di animazione salita/discesa e disattiva il suo collider che rimane disattivo finchè non finisce l'animazione
            currentPlatformJumpJet.SetIsJumping(true);

            //// animazione spinta - NON SERVE PIÙ
            //currentPlatformJumpJet.GetComponent<Animator>().SetTrigger(jumpGearHashID);
        }

        currentVelocity.y += initialJumpVelocity * 0.5f;
        isGroundedPlayer = false;
        isJumping = true;


        // isJumping rimane true finchè il player non è di nuovo a terra
        //StartCoroutine(SetIsJumping());
    }

    // Aggiorno la posizione dell'indicatore ombra di posizione e lo rendo visibile solo se sono in salto
    private void HandleShadowIndicator() 
    {
        Vector3 newShadowPosition = transform.position;

        if (isJumping)
        {

            // calcolo il punto di contatto con la superficie più vicino al giocatore mentre è in salto
            Ray ray = new Ray(transform.position, -transform.up);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                //Debug.Log("POSITION=" + hit.point);
                newShadowPosition.y = hit.point.y - PROJECTOR_Y_OFFSET;
                projectorT.position = Vector3.MoveTowards(projectorT.position, newShadowPosition, (Time.deltaTime * SHADOW_SPEED));

                projector.enabled = true;
            }
            else
                projector.enabled = false;
        }
        else
        {
            projector.enabled = false;
            projectorT.position = Vector3.MoveTowards(projectorT.position, newShadowPosition, (Time.deltaTime * SHADOW_SPEED));
        }
    }

    private void ApplyNewPlayerPosition() 
    {
        transform.position = newPlayerPosition;
    }

    // tengo traccia delle ultime 8 posizioni grounded, la meno recente [0] per il reset position
    private void UpdateLastPositionList(Vector3 position)
    {
        // aggiorno solo se il player si è spostato
        if (position != lastGroundPositionList[7])
        {
            lastGroundPositionList[0] = lastGroundPositionList[1];
            lastGroundPositionList[1] = lastGroundPositionList[2];
            lastGroundPositionList[2] = lastGroundPositionList[3];
            lastGroundPositionList[3] = lastGroundPositionList[4];
            lastGroundPositionList[4] = lastGroundPositionList[5];
            lastGroundPositionList[5] = lastGroundPositionList[6];
            lastGroundPositionList[6] = lastGroundPositionList[7];
            lastGroundPositionList[7] = position;
        }
    }

    private void InitializeLastPositionList(Vector3 startPosition)
    {
        lastGroundPositionList[0] = startPosition;
        lastGroundPositionList[1] = startPosition;
        lastGroundPositionList[2] = startPosition;
        lastGroundPositionList[3] = startPosition;
        lastGroundPositionList[4] = startPosition;
        lastGroundPositionList[5] = startPosition;
        lastGroundPositionList[6] = startPosition;
        lastGroundPositionList[7] = startPosition;
    }

    private void CheckActivateCheckPoint()
    {
        List<CheckPoint> checkPointList = itemsManager.GetCheckPointList();
        bool checkPointFound = false;

        foreach (CheckPoint cp in checkPointList)
        {
            if (!checkPointFound)
            {
                Vector3 checkPointPosition = cp.GetSpawnPoint().position;
                float distance = Vector3.Distance(transform.position, checkPointPosition);
                if (distance < 0.5f)
                {
                    checkPointFound = true;
                    cp.ActivateCheckPoint(false);
                }
            }
        }
    }

    /////////////////////////////////// ANIMATIONS

    private void HandleAnimation()
    {
        if (!isHandleAnimationBlocked)
        {
            if (isGroundedPlayer)
            {
                ResetInAirAnimation();

                // idle
                if (moveInput == Vector2.zero || !isHorizMovementAllowed)
                    IdleAnimation();

                // camminata
                else if (moveInput != Vector2.zero)
                    WalkAnimation(moveInput);

            }
            else
            {
                // salto
                if (isJumping)
                {
                    JumpAnimation(true);
                }

                //// caduta - rimossa perchè non voglio che l'animazione caduta parta subito
                //else
                //{
                //    // TODO ripristinare
                //    //FallingAnimation(true);
                //}
            }
        }
    }

    public void IdleAnimation()
    {
        animator.SetFloat(moveSpeedHashID, 0);
        //animator.SetFloat("MoveSpeed", 0, 0.05f, Time.deltaTime);
    }

    private void WalkAnimation(Vector2 moveInput)
    {
        float animValue = Mathf.Max(Mathf.Abs(moveInput.x), Mathf.Abs(moveInput.y));
        animator.SetFloat(moveSpeedHashID, animValue);
        //animator.SetFloat("MoveSpeed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void JumpAnimation(bool condition)
    {
        animator.SetBool(jumpingHashID, condition);
        animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), condition ? 1 : 0);
        animator.SetBool(legsJumpHashID, condition);
    }

    public void HittedAnimation(bool condition)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), condition ? 1 : 0);
        animator.SetBool(hittedHashID, condition);
    }

    public void ShockedAnimation(bool condition)
    {
        animator.SetLayerWeight(animator.GetLayerIndex("HeadTrunk"), condition ? 0.55f : 0);
        animator.SetBool(shockedHashID, condition);
    }

    public void DeathAnimation()
    {
        animator.SetBool(deathHashID, true);
    }

    public void ResetInAirAnimation()
    {
        JumpAnimation(false);
        //FallingAnimation(false);
    }

    public void FallingAnimation()
    {
        StopAnimantionsAndMovements();
        animator.SetBool(fallingHashID, true);

        // fermo la telecamera fino al ripristino posizione
        cinemachine.m_LookAt = null;
        cinemachine.m_Follow = null;

        StartCoroutine(FallDownCountDown());
    }

    public void BlackScreenAnimation() 
    {
        blackScreenAnimator.SetTrigger(blackScreenHashID);
    }

    public void StopAnimantionsAndMovements()
    {
        SetIsHandleAnimationBlocked(true);
        IdleAnimation();
        ResetInAirAnimation();
        ShockedAnimation(false);
        SetIsJumpAllowed(false);
        SetIsHorizMovementAllowed(false);
    }

    public void RestoreAnimationsAndMovements()
    {
        SetIsJumpAllowed(true);
        SetIsHorizMovementAllowed(true);
        SetIsHandleAnimationBlocked(false);
    }

    /////////////////////////////////// SETTER

    public void SetJumpHeight(float h)
    {
        currentJumpHeight = h;
    }

    public void SetMaxJumpTime(float t)
    {
        currentMaxJumpTime = t;
    }

    public void SetIsJetPackSession(bool condition) 
    {
        isJetPackSession = condition;
    }

    public void SetIsJumpAllowed(bool condition)
    {
        isJumpAllowed = condition;
    }

    public void SetIsHorizMovementAllowed(bool condition)
    {
        isHorizMovementAllowed = condition;
    }    
    
    public void SetIsVertMovementAllowed(bool condition)
    {
        isVertMovementAllowed = condition;
    }

    public void SetIsGroundedPlayer(bool condition) 
    {
        isGroundedPlayer = condition;
    }

    public void SetCurrentPlatformJumpJet(PlatformJumpJet platformJumpJet)
    {
        currentPlatformJumpJet = platformJumpJet;
    }

    public void SetLastCheckPoint(Vector3 position)
    {
        SetLastCheckPointToJson(position);
    }

    public void SetIsApplyNewPlayerPosition(bool condition)
    {
        isApplyNewPlayerPosition = condition;
    }

    public void SetIsHitted(bool condition)
    {
        isHitted = condition;
    }

    public void SetIsHandleAnimationBlocked(bool condition)
    {
        isHandleAnimationBlocked = condition;
    }

    public void SetIsShocked(bool condition)
    {
        isShocked = condition;
        SetShockedEffect(condition);
    }

    public void SetIsFalling(bool condition)
    {
        isFalling = condition;
    }

    public void SetIsDead(bool condition)
    {
        isDead = condition;
    }

    public void SetIsStoppedPlayerSessions(bool condition)
    {
        isStoppedPlayerSessions = condition;
    }

    public void SetIsFallingToDeath(bool condition)
    {
        isFallingToDeath = condition;
    }

    // chiamata da RoboltInteraction
    // se la posizione è diversa da 0,0,0 allora cambio la posizione del player, discriminando in base all'enum ResetPositionSource
    // l'ultima posizione valida (FALL) oppure una posizione prestabilita (ENVIRONMENT)
    public void SetNewPlayerPosition(Vector3 position, DamageOutputInterface.ResetPositionSource source)
    {
        if (position != new Vector3(0, 0, 0))
        {
            isJumping = false;
            JumpAnimation(false);
            IdleAnimation();
            BlackScreenAnimation();

            // se è ripristino da caduta, prendo l'ultima posizione valida
            if (source == DamageOutputInterface.ResetPositionSource.FALL_RESET)
            {
                newPlayerPosition = GetLastGroundedPosition();
            }
            // se è ripristino da danni provocati da trappole ambientali, usa la position passata in input
            else
            {
                newPlayerPosition = position;
            }

            SetIsApplyNewPlayerPosition(true);
        }
    }

    /////////////////////////////////// GETTER
    private Vector3 GetLastGroundedPosition()
    {
        return lastGroundPositionList[0];
    }

    public bool GetIsShocked()
    {
        return isShocked;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public bool GetIsFalling()
    {
        return isFalling;
    }

    public bool GetIsFallingToDeath()
    {
        return isFallingToDeath;
    }

    /////////////////////////////////// JSON

    // leggo la posizione da cui partire (l'ultimo checkpoint attivo nel caso di game over)
    private Vector3 GetStartPositionFromJson()
    {
        using (StreamReader stream = new StreamReader(jsonPath))
        {
            String jsonRawFile = stream.ReadToEnd();
            lastChkPtDataListJson = JsonUtility.FromJson<LastChkPtDataListJson>(jsonRawFile);
        }

        float x = lastChkPtDataListJson.lastChkPtData[0].xPos;
        float y = lastChkPtDataListJson.lastChkPtData[0].yPos;
        float z = lastChkPtDataListJson.lastChkPtData[0].zPos;

        return new Vector3(x, y, z);
    }

    // scrive nel json l'ultima posizione del checkpoint attivo
    private void SetLastCheckPointToJson(Vector3 position)
    {
        float xNewPos = position.x;
        float yNewPos = position.y;
        float zNewPos = position.z;

        LastChkPtData[] lastChkPtDataToJson = new LastChkPtData[1];
        lastChkPtDataToJson[0] = new LastChkPtData() { xPos = xNewPos, yPos = yNewPos, zPos = zNewPos };

        lastChkPtDataListJson = new LastChkPtDataListJson() { lastChkPtData = lastChkPtDataToJson };

        using (StreamWriter stream = new StreamWriter(jsonPath))
        {
            String jsonRawFile = JsonUtility.ToJson(lastChkPtDataListJson);
            stream.Write(jsonRawFile);
        }
    }

    [ContextMenu("Reset Level progression")]
    private void ResetLevelProgression()
    { 
        
    }

    private void SetShockedEffect(bool condition)
    {
        foreach (ParticleSystem effect in shockedListPS) 
        {
            if (condition)
                effect.Play();
            else
                effect.Stop();
        }
    }


    private IEnumerator FallDownCountDown()
    {

        yield return new WaitForSeconds(Const.FALL_DOWN_DURATION);

        if (!isDead)
        {
            animator.SetBool(fallingHashID, false);

            // ripristino la camera
            cinemachine.m_LookAt = followCamera;
            cinemachine.m_Follow = followCamera;

            Vector3 position = new Vector3(1, 1, 1);
            SetNewPlayerPosition(position, DamageOutputInterface.ResetPositionSource.FALL_RESET);
            RestoreAnimationsAndMovements();
        }

        SetIsFalling(false);
    }

    /////////////////////////////////// COMANDI

    // abilita/disabilita comandi input
    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        leftTrigger.action.Enable();
        rightTrigger.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        leftTrigger.action.Disable();
        rightTrigger.action.Disable();
    }

    // Mostra nel Gizmos la sfera di controllo grounded
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        Gizmos.color = isGroundedPlayer ? transparentGreen : transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Vector3 position = transform.position;
        Gizmos.DrawSphere(
            new Vector3(position.x, position.y - groundedOffset, position.z), groundedRadius);
    }
}


/* Coroutine che dopo l'inizio di un salto setta isJumping=true finchè il player non è di nuovo a terra;
* in questo modo è possibile distinguere quando invece il player sta cadendo */
//private IEnumerator SetIsJumping()
//{
//    yield return new WaitForSeconds(0.5f); // TODO sicuramente c'è un modo migliore
//    yield return new WaitUntil(() => isGroundedPlayer);
//    isJumping = false;
//}