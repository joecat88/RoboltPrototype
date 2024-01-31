using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        // references
        public GameObject mainMenu;
        public GameObject loadingScreen;
        public Image loadingBar;

        private Animator anim;
        ////////private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
        private AsyncOperation[] scenesToLoad;
        
        private static readonly int LoadingScreenTrigger = Animator.StringToHash("LoadingScreenTrigger");

        public void Start()
        {
            anim = GetComponent<Animator>();
            scenesToLoad = new AsyncOperation[2];
        }

        // chiamato da bottone "Continua Partita"
        public void LoadGameButton()
        {
            anim.SetTrigger(LoadingScreenTrigger);
        }

        // chiamato alla fine da Animator "LoadingSceenAnimator"
        public void LoadGame()
        {

            // TODO mettere i nomi delle scene in una classe di costanti global
            //////AsyncOperation loadAsync = SceneManager.LoadSceneAsync("LV1_SteamStation");
            //scenesToLoad[0] =(SceneManager.LoadSceneAsync("UniqueItemsManager"));
            scenesToLoad[0] = (SceneManager.LoadSceneAsync("LV1_SteamStation"));
            StartCoroutine(ProgressLoadingScreen());

        }
        
        // chaimato da bottone "Nuova partita"
        public void NewGameButton()
        {
            Debug.Log("Per ora non fa niente");
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
                    loadingBar.fillAmount = totalProgress/scenesToLoad.Length;
                    yield return null;
                }
            }
        }

        // chiamato da Animator "LoadingSceenAnimator"
        private void DisableMainMenu()
        {
            mainMenu.SetActive(false);
        }

        // chiamato da Animator "LoadingSceenAnimator"
        private void EnableLoadingScreen()
        {
            loadingScreen.SetActive(true);
        }
        
    }
}
