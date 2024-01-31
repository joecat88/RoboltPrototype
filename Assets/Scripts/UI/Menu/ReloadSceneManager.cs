using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReloadSceneManager : MonoBehaviour
{
    private Animator anim;
    private AsyncOperation[] scenesToLoad;
    public Image loadingBar;
    public GameObject gameOverScreen;

    private static readonly int reloadingScreenTrigger = Animator.StringToHash("ReloadingScreenTrigger");

    private void Start()
    {
        anim = GetComponent<Animator>();
        scenesToLoad = new AsyncOperation[Const.SCENES_AMOUNT];
    }

    public void ReloadSceneButton()
    {
        anim.SetTrigger(reloadingScreenTrigger);
    }

    // chiamato alla fine da Animator "ReloadingScreenAnimator"
    public void ReloadScene()
    { 
        scenesToLoad[0] = (SceneManager.LoadSceneAsync("LV1_SteamStation"));
        StartCoroutine(ProgressLoadingScreen());
    }

    // Metodo che gestisce la progressione della barra del caricamento
    private IEnumerator ProgressLoadingScreen()
    {
        float totalProgress = 0;

        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            while (!scenesToLoad[i].isDone)
            {
                totalProgress += scenesToLoad[i].progress;
                loadingBar.fillAmount = totalProgress / scenesToLoad.Length;
                yield return null;
            }
        }
    }

    public void DisableGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }
}
