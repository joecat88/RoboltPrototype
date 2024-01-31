using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
    // attributi
    [SerializeField] private bool isFirstActivation;
    [SerializeField] private bool isActive;
    [SerializeField] private bool isToReactivate;
    [SerializeField] private bool isReactivateFilled;
    public int id;
   

    // references
    public Animation globeAnimation;
    public Animator animCheckPoint;
    public Animator animRectivateButton;
    private Collider triggerCollider;
    public RoboltController roboltController;
    public ParticleSystem particlesBase;
    public ParticleSystem particlesActivation;
    public UniqueItemsManagerZone1 itemsManager;
    public ReactivateButton reactivateButtonScript;
    public AudioSource audio;
    //public Vector3 spawnPoint;
    public Transform spawnPoint;
    [SerializeField] public InputActionReference actionButton;

    // UI references
    public Image reactivatePromptBackground;
    public Image reactivatePromptFill;

    private static readonly int checkPointOnHashID = Animator.StringToHash("checkPointOn");
    private static readonly int reactivatePressedHashID = Animator.StringToHash("reactivatePressed");

    private void Start()
    {
        if (id == 99)
            throw new Exception("Non hai impostato un ID per il checkpoint");
        //if (spawnPoint == new Vector3(0,0,0))
        //    throw new Exception("Non hai impostato lo SPAWNPOINT per il checkpoint");

        isToReactivate = false;
        isReactivateFilled = false;
        triggerCollider = GetComponent<Collider>();
        //checkPointList = checkPointManager.GetCheckList();

    }

    private void Update()
    {
        if (isToReactivate)
        {
            // se il player sta premendo il bottone, inizio a riempiere il cerchio
            if (actionButton.action.ReadValue<float>() > 0)
            {
                reactivatePromptFill.fillAmount += 1.0f / Const.FILL_BUTTON_DURATION * Time.deltaTime;
            }

            // altimenti inizio a svuotare il cerchio
            else
            {
                reactivatePromptFill.fillAmount -= 1.0f / Const.FILL_BUTTON_DURATION * Time.deltaTime;
            }

            // quando il cerchio è stato riempito
            if (reactivatePromptFill.fillAmount >= 1f)
            {
                isToReactivate = false;
                animRectivateButton.SetTrigger(reactivatePressedHashID);

                ActivateCheckPoint(true);
                DeactivateOtherCheckPoints();
                //reactivateButton.ResetButtonPressed();
            }

        }

        bool isReactivateAnimationEnded = reactivateButtonScript.GetIsButtonPressed();

        if (isReactivateAnimationEnded)
        {
            reactivatePromptBackground.enabled = false;
            reactivatePromptFill.enabled = false;
            reactivateButtonScript.ResetButtonPressed();

        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            // mai attivato
            if (isFirstActivation && !isActive)
            {
                // se è la prima attivazione, aggiorno lo stato locale ed i dati per il json di ripristino gameover
                SetIsFirstActivation(false);
                itemsManager.UpdateChkPtFirstActStatus(id, false);

                ActivateCheckPoint(true);
                DeactivateOtherCheckPoints();
            }


            // da riattivare
            else if (!isFirstActivation && !isActive)
            {
                reactivatePromptBackground.enabled = true;
                reactivatePromptFill.enabled = true;
                isToReactivate = true;
            }

            // già attivo
            else if (!isFirstActivation && isActive)
            {
                //Debug.Log("CHECKPOINT GIÀ ATTIVO");
            }

            // anomalia
            else
                throw new Exception("CASO ANOMALO CHEKCPOINT TRIGGER");
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            reactivatePromptBackground.enabled = false;
            reactivatePromptFill.enabled = false;
            isToReactivate = false;

            reactivatePromptFill.fillAmount = 0f;
            reactivateButtonScript.ResetButtonPressed();
        }
    }
    public void ActivateCheckPoint(bool withAnimation)
    {

        isActive = true;

        if (withAnimation)
        {
            audio.Play();
            animCheckPoint.SetTrigger(checkPointOnHashID);
        }

        globeAnimation.Play();
        particlesActivation.Play();
        particlesBase.Play();
    }

    private void DeactivateOtherCheckPoints()
    {
        List<CheckPoint> checkPointList = itemsManager.GetCheckPointList();

        foreach (CheckPoint obj in checkPointList)
        {
            // disattivo tutti i checkpoint tranne me stesso
            if (obj.GetId() != this.id)
                obj.DeactivateCheckPoint();
        }

        //roboltController.SetLastCheckPoint(spawnPoint);
        roboltController.SetLastCheckPoint(spawnPoint.position);
    }

    public int GetId()
    {
        return id;
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public void DeactivateCheckPoint()
    {
        isActive = false;

        globeAnimation.Stop();
        particlesActivation.Stop();
        particlesBase.Stop();
    }

    // chiamato dal Manager 
    public void SetIsFirstActivation(bool condition)
    {
        isFirstActivation = condition;
    }

    public bool GetIsFirstActivation()
    {
        return isFirstActivation;
    }


}

    // chimata da RoboltInteractions
    //public void ActivateCheckPoint()
    //{
    //    if (isFirstActivation)
    //    {
    //        // se è la prima attivazione, aggiorno i dati per il json di ripristino gameover
    //        itemsManager.UpdateChkPtFirstActStatus(id, false);

    //        // se è la prima attivazione in questa sessione, la disattivo
    //        SetIsFirstActivation(false);
    //    }

    //    if (!isActive)
    //    {
    //        isActive = true;

    //        globeAnimation.Play();
    //        particlesActivation.Play();
    //        particlesBase.Play();

    //        List<CheckPoint> checkPointList = itemsManager.GetCheckList();

    //        foreach (CheckPoint obj in checkPointList)
    //        {
    //            // disattivo tutti i checkpoint tranne me stesso
    //            if (obj.GetId() != this.id)
    //                obj.DeactivateCheckPoint();
    //        }

    //        //roboltController.SetLastCheckPoint(spawnPoint);
    //        roboltController.SetLastCheckPoint(spawnPoint.position);
    //    }
    //}