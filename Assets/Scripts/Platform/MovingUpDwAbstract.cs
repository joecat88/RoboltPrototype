using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUpDwAbstract : MonoBehaviour
{
    public float speedUp;
    public float speedDown;
    protected bool isUp;

    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected Vector3 endPosition;
    public float offSetY;

    protected void MoveUpDw()
    {
        // muovo la piattaforma in su
        if (isUp)
        {
            if (transform.position.y < endPosition.y)
                transform.position = Vector3.MoveTowards(transform.position, endPosition, (Time.deltaTime * speedUp));
            else
                isUp = false;
        }

        // quando ha raggiunto l'apice impostato esternamente da offSetY, torna giù
        else
        {
            if (transform.position.y > startPosition.y)
                transform.position = Vector3.MoveTowards(transform.position, startPosition, (Time.deltaTime * speedDown));

            // quando ha terminato il ciclo di salita/discesa, ricomiancia il giro
            else
            {
                isUp = true;
            }
        }
    }
}
