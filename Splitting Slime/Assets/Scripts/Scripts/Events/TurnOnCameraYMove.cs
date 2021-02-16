using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnCameraYMove : TriggerEvent
{
    CameraBehavior behavior;
    [SerializeField]
    float yChange = 0;
    float initalY = 0;
    private void Start()
    {
        behavior = CameraManager.instance.mainCam.GetComponent<CameraBehavior>();
    }

    public override void Trigger()
    {
        behavior.UnlockY = false;
        behavior.minPosition.y = initalY;
        behavior.TeleportToPlayerNoZChange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            behavior.UnlockY = true;
            initalY = behavior.minPosition.y;
            behavior.minPosition.y = yChange;
        }
    }
}
