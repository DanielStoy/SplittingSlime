using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitHandler : EnemyHitHandler
{
    public Boss control;

    private void Start()
    {
        control = GetComponent<Boss>();
    }

    public override void ApplyMotionEffect(int motionNum, Transform attacker)
    {
        
    }

    public override void applyDamage(float damage)
    {
        control.TakeDamage(damage);
    }
}
