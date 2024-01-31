using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyActivationManager : MonoBehaviour
{
    public int keyRequired;
    public int id;
    [SerializeField] bool isPlayerInsideTrigger;
    [SerializeField] bool isKeyEnough;
    [SerializeField] int playerBoltKeys;
    [SerializeField] private bool isAlreadyPressed;
    [SerializeField] private bool isMessageDisappeard;
    private Collider triggerCollider;
    [SerializeField] private List<ActivableObjectInterface> activableObjectInterfaceList;

    // references
    public RoboltInteractions roboltInteraction;
    public List<GameObject> activableObjectList; // gli oggetti che si vogliono attivare dal Manager
    public ActionOnButton actionOnButton;
    public ActionOffButton actionOffButton;
    public UniqueItemsManagerZone1 uniqueItemsManager;
    public AudioSource okSound;
    public AudioSource errorSound;

    // materiale di yellow emission per quando il giocatore ha abbastanza chiavi
    public Material yellowEmissionMaterial;
    private Material[] yellowEmissionMaterialsArray;

    // materiale di red emission per quando il giocatore NON ha abbastanza chiavi
    public Material redEmissionMaterial;
    private Material[] redEmissionMaterialsArray;

    private Material[] keyOffMaterialsArray;
    public GameObject keyGhost;

    // il numero che compare affianco alla chiave
    public GameObject numberTextRequired;
    public GameObject numberTextOn;
    public GameObject numberTextNotEnough;

    private Animator actionOnButtonAnimator;
    private Animator actionOffButtonAnimator;
    private Animator keyManagerAnimator;


    // UI references
    public Image actionOnPrompt;
    public Image actionOffPrompt;
    ///////public GameObject keyWarning;

    [SerializeField] protected InputActionReference actionButton;

    private static readonly int buttonOnPressedHashID = Animator.StringToHash("buttonOnPressed");
    private static readonly int buttonOffPressedHashID = Animator.StringToHash("buttonOffPressed");
    private static readonly int disappearHashID = Animator.StringToHash("disappear");

    void Awake()
    {
        if (keyRequired==0)
            throw new Exception("Non hai impostato il numero di chiavi necessarie");

        isPlayerInsideTrigger = false;
        isAlreadyPressed = false;
        isMessageDisappeard = false;
        playerBoltKeys = 0;
        triggerCollider = GetComponent<Collider>();
        actionOnButtonAnimator = actionOnButton.GetComponent<Animator>();
        actionOffButtonAnimator = actionOffButton.GetComponent<Animator>();
        /////////////keyGhost = transform.GetChild(0).gameObject;

        // salvo il materiale keyOff prendendolo dalla chiave default
        keyOffMaterialsArray = keyGhost.GetComponent<MeshRenderer>().materials;

        // salvo il material yellowEmission prendendolo dalla reference esterna
        yellowEmissionMaterialsArray = new Material[1];
        yellowEmissionMaterialsArray[0] = yellowEmissionMaterial;

        // salvo il material yellowEmission prendendolo dalla reference esterna
        redEmissionMaterialsArray = new Material[1];
        redEmissionMaterialsArray[0] = redEmissionMaterial;

        //// riferimento al numero mostrato
        //////////////numeberTextOff = keyGhost.transform.GetChild(1).gameObject;
        /////////////////numebrTextOn = keyGhost.transform.GetChild(2).gameObject;
        /////////////////numberTextNotEnough = keyGhost.transform.GetChild(3).gameObject;

        keyManagerAnimator = gameObject.GetComponent<Animator>();

        activableObjectInterfaceList = new List<ActivableObjectInterface>();

        //Debug.Log("KEYMANAGER numero " + id + " in fase di Start");

        foreach (GameObject obj in activableObjectList)
        {
            activableObjectInterfaceList.Add(obj.GetComponent<ActivableObjectInterface>());
        }

        //activableObjectInterface = obj.GetComponent<ActivableObjectInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInsideTrigger)
        {
            // se il player ha abbastanza chiavi
            if (isKeyEnough)
            {
                if (actionButton.action.triggered && !isAlreadyPressed)
                {
                    triggerCollider.enabled = false;

                    okSound.Play();

                    isAlreadyPressed = true;

                    // faccio partire l'animazione di bottone premuto, che alla fine dall'animator attiverà la condizione "isButtonPressedAnimationEnded"
                    actionOnButtonAnimator.SetTrigger(buttonOnPressedHashID);

                    // faccio partire l'animazione della ghost key che scompare e si distrugge a fine animazione
                    keyManagerAnimator.SetTrigger(disappearHashID);
                }

                // controllo se è finita l'animazione di bottone premuto
                bool isButtonPressedAnimationEnded = actionOnButton.GetIsButtonPressed();

                // dopo aver premuto il bottone e finita la sua animazione, lo resetto
                if (isButtonPressedAnimationEnded)
                {
                    actionOnPrompt.enabled = false;

                    // resetto il bottone actionOn con pressed=false
                    actionOnButton.ResetButtonPressed();
                }

                // quando l'indicatore di chiavi richieste è sparito
                if(isMessageDisappeard)
                {
                    // TODO per sicurezza disabilito anche qui il botton
                    actionOnPrompt.enabled = false;
                    actionOnButton.ResetButtonPressed();

                    // attivo l'oggetto che richiedeva la chiavi
                    foreach (ActivableObjectInterface obj in activableObjectInterfaceList)
                    {
                        obj.ActivateObject();
                    }

                    // aggiorno la struttura json per il ripristino da gameover
                    uniqueItemsManager.UpdateKeyActManagerStatus(id, false);

                    // e infine distruggo il KeyActivationManager
                    Destroy(gameObject, 0);
                }
            }

            // se invece il player non ha abbastanza chiavi
            else
            {
                // se il player preme il tasto azione
                if (actionButton.action.triggered && !isAlreadyPressed)
                {
                    errorSound.Play();

                    isAlreadyPressed = true;

                    // faccio partire l'animazione di bottone premuto con errore, che alla fine dall'animator attiverà la condizione "isButtonPressedAnimationEnded"
                    actionOffButtonAnimator.SetTrigger(buttonOffPressedHashID);
                }

                // controllo se è finita l'animazione di bottone premuto
                bool isButtonPressedAnimationEnded = actionOffButton.GetIsButtonPressed();

                // dopo aver premuto il bottone e finita la sua animazione 
                if (isButtonPressedAnimationEnded && isAlreadyPressed)
                {
                    Debug.Log("Reset bottone OFF");
                    // resetto il bottone actionOff con pressed=false
                    actionOnButton.ResetButtonPressed();
                    isAlreadyPressed = false;
                }

            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // attivo l'Update
            isPlayerInsideTrigger = true;

            // attivo il prompt di azione On
            /////////////actionOnPrompt.SetActive(true);

            // controllo se il player ha abbastanza chiavi
            playerBoltKeys = roboltInteraction.GetCurrentBoltKeys();
            isKeyEnough = (playerBoltKeys >= keyRequired);

            // se il giocatore ha abbastanza chiavi
            if (isKeyEnough)
            {
                // sostituisco il materiale con quelle Emission
                keyGhost.GetComponent<MeshRenderer>().materials = yellowEmissionMaterialsArray;

                // accendo il numero di chiavi
                numberTextRequired.SetActive(false);
                numberTextOn.SetActive(true);

                // faccio comparire il prompt di azione attivabile
                actionOnPrompt.enabled = true;
            }
            else
            {
                // sostituisco il materiale con quelle Emission
                keyGhost.GetComponent<MeshRenderer>().materials = redEmissionMaterialsArray;

                // accendo il numero di chiavi
                numberTextRequired.SetActive(false);
                numberTextNotEnough.SetActive(true);

                // faccio comparire il prompt di azione attivabile
                actionOffPrompt.enabled = true;
            }

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInsideTrigger = false;
            isKeyEnough = false;
            isAlreadyPressed = false;
            playerBoltKeys = 0;

            // annullo tutti i prompt
            actionOnPrompt.enabled = false;
            actionOffPrompt.enabled = false;
            ////////////keyWarning.SetActive(false);

            // ripristino colori e materiali
            keyGhost.GetComponent<MeshRenderer>().materials = keyOffMaterialsArray;
            numberTextRequired.SetActive(true);
            numberTextOn.SetActive(false);
            numberTextNotEnough.SetActive(false);

            // TODO in teoria non serve perchè tutto l'oggettoKeyManager verrà distrutto
            // isAlreadyPressed = false;
        }
    }

    public void SetIsMessageDisappeared()
    {
        isMessageDisappeard = true;
    }

    // chiamata da UniqueItemsManager quando la scena viene caricata, in moda da attivare già eventuali oggetti già attivi dal precedente gameover
    public void ActivateObjectOnLoad()
    {
        // attivo l'oggetto che richiedeva la chiavi
        foreach (ActivableObjectInterface obj in activableObjectInterfaceList)
        {
            obj.ActivateObject();
        }


        // e infine distruggo il KeyActivationManager
        Destroy(gameObject, 0);
    }
}
