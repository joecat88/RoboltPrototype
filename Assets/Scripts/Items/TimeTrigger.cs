using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : ItemBase
{
    MeshRenderer childMesh;

    public void Start()
    {
        childMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
    }
    public override void setInvisible()
    {
        childMesh.enabled = false;
    }
}
