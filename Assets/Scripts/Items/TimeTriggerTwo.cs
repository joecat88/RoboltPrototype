using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTriggerTwo : TimeTriggerBase
{
    public GameObject platformRevolvingTimeCW_S;
    public GameObject platformRevolvingTimeCW_M;
    public GameObject platformRevolvingTimeCCW_S;
    public GameObject platformRevolvingTimeCCW_M;


    public Transform[] transformList;

    public override void StartTimeTrial()
    {
        ManageTimeTrial(InstantiateAllPlatforms, 7);
    }

    public void InstantiateAllPlatforms()
    {

        Vector3 position;

        position = transformList[0].position;
        InstantiatePlatform(platformRevolvingTimeCW_M, position, Quaternion.identity, true, 280, CW, GH, 6);

        position = transformList[1].position;
        InstantiatePlatform(platformRevolvingTimeCCW_S, position, Quaternion.identity, true, 300, CW, GH, 7);

    }

}
