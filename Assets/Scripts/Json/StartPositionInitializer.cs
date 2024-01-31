using System;
using System.IO;
using UnityEngine;

public class StartPositionInitializer : MonoBehaviour
{
    private string jsonPath;
    [SerializeField] private LastChkPtDataListJson startPositionListJson;

    void Start()
    {

        jsonPath = Application.streamingAssetsPath + "/LastCheckPoint.json";
        startPositionListJson = new LastChkPtDataListJson();
        InitializeJsonFile();
    }

    [ContextMenu("InitializeStartPosition")]
    // scrive nel json la posizione iniziale del player
    void InitializeJsonFile()
    {
        LastChkPtData[] startPositionData = new LastChkPtData[1];
        startPositionData[0] = new LastChkPtData() { xPos = -128.8f, yPos = 9.1f, zPos = -6.13f };

        startPositionListJson = new LastChkPtDataListJson() { lastChkPtData = startPositionData };

        using (StreamWriter stream = new StreamWriter(jsonPath))
        {
            String jsonRawFile = JsonUtility.ToJson(startPositionListJson);
            stream.Write(jsonRawFile);
        }
    }
}


// Vector3(-128.8,9.1,-6.13)