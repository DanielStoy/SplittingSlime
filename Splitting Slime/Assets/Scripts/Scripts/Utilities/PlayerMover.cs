using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{

    public Transform moveObject;
    public CameraBehavior cam;
    public Vector3 camMinPos;
    public Vector3 camMaxPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = moveObject.position;
            CameraManager.instance.mainCam.GetComponent<CameraBehavior>().minPosition = camMinPos;
            CameraManager.instance.mainCam.GetComponent<CameraBehavior>().maxPosition = camMaxPos;
            CameraManager.instance.mainCam.transform.position = camMinPos;
        }
    }
}
