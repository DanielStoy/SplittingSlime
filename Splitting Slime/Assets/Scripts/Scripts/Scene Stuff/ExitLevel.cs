using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ExitLevel : MonoBehaviour
{
    private GameObject player;
    private GameObject playerSword;
    private PlayerController playerControl;
    private PlayableDirector dir;
    private bool triggered = false;
    public SceneIndex chosenIndex;

    void Start()
    {
        dir = GetComponent<PlayableDirector>();
        playerControl = PlayerManager.instance.Player.GetComponent<PlayerController>();
        if (dir != null)
        {
            player = PlayerManager.instance.Player.transform.GetChild(0).gameObject;
            playerSword = PlayerManager.instance.Player.transform.GetChild(1).gameObject;
            var timeLineAsset = (TimelineAsset)dir.playableAsset;
            foreach (var item in timeLineAsset.outputs)
            {
                if (item.streamName == "PlayerTrack")
                    dir.SetGenericBinding(item.sourceObject, player);
                else if (item.streamName == "WeaponTrack")
                    dir.SetGenericBinding(item.sourceObject, playerSword);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !triggered)
        {
            if (dir != null)
            {
                dir.Play();
                playerControl.canPlay = false;
                playerControl.DisableShadow();
            }
            else
                teleportPlayer();
        }
    }

    public void teleportPlayer()
    {
        playerControl.canPlay = true;
        SceneManagerScript.instance.loadScene(chosenIndex);
        AudioManager.instance.StopMusic();
    }
}
