using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerAnimEvents : MonoBehaviour
{
    public TreeBossMoveController control;
    public GameObject LazerReady;
    public GameObject LazerGFX;
    PlayerController player;
    [SerializeField]
    private AudioClip lazerSound;
    private void Start()
    {
        control = transform.root.GetComponent<TreeBossMoveController>();
        LazerReady = transform.GetChild(2).gameObject;
        LazerGFX = transform.GetChild(1).gameObject;
        player = PlayerManager.instance.GetComponent<PlayerController>();
    }

    public void setBack()
    {
        LazerReady.SetActive(false);
        LazerGFX.SetActive(false);
        control.resetLazer();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(player == null)
            {
                player = PlayerManager.instance.Player.GetComponent<PlayerController>();
            }
            player.TakeDamage(10);
        }
    }

    public void PlayeLazerSound()
    {
        AudioManager.instance.PlaySFX(lazerSound);
    }
}
