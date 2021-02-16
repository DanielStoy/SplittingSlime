using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<string> lookupStrings = new List<string>();
    private void Start()
    {
        GameObject modify;
        for(int i = 0; i< transform.childCount; i++)
        {
            if(lookupStrings.Count > i)
            {
                modify = Instantiate(DroppableObjects.instance.weaponList[lookupStrings[i]].model, transform.GetChild(i).position, Quaternion.identity);
                modify.GetComponent<WeaponTrigger>().buying = true;
            }
        }
    }

}
