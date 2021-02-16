using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpitter : RangedEnemy
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    public override void throwItem(string tag, bool flipThrowableX = false)
    {
        ObjectPooling.instance.SpawnFromPool(tag, throwHolder.transform.position, Quaternion.identity, true, true, transform.localScale.x);
    }
}
