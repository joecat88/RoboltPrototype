using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UniqueItemsManagerZone1 : MonoBehaviour
{
    // array degli oggetti presenti nella senza ZONA 1
    public GameObject[] boltKeys;
    public GameObject[] boltCoins;
    public GameObject[] keyActManagers;
    public GameObject[] checkPoints;

    [SerializeField] private List<CheckPoint> checkPointList;

    // json
    [SerializeField] private UniqueItemsDataListJson uniqueItemsFromJson;
    [SerializeField] private BoltKeyData[] boltKeyDataToJson;
    [SerializeField] private BoltCoinData[] boltCoinDataToJson;
    [SerializeField] private KeyActManagerData[] keyActManagerDataToJson;
    [SerializeField] private ChkPtFirstActData[] chkPtFirstActToJson;

    private string jsonPath;
    private bool isKeyCoinsDataLoaded;

    void Start()
    {
        
        jsonPath = Application.streamingAssetsPath + "/UniqueItems_ZONE1.json";
        uniqueItemsFromJson = new UniqueItemsDataListJson();
        isKeyCoinsDataLoaded = false;

        // inizializzo le strutture dati per il json
        InitializeDataForJson();

        // leggo i dati dal json relativi all'ultima sessione (tutto "true" nel caso di nuova partita)
        ReadDataFromJson();

        SetBoltKeysStatus();
        SetBoltCoinsStatus();
        isKeyCoinsDataLoaded = true;
        SetKeyActManagerStatus();
        SetCheckPointsStatus();   
    }

    private void ReadDataFromJson()
    {
        using (StreamReader stream = new StreamReader(jsonPath))
        {
            String jsonRawFile = stream.ReadToEnd();
            uniqueItemsFromJson = JsonUtility.FromJson<UniqueItemsDataListJson>(jsonRawFile);
        }
    }

    //////////////////// SETTER

    // imposto lo stato attivo/disattivo in base ai dati letti dal json
    private void SetBoltKeysStatus()
    {
        if (uniqueItemsFromJson.boltKeyData.Length != boltKeys.Length)
            throw new Exception("Il numero di boltKeys non corrisponde a quello nel json");

        for (int i = 0; i < boltKeys.Length; i++)
        {
            // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
            bool valueFromJson = uniqueItemsFromJson.boltKeyData[i].value;
            boltKeys[i].SetActive(valueFromJson);
            boltKeyDataToJson[i] = new BoltKeyData() { value = valueFromJson };
        }
    }

    // imposto lo stato attivo/disattivo in base ai dati letti dal json
    private void SetBoltCoinsStatus()
    {
        if (uniqueItemsFromJson.boltCoinData.Length != boltCoins.Length)
            throw new Exception("Il numero di boltCoins non corrisponde a quello nel json");

        for (int i = 0; i < boltCoins.Length; i++)
        {
            // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
            bool valueFromJson = uniqueItemsFromJson.boltCoinData[i].value;
            boltCoins[i].SetActive(valueFromJson);
            boltCoinDataToJson[i] = new BoltCoinData() { value = valueFromJson };
        }
    }

    private void SetKeyActManagerStatus()
    {
        if (uniqueItemsFromJson.keyActManagerData.Length != keyActManagers.Length)
            throw new Exception("Il numero di keyActManagers non corrisponde a quello nel json");

        for (int i = 0; i < keyActManagers.Length; i++)
        {
            // se è false, allora il keyActivatorManager deve essere spento e attivati i suoi gameObject
            bool valueFromJson = uniqueItemsFromJson.keyActManagerData[i].value;
            if (!valueFromJson)
            {
                keyActManagers[i].GetComponent<KeyActivationManager>().ActivateObjectOnLoad();
            }

            keyActManagerDataToJson[i] = new KeyActManagerData() { value = valueFromJson };
        }
    }

    private void SetCheckPointsStatus()
    {
        if (uniqueItemsFromJson.chkPtFirstActData.Length != checkPoints.Length)
            throw new Exception("Il numero di checkPoints non corrisponde a quello nel json");

        for (int i = 0; i < checkPoints.Length; i++)
        {
            // aggiungo lo script del checkPoint alla lista
            checkPointList.Add(checkPoints[i].GetComponent<CheckPoint>());

            // leggo dal json la condizione di prima attivazione e la imposto nel checkPoint
            bool isFirstActivation = uniqueItemsFromJson.chkPtFirstActData[i].value;
            checkPoints[i].GetComponent<CheckPoint>().SetIsFirstActivation(isFirstActivation);

            chkPtFirstActToJson[i] = new ChkPtFirstActData() { value = isFirstActivation };
        }
    }

    // chiamato da RoboltInteraction quando il player prende una boltKey
    public void UpdateBoltKeyStatus(int index, bool condition)
    {
        boltKeyDataToJson[index] = new BoltKeyData() { value = condition };
    }

    // chiamato da RoboltInteraction quando il player prende un boltCoin
    public void UpdateBoltCoinStatus(int index, bool condition)
    {
        boltCoinDataToJson[index] = new BoltCoinData() { value = condition };
    }

    // chiamato da KeyActivationManager quando il player prende attiva un meccanismo
    public void UpdateKeyActManagerStatus(int index, bool condition)
    {
        keyActManagerDataToJson[index] = new KeyActManagerData() { value = condition };
    }

    // chiamato da CheckPoint quando viene attivato per la prima volta
    public void UpdateChkPtFirstActStatus(int index, bool condition)
    {
        chkPtFirstActToJson[index] = new ChkPtFirstActData() { value = condition };
    }

    //////////////////// GETTER

    public List<CheckPoint> GetCheckPointList()
    {
        return checkPointList;
    }

    public BoltKeyData[] GetBoltKeyData()
    {
        return boltKeyDataToJson;
    }

    public BoltCoinData[] GetBoltCoinData()
    {
        return boltCoinDataToJson;
    }

    public bool GetIsKeyCoinsDataLoaded()
    {
        return isKeyCoinsDataLoaded;
    }

    //////////////////// JSON MANAGEMENT

    // chiamato da RoboltIntercation quando c'è un gameover, per fare in modo che alla ripresa della partita ci siano solo gli oggetti non ancora presi
    [ContextMenu("WriteToJson")]
    public void WriteDataToJson()
    {
        UniqueItemsDataListJson uniqueItemsDataList = new UniqueItemsDataListJson()
        {
            boltKeyData = boltKeyDataToJson,
            boltCoinData = boltCoinDataToJson,
            keyActManagerData = keyActManagerDataToJson,
            chkPtFirstActData = chkPtFirstActToJson
        };

        using (StreamWriter stream = new StreamWriter(jsonPath))
        {
            String jsonRawFile = JsonUtility.ToJson(uniqueItemsDataList);
            stream.Write(jsonRawFile);
        }
    }

    private void InitializeDataForJson()
    {
        boltKeyDataToJson = new BoltKeyData[Const.BOLT_KEYS_AMOUNT];
        boltCoinDataToJson = new BoltCoinData[Const.BOLT_COINS_AMOUNT];
        keyActManagerDataToJson = new KeyActManagerData[Const.KEY_ACT_MGR_AMOUNT];
        chkPtFirstActToJson = new ChkPtFirstActData[Const.CHK_PT_AMOUNT];
    }

    // inizializzo i dati con cui aggiornare il json in caso di gameover
    private void InizializeDataToJson2()
    {
        //boltKeyDataToJson = new BoltKeyData[Const.BOLT_KEYS_AMOUNT];
        //for (int i = 0; i < Const.BOLT_KEYS_AMOUNT; i++)
        //    boltKeyDataToJson[i] = new BoltKeyData() { value = true };

        //boltCoinDataToJson = new BoltCoinData[Const.BOLT_COINS_AMOUNT];
        //for (int i = 0; i < Const.BOLT_COINS_AMOUNT; i++)
        //    boltCoinDataToJson[i] = new BoltCoinData() { value = true };

        //keyActManagerDataToJson = new KeyActManagerData[Const.KEY_ACT_MGR_AMOUNT];
        //for (int i = 0; i < Const.KEY_ACT_MGR_AMOUNT; i++)
        //    keyActManagerDataToJson[i] = new KeyActManagerData() { value = true };

        //chkPtFirstActToJson = new ChkPtFirstActData[Const.CHK_PT_AMOUNT];
        //for (int i = 0; i < Const.CHK_PT_AMOUNT; i++)
        //    chkPtFirstActToJson[i] = new ChkPtFirstActData() { value = true };

        //boltKeyDataToJson = new BoltKeyData[3];
        //boltKeyDataToJson[0] = new BoltKeyData() { value = true };
        //boltKeyDataToJson[1] = new BoltKeyData() { value = true };
        //boltKeyDataToJson[2] = new BoltKeyData() { value = true };

        //boltCoinDataToJson = new BoltCoinData[5];
        //boltCoinDataToJson[0] = new BoltCoinData() { value = true };
        //boltCoinDataToJson[1] = new BoltCoinData() { value = true };
        //boltCoinDataToJson[2] = new BoltCoinData() { value = true };
        //boltCoinDataToJson[3] = new BoltCoinData() { value = true };
        //boltCoinDataToJson[4] = new BoltCoinData() { value = true };

        //keyActManagerDataToJson = new KeyActManagerData[3];
        //keyActManagerDataToJson[0] = new KeyActManagerData() { value = true };
        //keyActManagerDataToJson[1] = new KeyActManagerData() { value = true };
        //keyActManagerDataToJson[2] = new KeyActManagerData() { value = true };

        //chkPtFirstActToJson = new KeyActivationManager

    }
}



//{
//    public GameObject[] boltKeys;
//public GameObject[] boltCoins;
////////private UniqueItemsManagerZone1 manager;

//private string managerPath;
//public TextAsset textAsset;
//public BoltKeyDataList listContainer;

//[Serializable]
//public class BoltKeyData
//{
//    public bool value;
//}

//[Serializable]
//public class BoltKeyDataList
//{
//    public BoltKeyData[] boltKeyData;
//}

//// Start is called before the first frame update
//void Start()
//{
//    /////manager = GameObject.Find("UniqueItemsManager_ZONE1").GetComponent<UniqueItemsManagerZone1>();
//    //managerPath = "JSON/UniqueItems_ZONE1.json";

//    textAsset = Resources.Load<TextAsset>("JSON/UniqueItems_ZONE1");

//    ActivateKeys();
//    ActivateCoins();
//}

//private void ActivateKeys()
//{

//    listContainer = new BoltKeyDataList();
//    //using (StreamReader stream = new StreamReader(TextAsset))
//    //{
//    //    String json = stream.ReadToEnd();
//    listContainer = JsonUtility.FromJson<BoltKeyDataList>(textAsset.text);
//    //}
//    //Debug.Log("KEY "+listContainer.boltKeyData[1].value);

//    for (int i = 0; i < boltKeys.Length; i++)
//    {
//        // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
//        //boltKeys[i].SetActive(manager.GetBoltKeyStatus(i));
//        //Debug.Log("KEY "+i+" = "+ manager.GetBoltKeyStatus(i));
//    }
//}

//private void ActivateCoins()
//{
//    for (int i = 0; i < boltCoins.Length; i++)
//    {
//        // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
//        //boltCoins[i].SetActive(manager.GetBoltCoinStatus(i));
//        //Debug.Log("COIN " +i+ " = " + manager.GetBoltCoinStatus(i));
//    }
//}

//[ContextMenu("PUTJSON")]
//void PutJson()
//{
//    BoltKeyDataList newList = new BoltKeyDataList();
//    BoltKeyData boltKeydata = new BoltKeyData();

//    newList.boltKeyData = new BoltKeyData[4];
//    boltKeydata.value = false;
//    newList.boltKeyData[0] = boltKeydata;
//    boltKeydata.value = true;
//    newList.boltKeyData[1] = boltKeydata;
//    boltKeydata.value = false;
//    newList.boltKeyData[2] = boltKeydata;
//    boltKeydata.value = false;
//    newList.boltKeyData[3] = boltKeydata;



//}
//}




//private void ActivateKeys()
//{
//    for (int i = 0; i < boltKeys.Length; i++)
//    {
//        // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
//        boltKeys[i].SetActive(manager.GetBoltKeyStatus(i));
//        Debug.Log("KEY " + i + " = " + manager.GetBoltKeyStatus(i));
//    }
//}

//private void ActivateCoins()
//{
//    for (int i = 0; i < boltCoins.Length; i++)
//    {
//        // leggo lo stato di attivo (true) o disattivo (false) dal manager e lo applico all'oggetto
//        boltCoins[i].SetActive(manager.GetBoltCoinStatus(i));
//        Debug.Log("COIN " + i + " = " + manager.GetBoltCoinStatus(i));
//    }
//}
