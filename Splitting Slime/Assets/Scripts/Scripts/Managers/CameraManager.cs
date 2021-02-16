using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager instance;
    public Camera mainCam;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        if(mainCam == null)
        {
            if (Camera.main != null)
            {
                mainCam = Camera.main;
            }
        }
    }
}
