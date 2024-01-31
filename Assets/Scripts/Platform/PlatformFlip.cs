using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Se il giocatore salta sulla piattaforma, dopo downTime secondi la piattaforma cade
public class PlatformFlip : MonoBehaviour
{
    private const float PULSING_INTERVAL_DEFAULT = 0.3f;
    private const float PULSING_INTERVAL_PANIC = 0.1f;


    private static readonly int flipHashID = Animator.StringToHash("flip");
    private bool isPulsing;
    private float currentTimeUp;
    private float currentTimeDown;
    [SerializeField] private bool isTimerDown;
    [SerializeField] private bool isTimerUp;
    private float pulsingInterval;
    public Collider platformCollider;
    private Collider detectorCollider;


    // timer
    public float timeUpLimit; 
    public float timeDownLimit; // almeno 2 secondi per dare il tempo all'animazione

    private Color pulsingColor = new Color(1f, 0.7f, 0.7f);
    private Color originalColor = new Color(0.9f, 0.49f, 0.44f);

    // references
    private Animator animator;
    public MeshRenderer gearRenderer;
    public MeshRenderer teethRenderer;

    void Start()
    {
        if (timeUpLimit == 0 || timeDownLimit == 0)
            throw new Exception("Non hai impostato i parametri di tempo");

        animator = GetComponent<Animator>();
        detectorCollider = GetComponent<Collider>();
        currentTimeUp = 0f;
        currentTimeDown = 0f;
        isPulsing = false;
        isTimerDown = false;
        isTimerUp = false;
        pulsingInterval = PULSING_INTERVAL_DEFAULT;
    }

    void Update()
    {
        // se il giocatore è salito sulla piattaforma, parte il timer prima che la piattaforma cada
        if (isTimerUp)
        {
            currentTimeUp += Time.deltaTime;

            // se manca meno di 1 secondo, aumento la velocità di pulsing
            //if (timeUpLimit - currentTimeUp <= 0.7f)
            //    pulsingInterval = PULSING_INTERVAL_PANIC;

            //pulsingInterval = timeUpLimit / currentTimeUp;
            // quando il timer raggiunge il limite, la piattaforma cade
            if (currentTimeUp >= timeUpLimit)
            {
                
                animator.SetBool(flipHashID, true);
                isTimerUp = false;
                currentTimeUp = 0;

                isTimerDown = true;
            }
        }

        // se la piattaforma è caduta, parte il timer per farla risalire
        if (isTimerDown)
        {
            currentTimeDown += Time.deltaTime;
            // quando il timer raggiunge il limite, la piattaforma risale
            if (currentTimeDown >= timeDownLimit)
            {
                animator.SetBool(flipHashID, false);
                isTimerDown = false;
                currentTimeDown = 0;

                isTimerUp = false;       
            }
        }
    
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //Debug.Log("FlipTriggered");
            detectorCollider.enabled = false;
            isTimerUp = true;
            isPulsing = true;
            pulsingInterval = PULSING_INTERVAL_DEFAULT;
            StartCoroutine(GearPulsing());
        }
    }

    // chiamata dall'evento finale di animazione di risalita
    public void ActivateTriggerCollider() 
    {
        detectorCollider.enabled = true;
        platformCollider.enabled = true;
    }

    // chiamata dall'animator quando cade
    public void DisableGearCollide() {

        platformCollider.enabled = false;
    }

    // chimata da un evento nell'animator
    public void SetIsPulsingFalse()
    {
        isPulsing = false;
    }

    private IEnumerator GearPulsing()
    {

        while (isPulsing)
        {
            gearRenderer.materials[0].color = pulsingColor;
            teethRenderer.materials[0].color = pulsingColor;

            yield return new WaitForSeconds(pulsingInterval);

            gearRenderer.materials[0].color = originalColor;
            teethRenderer.materials[0].color = originalColor;
            yield return new WaitForSeconds(pulsingInterval);
        }
    }
}
