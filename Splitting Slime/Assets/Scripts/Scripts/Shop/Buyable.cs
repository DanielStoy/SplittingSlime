using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    public int cost;
    public Shop myShop;
    Item item;
    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Player"))
        {

        }
    }
}
