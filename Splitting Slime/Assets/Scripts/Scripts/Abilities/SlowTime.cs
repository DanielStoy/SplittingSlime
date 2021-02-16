using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : Ability
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        abilityTime = 30;
    }

    public override void Activate()
    {
        EnemyHeadquarters.instance.SlowDownAllCurrentEnemies();
    }

    public override bool ReturnType()
    {
        return false;
    }

    public override void DestroyCheck()
    {
        
    }
}
