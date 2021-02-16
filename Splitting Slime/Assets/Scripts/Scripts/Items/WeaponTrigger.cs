using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{
    public bool inTrigger = false;
    PlayerController player;
    Collider ourCollider;
    public string lookupName;
    Weapon myWeap;
    DialogueTrigger trig;
    public bool buying = false;
    public bool costSCoin = false;

    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        ourCollider = GetComponent<Collider>();
        myWeap = DroppableObjects.instance.weaponList[lookupName];
        trig = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }

    }

    private void Update()
    {
        if (inTrigger && Input.GetButtonDown("LightAttack"))
        {
            inTrigger = false;
            ourCollider.enabled = false;
            if (!buying)
            {
                player.EquipWeapon(myWeap, true, gameObject, true);
            }
            else
            {
                if (player.myStats.coins > myWeap.cost)
                {
                    if (costSCoin)
                    {
                        player.addSCoin(-myWeap.cost);
                    }
                    else
                    {
                        player.addCoin(-myWeap.cost);
                    }
                    player.EquipWeapon(myWeap);
                    buying = false;
                    trig.disableDialogueObject();
                }
                else
                {
                    Debug.Log("Not enough mons");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            inTrigger = false;
    }
}
