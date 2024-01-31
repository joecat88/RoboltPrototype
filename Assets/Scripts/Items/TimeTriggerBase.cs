using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeTriggerBase : MonoBehaviour
{
    // Metodo che viene richiamato dal Rob0ltInteractions
    public abstract void StartTimeTrial();

    protected static Rotation.Direction CW = Rotation.Direction.clockwise;
    protected static Rotation.Direction CCW = Rotation.Direction.counterclockwise;
    protected static PlatformType GH = PlatformType.gearHorz;
    protected static PlatformType GV = PlatformType.gearVert;

    protected enum PlatformType { gearHorz, gearVert }

    //public Transform player;

    public void ManageTimeTrial(Action callback,float seconds)
    {
        callback.Invoke();
        //InstantiatePlatforms();
        StartCoroutine(WaitEndTimeTrial(seconds));
    }

    private IEnumerator WaitEndTimeTrial(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        gameObject.GetComponent<Collider>().enabled = true;

    }

    // Crea un'istanza del prefab in input, che può essere fermo oppure in rotazione, e con un timeout
    protected void InstantiatePlatform(GameObject objPrefab, Vector3 position, Quaternion rotation, bool isRotating, float speedRotation, Rotation.Direction direction, PlatformType type, float timeOut)
    {
        GameObject obj = Instantiate(objPrefab, position, rotation);
        
        switch (type) 
        {
            case PlatformType.gearHorz:

                if (isRotating)
                {
                    obj.AddComponent<RotateY>();
                    obj.GetComponent<RotateY>().setDirection(direction);
                    obj.GetComponent<RotateY>().setSpeed(speedRotation);
                }

                //obj.transform.GetChild(0).GetComponent<CountDownAbstract>().SetPlayer(player);
                obj.GetComponent<GearManagerHorz>().SetTotSec(timeOut);
                obj.GetComponent<GearManagerHorz>().TriggerCountDown();

                break;

            //case PlatformType.gearVert:

            //    if (isRotating)
            //    {
            //        obj.transform.GetChild(0).gameObject.AddComponent<RotateX>();
            //        obj.transform.GetChild(0).gameObject.GetComponent<RotateX>().setDirection(direction);
            //    }

            //    obj.GetComponent<GearManagerVert>().SetTotSec(timeOut);
            //    obj.GetComponent<GearManagerVert>().TriggerCountDown();

            //    break;
        }
      
    }
}
