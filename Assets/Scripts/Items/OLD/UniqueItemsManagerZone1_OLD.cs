using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueItemsManagerZone1_OLD : MonoBehaviour
{
    private const int BOLT_KEYS_AMOUNT = 7;
    [SerializeField] private bool[] boltKeysStatusList;
    private const int BOLT_COINS_AMOUNT = 5;
    [SerializeField] private bool[] boltCoinsStatusList;



    private void Start()
    {
        boltKeysStatusList = new bool[BOLT_KEYS_AMOUNT];
        InitializeBoltKeys();
        boltCoinsStatusList = new bool[BOLT_COINS_AMOUNT];
        InitializeBoltCoins();
    }

    // BOLTKEYS
    public void SetBoltKeyStatus(int index, bool condition)
    {
        boltKeysStatusList[index] = condition;
    }

    public bool GetBoltKeyStatus(int index)
    {
        return boltKeysStatusList[index];
    }

    private void InitializeBoltKeys()
    {
        for (int i=0; i < BOLT_KEYS_AMOUNT; i++)
        {
            boltKeysStatusList[i] = true;
        }
    }

    // BOLTCOIN
    public void SetBoltCoinActive(int index, bool condition)
    {
        boltCoinsStatusList[index] = condition;
    }

    public bool GetBoltCoinStatus(int index)
    {
        return boltCoinsStatusList[index];
    }

    private void InitializeBoltCoins()
    {
        for (int i = 0; i < BOLT_COINS_AMOUNT; i++)
        {
            boltCoinsStatusList[i] = true;
        }
    }

}
