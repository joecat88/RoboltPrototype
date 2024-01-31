using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTitleManager : MonoBehaviour
{
    public JetPackManager jetPackManager;
    public Image title;
    public Animator animator;

    // chimato dal trigger
    public void ActivateEndTitle()
    {
        title.enabled = true;
        jetPackManager.SetCameraTargetNull();
        jetPackManager.SetIsPlayerControl(false);
        StartCoroutine(ShowEndTitle());
    }


    IEnumerator ShowEndTitle()
    {
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("EndTitle");
    }
}
