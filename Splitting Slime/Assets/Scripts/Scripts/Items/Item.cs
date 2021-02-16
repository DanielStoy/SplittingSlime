using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public abstract class Item
{
    public string name;

    public GameObject model;

    public int spawnArea = 0; //For weapon Room

    public int cost;


}
