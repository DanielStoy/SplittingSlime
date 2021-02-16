using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAndKnockBack : MonoBehaviour
{
    private PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(2.5f);
            player.applyKnockback(40);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
