using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnWall : TriggerEvent
{

    [SerializeField]
    GameObject wall;
    public override void Trigger()
    {
        wall.SetActive(true);
    }
}
