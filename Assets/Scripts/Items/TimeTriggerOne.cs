using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTriggerOne : TimeTriggerBase
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
        InstantiatePlatform(platformRevolvingTimeCW_M, position, Quaternion.identity, true, 500, CW, GH, 4);

        position = transformList[1].position;
        InstantiatePlatform(platformRevolvingTimeCCW_S, position, Quaternion.identity, true, 300, CW, GH, 6);

        position = transformList[2].position;
        InstantiatePlatform(platformRevolvingTimeCW_S, position, Quaternion.identity, true, 380, CW, GH, 7);

    }

}

//GameObject gearHorS_1 = Instantiate(gearHorzSmallPrefab, new Vector3(-48.7159996f, 15.0030003f, -6.15500021f), Quaternion.identity);
//gearHorS_1.AddComponent<RotateY>();
//gearHorS_1.AddComponent<GearManagerHorz>();
//gearHorS_1.GetComponent<GearManagerHorz>().SetTotSec(8);
//gearHorS_1.GetComponent<GearManagerHorz>().TriggerCountDown(); 