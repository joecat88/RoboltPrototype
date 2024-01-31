using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotCheatTrigger : MonoBehaviour
{
    public GearTrapDetector trapDetector;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            trapDetector.SetIsPlayerTryToCheat(false);
        }
    }
}
