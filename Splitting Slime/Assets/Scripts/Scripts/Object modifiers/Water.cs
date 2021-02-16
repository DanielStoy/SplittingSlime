using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public float slowRate = 1.25f;
    public PlayerController player;
    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        player.myStats.Speed /= slowRate;
    }

    private void OnTriggerExit(Collider other)
    {
        player.myStats.Speed *= slowRate;
    }
}
