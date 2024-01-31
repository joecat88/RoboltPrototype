using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float speedUp;
    [SerializeField] private bool isGameOver;
    private bool startMovement;
    [SerializeField] protected Vector3 endPosition;

    void Start()
    {
        isGameOver = false;
        startMovement = true;
    }

    void Update()
    {
        // quando si attiva il game over, la telecamera di sposta verso il basso per mettere il player al centro
        if (isGameOver)
        {
            if (startMovement)
            {
                Vector3 startPosition = transform.position;
                endPosition = startPosition;
                endPosition.y = startPosition.y - Const.FOLLOW_CAMERA_Y_OFFSET;
                startMovement = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, endPosition, (Time.deltaTime * speedUp));
        }
    }

    public void SetIsGameOver(bool condition)
    {
        isGameOver = condition;
    }
}
