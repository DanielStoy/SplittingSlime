using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField]
    private TriggerEvent eventToTrigger;
    [SerializeField]
    private Transform objToTeleportTo;
    private Transform player;
    private Transform cam;

    private void Start()
    {
        player = PlayerManager.instance.Player.transform;
        cam = CameraManager.instance.mainCam.transform; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.position = objToTeleportTo.position;
            if (eventToTrigger != null)
                eventToTrigger.Trigger();
        }
    }
}
