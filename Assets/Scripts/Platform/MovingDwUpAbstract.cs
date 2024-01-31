using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDwUpAbstract : MonoBehaviour
{
    public float speedUp;
    public float speedDown;
    protected bool isDown;

    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Vector3 endPosition;
    public float offSetY;

    protected void MoveDwUp()
    {
        // muovo la piattaforma in giù
        if (isDown)
        {
            if (transform.position.y > endPosition.y)
                transform.position = Vector3.MoveTowards(transform.position, endPosition, (Time.deltaTime * speedUp));
            else
                isDown = false;
        }

        // quando ha raggiunto il punto inferiore impostato esternamente da offSetY, torna su
        else
        {
            if (transform.position.y < startPosition.y)
                transform.position = Vector3.MoveTowards(transform.position, startPosition, (Time.deltaTime * speedDown));

            // quando ha terminato il ciclo di salita/discesa, ricomiancia il giro
            else
            {
                isDown = true;
            }
        }
    }
}
