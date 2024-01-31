using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingDwUp : MovingDwUpAbstract, ActivableObjectInterface
{
    public bool isStopped;

    void Awake()
    {
        isDown = true;

        startPosition = transform.position;
        endPosition = startPosition;
        endPosition.y = startPosition.y - offSetY;
    }

    private void Update()
    {
        if(!isStopped)
            MoveDwUp();
    }

    public void SetIsStopped(bool condition)
    {
        isStopped = condition;
    }

    public void ActivateObject()
    {
        SetIsStopped(false);
    }
}
