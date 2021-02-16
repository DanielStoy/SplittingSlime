using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallDamage : MonoBehaviour
{

    [SerializeField]
    private Transform respawnPoint;
    private bool triggered = false;
    private Transform player;
    private Rigidbody playerRigid;
    private PlayerController playerControl;
    private bool playingStuff = false;

    [SerializeField]
    private float damage = 10;

    private void Start()
    {
        player = PlayerManager.instance.Player.transform;
        playerControl = player.GetComponent<PlayerController>();
        playerRigid = player.GetComponent<Rigidbody>();
        if (this.enabled)
            this.enabled = false;
    }

    private void Update()
    {
        if(player.position.y < transform.position.y && !playingStuff)
        {
            playingStuff = true;
            Fall();
        }
    }

    private void Fall()
    {
        if(playerControl.myStats.Health > damage)
        {
            playerRigid.isKinematic = true;
            playerControl.canPlay = false;
            playerControl.TakeDamage(damage);
            player.position = new Vector3(respawnPoint.position.x, respawnPoint.position.y + 20, respawnPoint.position.z);
            player.DOMoveY(respawnPoint.position.y, 1).OnComplete(DonePlaying);
        }
    }

    private void DonePlaying()
    {
        playerRigid.isKinematic = false;
        playerControl.canPlay = true;
        playingStuff = false;
    }
}
