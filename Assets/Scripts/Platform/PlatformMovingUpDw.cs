using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingUpDw : MovingUpDwAbstract
{

    void Start()
    {
        isUp = true;

        startPosition = transform.position;
        endPosition = startPosition;
        endPosition.y = startPosition.y + offSetY;
    }

    private void Update()
    {
        MoveUpDw();
    }
}

