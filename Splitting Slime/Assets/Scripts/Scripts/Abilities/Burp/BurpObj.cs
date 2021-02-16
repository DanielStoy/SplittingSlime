using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurpObj : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHitHandler>().applyDamage(10 * player.myStats.level);
        }
    }
}
