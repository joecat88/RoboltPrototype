using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackManager : RoboltBase
{
    // costanti
    private const float JETPACK_VERT_SPEED = 3.5f;
    private const float JETPACK_HORZ_SPEED = 1.0f;
    private const int ENGINE_SMOOTHNES = 4;

    // reference
    public ParticleSystem leftFlamePS;
    public ParticleSystem leftSkomePS;
    public ParticleSystem rightFlamePS;
    public ParticleSystem rightSmokePS;
    private GameObject jetPack;
    private Animator leftEngineAnim;
    private Animator rightEngineAnim;

    // variabili di controllo
    private ParticleSystem.MainModule leftFlameMainPS;
    private ParticleSystem.MainModule leftSmokeMainPS;
    private ParticleSystem.MainModule rightFlameMainPS;
    private ParticleSystem.MainModule rightSmokeMainPS;
    private int leftEngineLeftPressed = 0;
    private int leftEngineRightPressed = 0;
    private int leftEngineUpPressed = 0;
    private int leftEngineNotPressed = 0;
    private int rightEngineLeftPressed = 0;
    private int rightEngineRightPressed = 0;
    private int rightEngineUpPressed = 0;
    private int rightEngineNotPressed = 0;
    private bool isPlayerControl;

    // variabili di stato
    [SerializeField] protected bool isJetPackReady;
    [SerializeField] protected bool isJetPackSession;
    [SerializeField] private bool isCameraToSet;

    // stringhe animator
    private static readonly int engineLeftID = Animator.StringToHash("left");
    private static readonly int engineRightID = Animator.StringToHash("right");
    private static readonly int engineUpID = Animator.StringToHash("up");

    enum JETPACK_DIR { UP, UPLF, UPRG }

    protected override void Start()
    {
        base.Start();

        jetPack = transform.Find("JetPack").gameObject;
        leftEngineAnim = jetPack.transform.Find("LeftEngine").GetComponent<Animator>();
        rightEngineAnim = jetPack.transform.Find("RightEngine").GetComponent<Animator>();

        isJetPackReady = false;
        isCameraToSet = false;
        isJetPackSession = false;
        isPlayerControl = true;

        // inizializzo le variabili per modicare i parametri dei particellari del JetPack
        leftFlameMainPS = leftFlamePS.main;
        leftSmokeMainPS = leftSkomePS.main;
        rightFlameMainPS = rightFlamePS.main;
        rightSmokeMainPS = rightSmokePS.main;
    }

    protected void ManageJetPackSession()
    {
        // AVVIO JETPACK, alla prima chiamata, fa comparire il jetpack sul giocatore e avvia la routine di cambiamento camera
        if (!isCameraToSet)
        {

            // faccio comparire le mesh del jetpack
            foreach (Transform child in jetPack.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }

            // faccio partire gli effetti particellari
            leftFlamePS.Play();
            leftSkomePS.Play();
            rightFlamePS.Play();
            rightSmokePS.Play();

            isGroundedPlayer = false;

            // disabilito in controllo telecamera
            cinemachineScript.enabled = false;

            // centro, abbasso e allontano la camera
            StartCoroutine(MoveCameraJetPackSession());
            isCameraToSet = false;
        }

        // SESSIONE JETPACK
        if (isJetPackReady)
        {
            if (transform.position.y < 120)
            {
                if (isPlayerControl)
                {
                    // entrambi i trigger = vola in verticale
                    if (leftTrigger.action.ReadValue<float>() != 0 && rightTrigger.action.ReadValue<float>() != 0)
                    {
                        // particellari fiamme/fumo
                        LeftEnginePower(true);
                        RightEnginePower(true);

                        // animazione engine
                        LeftEngineUpAnim();
                        RightEngineUpAnim();

                        // movimento
                        MoveJetPack(JETPACK_DIR.UP);
                    }
                    // solo trigger sinistra = vola in verticale/sinistra
                    else if (leftTrigger.action.ReadValue<float>() != 0 && rightTrigger.action.ReadValue<float>() == 0)
                    {
                        // particellari fiamme/fumo
                        LeftEnginePower(true);
                        RightEnginePower(true);

                        // animazione engine
                        LeftEngineLeftAnim();
                        RightEngineLeftAnim();

                        // movimento
                        MoveJetPack(JETPACK_DIR.UPLF);
                    }
                    // solo trigger destra = vola in verticale/destra
                    else if (leftTrigger.action.ReadValue<float>() == 0 && rightTrigger.action.ReadValue<float>() != 0)
                    {
                        // particellari fiamme/fumo
                        LeftEnginePower(true);
                        RightEnginePower(true);

                        // animazione engine
                        LeftEngineRightAnim();
                        RightEngineRightAnim();

                        // movimento
                        MoveJetPack(JETPACK_DIR.UPRG);
                    }
                    // nessun trigger = caduta libera
                    else
                    {
                        // particellari fiamme/fumo
                        LeftEnginePower(false);
                        RightEnginePower(false);

                        // animazione engine
                        LeftEngineDownAnim();
                        RightEngineDownAnim();
                    }
                }
                // dopo il trigger finale, il player vola in alto da solo
                else
                {
                    // particellari fiamme/fumo
                    LeftEnginePower(true);
                    RightEnginePower(true);

                    // animazione engine
                    LeftEngineUpAnim();
                    RightEngineUpAnim();

                    // movimento
                    MoveJetPack(JETPACK_DIR.UP);
                }

                // GRAVITÀ (limitata)
                if (currentVelocity.y > -4)
                    currentVelocity.y += GRAVITY_JETPACK * Time.deltaTime;

                charController.Move(currentVelocity * Time.deltaTime);

                // TODO farmi seguire comunque dall'ombra salto
            }
        }

    }

    public void SetIsPlayerControl(bool condition)
    {
        isPlayerControl = condition;
    }

    public void SetCameraTargetNull()
    {
        cinemachine.m_LookAt = null;
        cinemachine.m_Follow = null;
    }

    // controlla il movimento del JetPack in base alla direzione in input
    private void MoveJetPack(JETPACK_DIR direction)
    {
        currentVelocity.y = 0f;
        jetPackMove = Vector3.zero;
        jetPackMove += transform.up * JETPACK_VERT_SPEED * Time.deltaTime;
        switch (direction)
        {
            case JETPACK_DIR.UPLF:
                jetPackMove += -transform.right * JETPACK_HORZ_SPEED * Time.deltaTime;
                break;
            case JETPACK_DIR.UPRG:
                jetPackMove += transform.right * JETPACK_HORZ_SPEED * Time.deltaTime;
                break;
        }
        charController.Move(jetPackMove);
    }

    private void LeftEnginePower(bool on)
    {
        if (on)
        {
            leftFlameMainPS.startLifetime = 0.25f;
            leftSmokeMainPS.startSpeed = 0.1f;
            leftSmokeMainPS.startSize = 0.34f;
        }
        else
        {
            leftFlameMainPS.startLifetime = 0.12f;
            leftSmokeMainPS.startSpeed = 0.05f;
            leftSmokeMainPS.startSize = 0.18f;
        }
    }

    private void RightEnginePower(bool on)
    {
        if (on)
        {
            rightFlameMainPS.startLifetime = 0.25f;
            rightSmokeMainPS.startSpeed = 0.1f;
            rightSmokeMainPS.startSize = 0.34f;
        }
        else
        {
            rightFlameMainPS.startLifetime = 0.12f;
            rightSmokeMainPS.startSpeed = 0.05f;
            rightSmokeMainPS.startSize = 0.18f;
        }
    }

    private void LeftEngineLeftAnim()
    {

        leftEngineUpPressed = 0;
        leftEngineRightPressed = 0;
        leftEngineNotPressed = 0;

        if (leftEngineLeftPressed < ENGINE_SMOOTHNES)
            leftEngineLeftPressed++;
        else
        {
            // animazione engine
            leftEngineAnim.SetBool(engineLeftID, true);
            leftEngineAnim.SetBool(engineRightID, false);
            leftEngineAnim.SetBool(engineUpID, false);
        }
    }

    private void LeftEngineRightAnim()
    {

        leftEngineLeftPressed = 0;
        leftEngineUpPressed = 0;
        leftEngineNotPressed = 0;

        if (leftEngineRightPressed < ENGINE_SMOOTHNES)
            leftEngineRightPressed++;
        else
        {
            leftEngineAnim.SetBool(engineLeftID, false);
            leftEngineAnim.SetBool(engineRightID, true);
            leftEngineAnim.SetBool(engineUpID, false);
        }
    }

    private void LeftEngineUpAnim()
    {

        leftEngineLeftPressed = 0;
        leftEngineRightPressed = 0;
        leftEngineNotPressed = 0;

        if (leftEngineUpPressed < ENGINE_SMOOTHNES)
            leftEngineUpPressed++;
        else
        {
            // animazione engine
            leftEngineAnim.SetBool(engineLeftID, false);
            leftEngineAnim.SetBool(engineRightID, false);
            leftEngineAnim.SetBool(engineUpID, true);
        }
    }

    private void LeftEngineDownAnim()
    {

        leftEngineLeftPressed = 0;
        leftEngineRightPressed = 0;
        leftEngineUpPressed = 0;

        if (leftEngineNotPressed < ENGINE_SMOOTHNES)
            leftEngineNotPressed++;
        else
        {
            // animazione engine
            leftEngineAnim.SetBool(engineLeftID, false);
            leftEngineAnim.SetBool(engineRightID, false);
            leftEngineAnim.SetBool(engineUpID, true);
        }
    }

    private void RightEngineLeftAnim()
    {
        rightEngineUpPressed = 0;
        rightEngineRightPressed = 0;
        rightEngineNotPressed = 0;

        if (rightEngineLeftPressed < ENGINE_SMOOTHNES)
            rightEngineLeftPressed++;
        else
        {
            // animazione engine
            rightEngineAnim.SetBool(engineLeftID, true);
            rightEngineAnim.SetBool(engineRightID, false);
            rightEngineAnim.SetBool(engineUpID, false);
        }
    }

    private void RightEngineRightAnim()
    {
        rightEngineLeftPressed = 0;
        rightEngineUpPressed = 0;
        rightEngineNotPressed = 0;

        if (rightEngineRightPressed < ENGINE_SMOOTHNES)
            rightEngineRightPressed++;
        else
        {
            rightEngineAnim.SetBool(engineLeftID, false);
            rightEngineAnim.SetBool(engineRightID, true);
            rightEngineAnim.SetBool(engineUpID, false);
        }
    }

    private void RightEngineUpAnim()
    {
        rightEngineLeftPressed = 0;
        rightEngineRightPressed = 0;
        rightEngineNotPressed = 0;

        if (rightEngineUpPressed < ENGINE_SMOOTHNES)
            rightEngineUpPressed++;
        else
        {
            // animazione engine
            rightEngineAnim.SetBool(engineLeftID, false);
            rightEngineAnim.SetBool(engineRightID, false);
            rightEngineAnim.SetBool(engineUpID, true);
        }
    }

    private void RightEngineDownAnim()
    {
        rightEngineLeftPressed = 0;
        rightEngineRightPressed = 0;
        rightEngineUpPressed = 0;

        if (rightEngineNotPressed < ENGINE_SMOOTHNES)
            rightEngineNotPressed++;
        else
        {
            // animazione engine
            rightEngineAnim.SetBool(engineLeftID, false);
            rightEngineAnim.SetBool(engineRightID, false);
            rightEngineAnim.SetBool(engineUpID, true);
        }
    }

    // cambio l'inquadratura della telecamera per la sessione jetpack
    private IEnumerator MoveCameraJetPackSession()
    {
        cinemachine.m_XAxis.Value = Mathf.Round(cinemachine.m_XAxis.Value);
        float xStep;

        while (cinemachine.m_YAxis.Value > 0.2f || cinemachine.m_XAxis.Value != 90f || followCamera.position != followCameraJetPack.position)
        {
            cinemachine.m_YAxis.Value -= 0.005f;

            if (cinemachine.m_XAxis.Value != 90f)
            {
                // arrotondo gli step di spostamento dell'asse X per assicurarmi di arrivare in un intorno di 90
                xStep = (cinemachine.m_XAxis.Value < 90f) ? 0.03f : -0.03f;
                cinemachine.m_XAxis.Value += xStep;
                if (Mathf.Round(cinemachine.m_XAxis.Value) == 90f)
                {
                    cinemachine.m_XAxis.Value = 90f;
                    //Debug.Log("Arrotondato a 90");
                }
            }

            // allontano il target della camera
            followCamera.position = Vector3.MoveTowards(followCamera.position, followCameraJetPack.position, 0.005f);
            yield return null;
        }

        // perfeziono il posizionamento finale dell'asse Y
        cinemachine.m_YAxis.Value = 0.2f;

        isJetPackReady = true;
    }

}
