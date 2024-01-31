using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PressedButtonManager : MonoBehaviour
{
    [SerializeField] private bool isButtonPressed;

    private void Start()
    {
        isButtonPressed = false;
    }

    public void ActionButtonPressed()
    {
        isButtonPressed = true;
    }

    public void ResetButtonPressed()
    {
        isButtonPressed = false;
    }

    public bool GetIsButtonPressed()
    {
        return isButtonPressed;
    }
}
