using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManagerOLD : MonoBehaviour
{
    public RoboltInteractions interaction;
    public AudioSource soundTrack;

    // chimata dall'animator a inizio transizione di entrata
    public void GameOverOn()
    {
        // TODO cancellare
        //interaction.GameOverOn();
    }

    // chimata dall'animator a fine transizione di uscita
    public void GameOverOff()
    {
        // TODO cancellare
        //interaction.GameOverOff();
        soundTrack.Play();
    }
}
