using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    // Start is called before the first frame update
    public void loadLevel1() {

        SceneManager.LoadScene("SteamCityDayScene");
    }
}
