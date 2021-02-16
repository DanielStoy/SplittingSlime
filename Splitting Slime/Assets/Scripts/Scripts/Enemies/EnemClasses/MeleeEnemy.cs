using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MeleeEnemy : Enemy
{
    public new void Awake()
    {
        base.Awake();
        attackBox = transform.Find("AttackBox").gameObject;
        attackBoxScript = attackBox.GetComponent<EnemyAttackBox>();
    }

    public new void Start()
    {
        level = EnemyHeadquarters.instance.currentEnemyLevel;
        myStats = new EnemyStats(level, EnemyType.Melee);
        base.Start();
        attackBox.SetActive(false);
        attackBoxScript.stats = myStats;
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if (!stunned && !dead)
        {
            Melee();
        }
    }

    private void Melee()
    {
        if (!hover)
        {
            MeleeMove();
        }
        else if (!hoverCoreutineOn)
        {
            StartCoroutine(hoverMovement());
        }
        if (currentTrigger != TriggerStates.None)
        {
            setAttackStatus(Target);
            attackControl();
        }
    }
}
