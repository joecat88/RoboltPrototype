using UnityEngine;

public class HiddenObjectsActivator : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    private bool firstTrigger;

    // in fase di avvio, disattivo tutti gli oggetti
    private void Start()
    {
        firstTrigger = true;
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }
    }

    // quando il giocatore entra nel trigger, attivo tutti gli oggetti
    void OnTriggerEnter(Collider other)
    {
        if (firstTrigger)
        {
            if (other.gameObject.tag == "Player")
            {
                foreach (GameObject obj in objectsToActivate)
                {
                    obj.SetActive(true);
                }
                firstTrigger = false;
            }
        }
    }
}
