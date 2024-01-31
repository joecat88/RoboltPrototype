using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformActivator : MonoBehaviour
{
    public List<GameObject> activableObjectList;
    [SerializeField] private List<ActivableObjectInterface> activableObjectInterfaceList;

    // Start is called before the first frame update
    void Start()
    {
        activableObjectInterfaceList = new List<ActivableObjectInterface>();

        foreach (GameObject obj in activableObjectList)
        {
            activableObjectInterfaceList.Add(obj.GetComponent<ActivableObjectInterface>());
        }

        foreach (ActivableObjectInterface obj in activableObjectInterfaceList)
        {
            obj.ActivateObject();
        }

    }

}
