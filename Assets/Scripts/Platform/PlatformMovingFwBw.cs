using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingFwBw : MovingFwBwAbstract
{
    private void Start()
    {
        timer = 0;
        isForward = true;
        if (speed == 0)
            throw new Exception("Non hai impostato la velocità");
        if (endTime == 0)
            throw new Exception("Non hai impostato la durata");
    }

    private void Update()
    {
        MoveFwBw();
    }

    //public Vector3 start;
    //public Vector3 end;
    //// Start is called before the first frame update
    //private bool isForward;
    //void Start()
    //{
    //    isForward = true;
    //    if (start == new Vector3(0, 0, 0) || end == new Vector3(0, 0, 0))
    //        throw new Exception("Non hai impostato i start/end");
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if (isForward)
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, end, (Time.deltaTime * 3f));
    //        if (transform.position == end)
    //            isForward = false;
    //    }
    //    else
    //    {
    //        transform.position = Vector3.MoveTowards(transform.position, start, (Time.deltaTime * 3f));
    //        if (transform.position == start)
    //            isForward = true;
    //    }
    //}
}
