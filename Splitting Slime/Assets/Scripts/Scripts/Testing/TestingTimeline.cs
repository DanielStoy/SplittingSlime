using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TestingTimeline : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerSword;
    public PlayableDirector dir;
    void Start()
    {
        Player = PlayerManager.instance.Player.transform.GetChild(0).gameObject;
        PlayerSword = PlayerManager.instance.Player.transform.GetChild(1).gameObject;
        dir = GetComponent<PlayableDirector>();
        var timeLineAsset = (TimelineAsset)dir.playableAsset;
        foreach(var item in timeLineAsset.outputs)
        {
            if (item.streamName == "PlayerTrack")
                dir.SetGenericBinding(item.sourceObject, Player);
            else if (item.streamName == "WeaponTrack")
                dir.SetGenericBinding(item.sourceObject, PlayerSword);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dir.Play();
        }
    }
}
