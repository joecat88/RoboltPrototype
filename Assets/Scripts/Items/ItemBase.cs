using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{

    protected void destroy() 
    {
        Destroy(gameObject, 0);
    }

    public virtual void setInvisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void setVisible()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void disableBoxCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    public void enableBoxCollider()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
