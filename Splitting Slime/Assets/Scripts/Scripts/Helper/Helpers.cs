using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public static Helpers instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public float SetAspect()
    {
        float comparableAspect = 16.0f / 9.0f;
        float actualAspect = (float)Screen.width / (float)Screen.height;

        if(Mathf.Abs(comparableAspect - actualAspect) > .03)
        {
            return (CameraManager.instance.mainCam.orthographicSize * comparableAspect) - (CameraManager.instance.mainCam.orthographicSize * actualAspect);
        }
        else
        {
            return 0;
        }
    }
}
