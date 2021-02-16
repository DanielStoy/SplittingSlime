using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    GameObject GFX;
    public string explosionString;
    Transform explosionSpot;
    PlayerController player;
    Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        GFX = transform.GetChild(0).gameObject;
        explosionSpot = transform.GetChild(1);
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        playerPos = PlayerManager.instance.Player.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GFX.SetActive(false);
            ObjectPooling.instance.SpawnFromPool(explosionString, explosionSpot.position, Quaternion.identity, false);
            player.TakeDamage(10);
            float num = (transform.position.x - playerPos.position.x) / (Mathf.Abs(transform.position.x - playerPos.position.x));
            Vector3 direction;
            if (double.IsNaN(num))
            {
                direction = new Vector3(Mathf.Ceil(playerPos.localScale.x), .5f, 0);
            }
            else
            {
                direction = new Vector3((num * -1) * .3f, .5f, 0);
            }
            player.applyKnockback(20,false, direction);
        }
    }
}
