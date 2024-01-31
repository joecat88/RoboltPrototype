using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed;
    [SerializeField] protected Direction direction;
    public enum Direction { clockwise, counterclockwise }

    private void Start()
    {
        if (direction == Direction.counterclockwise)
            speed *= -1f;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setDirection(Direction direction) 
    {
        this.direction = direction;
    }
}
