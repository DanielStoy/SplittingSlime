using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PlayerSpawner : MonoBehaviour
{
    private GameObject player;
    private GameObject playerGFX;
    private PlayerController playerControl;
    private GameObject PlayerSword;
    private PlayableDirector dir;
    public bool teleported = false;
    [SerializeField]
    private Vector3 CameraChangeMin, cameraChangeMax;
    [SerializeField]
    private TriggerEvent eventToTrigger;

    public void SpawnPlayer()
    {
        player = PlayerManager.instance.Player;
        if (player == null)
        {
            player = Instantiate(PlayerManager.instance.PlayerPrefab, transform.position, transform.rotation);
            PlayerManager.instance.Player = player;
            playerControl = player.GetComponent<PlayerController>();
        }
        else
        {
            playerControl = player.GetComponent<PlayerController>();
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            player.transform.localScale = transform.localScale;
            playerControl.resetPosition(transform.position);
            playerControl.setFacingLeft();

        }
        GetDirector();
        SetupDirector();
        SetupCamera();
        Trigger();
    }

    private void GetDirector()
    {
        if (GetComponents<PlayableDirector>().Length > 0)
        {
            if (!teleported)
            {
                dir = GetComponents<PlayableDirector>()[0];
            }
            else
            {
                dir = GetComponents<PlayableDirector>()[1];
            }
        }
        else
        {
            dir = null;
        }
    }

    private void SetupDirector()
    {
        if (dir != null)
        {
            playerGFX = PlayerManager.instance.Player.transform.GetChild(0).gameObject;
            PlayerSword = PlayerManager.instance.Player.transform.GetChild(1).gameObject;
            var timeLineAsset = (TimelineAsset)dir.playableAsset;
            foreach (var item in timeLineAsset.outputs)
            {
                if (item.streamName == "PlayerTrack")
                    dir.SetGenericBinding(item.sourceObject, playerGFX);
                else if (item.streamName == "WeaponTrack")
                    dir.SetGenericBinding(item.sourceObject, PlayerSword);
            }
            DisablePlayer();
        }
    }

    private void SetupCamera()
    {
        if(CameraChangeMin != Vector3.zero)
        {
            CameraBehavior behavior = CameraManager.instance.mainCam.GetComponent<CameraBehavior>();
            behavior.minPosition = CameraChangeMin;
            behavior.maxPosition = cameraChangeMax;
        }
    }

    private void Trigger()
    {
        if (eventToTrigger != null)
            eventToTrigger.Trigger();
    }

    public void EnablePlayer()
    {
        if (playerControl == null)
        {
            playerControl = PlayerManager.instance.Player.GetComponent<PlayerController>();
        }
        playerControl.canPlay = true;
    }

    public void DisablePlayer()
    {
        if(playerControl == null)
        {
            playerControl = PlayerManager.instance.Player.GetComponent<PlayerController>();
        }
        playerControl.canPlay = false;
    }
}
