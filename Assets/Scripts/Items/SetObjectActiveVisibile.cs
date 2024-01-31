using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActiveVisibile : MonoBehaviour
{
    public List<GameObject> objectVisibleList;
    public List<GameObject> objectActiveList;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject obj in objectVisibleList)
            {
                obj.GetComponent<MeshRenderer>().enabled = true;
                obj.GetComponent<BoxCollider>().enabled = true;
            }

            foreach (GameObject obj in objectActiveList)
            {
                obj.SetActive(true);
            }
        }

    }
}
