using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
//May get rid of
public class TankEnemy : Enemy
{
    private int blockingHits = 3;
    private bool blockRecentlyBroke = false;
    private bool guarding = false;
    private Transform GuardedEnemy;

    public new void Awake()
    {
        base.Awake();
    }

    public new void Start()
    {
        base.Start();
        //StartCoroutine(gainBlock());
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if (!stunned && !dead)
        {
            Tank();
        }
    }

    private IEnumerator gainBlock()
    {
        while (true)
        {
            if (blockingHits < 3 && !blockRecentlyBroke)
            {
                blockingHits++;
            }
            else if (blockRecentlyBroke)
            {
                yield return new WaitForSeconds(8f);
                blockRecentlyBroke = false;
            }
            yield return new WaitForSeconds(5f);
        }
    }

    private void Tank()
    {
        if (player.attacking)
        {
            float random = returnRandomNumber(); //TODO: Make a common functions singleton
            if(random < myStats.blockChance)
            {
                anim.SetInteger(stateHash, 10);
            }

        }
        //TODO: Make if statement more performant, prehaps handled by enemy Headquarters to do one call
        if (GuardedEnemy == null)
        {
            GuardedEnemy = EnemyHeadquarters.instance.getRangedEnemy();
        }
        if (anim.GetInteger(stateHash) != 10) {
            if (currentTrigger == TriggerStates.MustMelee)
            {
                MeleeMove();
                guarding = false;
                tankAttackControl();
            }
            else
            {
                if (GuardedEnemy != null && agent.enabled)
                {
                    agent.SetDestination(new Vector3(GuardedEnemy.position.x + (2 * GuardedEnemy.localScale.x), transform.position.y, GuardedEnemy.position.z));
                    guarding = true;
                    tankControl();
                }
                else
                {
                    MeleeMove();
                    guarding = false;
                    tankAttackControl();
                }
            }
        }
        setAttackStatus(Target);
    }

    public void tankControl()
    {
        if (anim.GetInteger(stateHash) != 3 && guarding)
        {
            if (blocking && agent.remainingDistance > .1)
            {
                anim.SetInteger(stateHash, 7);
                blocking = true;
            }
            else
            {
                anim.SetInteger(stateHash, 6);
                blocking = true;
            }
        }
    }

    public void tankAttackControl()
    {
        if (canAttack && !recentlyAttacked)
        {
            if (currentTrigger == TriggerStates.MustMelee)
            {
                if ((anim.GetInteger(stateHash) != 3) && (anim.GetInteger(stateHash) != 2))
                {
                    recentlyAttacked = true;
                    agent.enabled = false;
                    anim.SetInteger(stateHash, 0);
                    anim.SetInteger(stateHash, 3);
                }
            }
        }
        resetRecentlyAttacked();
    }

    //public override void applyDamage(float damage)
    //{
    //    if (!dead)
    //    {
    //        myStats.Health -= damage;
    //        if (!blocking)
    //        {
    //            stunHandler();
    //        }
    //        else
    //        {
    //            blockingHits--;
    //            if (blockingHits == 0)
    //            {
    //                blocking = false;
    //                blockRecentlyBroke = true;
    //            }
    //        }
    //        if (myStats.Health < 0 && !dead)
    //        {
    //            dead = true;
    //            die();
    //        }
    //    }
    //}

    private float returnRandomNumber()
    {
        return Random.Range(0, 100);
    }
}
