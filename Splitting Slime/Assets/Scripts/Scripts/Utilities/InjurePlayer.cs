using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjurePlayer : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    private float damage = 10;
    void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
