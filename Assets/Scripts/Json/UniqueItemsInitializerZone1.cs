using System;
using System.IO;
using UnityEngine;

public class UniqueItemsInitializerZone1 : MonoBehaviour
{

    private string jsonPath;
    [SerializeField] private UniqueItemsDataListJson uniqueItemsDataList;


    // Start is called before the first frame update
    void Start()
    {

        jsonPath = Application.streamingAssetsPath + "/UniqueItems_ZONE1.json";
        uniqueItemsDataList = new UniqueItemsDataListJson();
        InitializeJsonFile();
    }

    [ContextMenu("InitializeItems")]
    // Stato iniziale del file json, con tutti gli oggetti attivi=true
    void InitializeJsonFile()
    {
        BoltKeyData[] boltKeyData = new BoltKeyData[Const.BOLT_KEYS_AMOUNT];  
        for(int i=0; i< Const.BOLT_KEYS_AMOUNT; i++)
            boltKeyData[i] = new BoltKeyData() { value = true };

        BoltCoinData[] boltCoinData = new BoltCoinData[Const.BOLT_COINS_AMOUNT];
        for (int i = 0; i < Const.BOLT_COINS_AMOUNT; i++)
            boltCoinData[i] = new BoltCoinData() { value = true };

        KeyActManagerData[] keyActManagerData = new KeyActManagerData[Const.KEY_ACT_MGR_AMOUNT];
        for (int i = 0; i < Const.KEY_ACT_MGR_AMOUNT; i++)
            keyActManagerData[i] = new KeyActManagerData() { value = true };

        ChkPtFirstActData[] chkPtFirstActData = new ChkPtFirstActData[Const.CHK_PT_AMOUNT];
        for (int i = 0; i < Const.CHK_PT_AMOUNT; i++)
            chkPtFirstActData[i] = new ChkPtFirstActData() { value = true };

        uniqueItemsDataList = new UniqueItemsDataListJson() 
        { 
            boltKeyData = boltKeyData, 
            boltCoinData = boltCoinData, 
            keyActManagerData = keyActManagerData, 
            chkPtFirstActData = chkPtFirstActData 
        };

        using (StreamWriter stream = new StreamWriter(jsonPath))
        {
            String jsonRawFile = JsonUtility.ToJson(uniqueItemsDataList);
            stream.Write(jsonRawFile);
        }
    }

}
