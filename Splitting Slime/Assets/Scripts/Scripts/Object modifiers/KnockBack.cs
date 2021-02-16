using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private PlayerController player;
    public bool damage = false;
    public int damageAmount = 10;
    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (damage){
                player.TakeDamage(damageAmount);
            }
            player.applyKnockback();
        }
    }
}
