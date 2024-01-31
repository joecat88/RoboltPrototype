using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public RoboltInteractions interaction;

    // chimata dall'animator a inizio transizione di entrata
    public void PauseOn()   
    {
        interaction.PauseOn();
    }

    // chimata dall'animator a fine transizione di uscita
    public void PauseOff()
    {
        interaction.PauseOff();
    }
}
