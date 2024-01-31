using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    //public AudioSource soundTrack;
    public FollowCamera followCamera;

    public GameObject gameOverEventSystem;

    // chimato da un evento dell'animatoion del GameOver
    public void StartFollowCameraCentered()
    {
        followCamera.SetIsGameOver(true);
    }

    public void ActivateGameOverEventSystem()
    {
        gameOverEventSystem.SetActive(true);
    }
}
