using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float time;


    private void Update()
    {
        time += Time.deltaTime;
        if(time > 20)
        {
            //ObjectPooling.instance.addToPool("Coin", gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //ObjectPooling.instance.addToPool("Coin", gameObject);
        }
    }
}
