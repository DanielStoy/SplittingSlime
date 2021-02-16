using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class RangedEnemy : Enemy
{
    public new void Awake()
    {
        base.Awake();
    }

    public new void Start()
    {
        level = EnemyHeadquarters.instance.currentEnemyLevel;
        myStats = new EnemyStats(level, EnemyType.Range);
        base.Start();
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if (!stunned && !dead)
        {
            Range();
        }
    }

    private void Range()
    {
        if (!hover)
        {
            if(currentTrigger == TriggerStates.None && !currentlyAttacking)
            {
                agent.SetDestination(Target.position);
            }
            else if (currentTrigger == TriggerStates.MustMelee && !currentlyAttacking)
            {
                runAway();
                canAttack = false;
                resetRecentlyAttacked();
            }
            else if(!currentlyAttacking)
            {
                if (runningAway)
                {
                    runningAway = false;
                    agent.ResetPath();
                }
                RangedMove();
                setAttackStatus(Target);
                rangedAttackControl();
            }
        }
        else
        {
            setAttackStatus(Target);
            rangedAttackControl();
            if (!hoverCoreutineOn)
            {
                StartCoroutine(hoverMovement());
            }
        }
    }

    public void rangedAttackControl()
    {
        if (canAttack && !recentlyAttacked && !currentlyAttacking)
        {
            currentlyAttacking = true;
            recentlyAttacked = true;
            agent.enabled = false;
            anim.Play("Throw");
        }
        resetRecentlyAttacked();
    }
}
