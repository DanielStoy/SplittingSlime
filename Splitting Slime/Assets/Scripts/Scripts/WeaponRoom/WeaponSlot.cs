using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    public bool isUnlocked = false;
    public int slotNum = 0;
    private WeaponRoom parent;

    private void Start()
    {
        parent = GetComponentInParent<WeaponRoom>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            parent.playerIn = true;
            parent.playerSlot = slotNum;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isUnlocked)
        {
            parent.playerIn = false;
            parent.playerSlot = -1;
        }
    }
}
