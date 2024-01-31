using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CountDownAbstract : MonoBehaviour
{
    private static float PULSING_COUNTDOWN = 2.5f;
    protected static float PULSING_INTERVAL = 0.3f;

    public enum GearType { HOR_CW, HOR_CCW, VERTICAL };
    protected GearType gearType;

    protected abstract IEnumerator RedPulse();

    private float totSec;
    private bool isCountDown = false;
    private bool isAlreadyTriggered = false;
    private float beforePulsingSec;

    protected bool isPulsing;

    protected Color pulsingColor;
    protected Color originalColor;

    //protected Transform player;


    void Update()
    {
        if (isCountDown && totSec > 0f)
        {
            // calcolo il tempo prima di inizare a lampeggiare
            beforePulsingSec = totSec - PULSING_COUNTDOWN;
            if (beforePulsingSec < 0)
                beforePulsingSec = 0;

            // rimetto a false per fare in modo che non venga più eseguita
            isCountDown = false;
            StartCoroutine(StartCountDown());
        }
    }

    public void TriggerCountDown() 
    {
        // mi assicuro possa essere chiamata una sola volta
        if (!isAlreadyTriggered) 
            {
                isAlreadyTriggered = true;
                isCountDown = true;
            }
    }

    public void SetTotSec(float totSec)
    {
        this.totSec = totSec;
    }

    //public void SetPlayer(Transform player)
    //{
    //    this.player = player;
    //}

    // Metodo che avvia il conto alla rovescia per l'oggetto che implmenta la classe, e avvia l'effetto pulsing quando mancano COOL_DOWN secondi
    protected IEnumerator StartCountDown()
    {
        yield return new WaitForSeconds(beforePulsingSec);
        isPulsing = true;
        StartCoroutine(RedPulse());
        yield return new WaitForSeconds(PULSING_COUNTDOWN);
        isPulsing = false;
    }

}
