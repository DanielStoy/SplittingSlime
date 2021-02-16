using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private PlayerController player;
    private bool exited = false;
    public int burnDamage;
    public float burnTime;
    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(lavaWalk());
        }
    }

    IEnumerator lavaWalk()
    {
        while (!exited)
        {
            player.TakeDamage(burnDamage);
            yield return new WaitForSeconds(burnTime);
        }
        yield break;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            exited = true;
        }
    }
}
