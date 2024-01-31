using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTrapDetector : MonoBehaviour
{
    public SpawnElectricGearTrap trap;
    private bool isPlayerTryToCheat;
    // Start is called before the first frame update
    void Start()
    {
        isPlayerTryToCheat = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // solo se il player sta provando a saltare per evitare la sfida platform a tempo
            if(isPlayerTryToCheat)
                trap.SpawnGear();
        }
    }

    public void SetIsPlayerTryToCheat(bool condition)
    {
        isPlayerTryToCheat = condition;
    }
}
