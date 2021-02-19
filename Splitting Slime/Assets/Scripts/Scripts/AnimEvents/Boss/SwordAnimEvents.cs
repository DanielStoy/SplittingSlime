using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAnimEvents : MonoBehaviour
{
    private Animator anim;
    PlayerController player;
    [SerializeField]
    private AudioClip swordSwingSound;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }

    //Resets swords animation and trees animation
    public void resetSwing()
    {
        anim.SetInteger("State", 0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(10);
        }
    }

    public void PlayerSwingSound()
    {

    }
}
