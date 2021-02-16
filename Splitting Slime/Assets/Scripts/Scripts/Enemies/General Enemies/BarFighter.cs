using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;

public class BarFighter : PickupEnemy
{
    [Header("BarFighter Stuff")]
    [SerializeField]
    private GameObject redCircle;
    private Transform redCircleTrans;
    private bool throwing = false;
    private bool resetHover = false;
    private bool betweenCarry = false;
    private int mySpawnerInstanceId = 0;

    private DialogueTrigger myDialogue;

    [SerializeField]
    private float targetHeight = .5f, enemyHeight = .75f, pickupHeight = .85f, throwingAnimLength = .750f, maxXSet, minXSet;
    void Start()
    {
        myDialogue = GetComponentInChildren<DialogueTrigger>();
        MaxXBounds = maxXSet;
        minXBounds = minXSet;
        Create();
        SetSpawn();
        base.Start();
    }

    void Create()
    {
        redCircle = Instantiate(redCircle);
        redCircleTrans = redCircle.transform;
        redCircle.SetActive(false);
    }

    void SetSpawn()
    {
        mySpawnerInstanceId = EnemyHeadquarters.instance.GetCurrentInstanceId();
        EnemyHeadquarters.instance.AddEnemyToScripts(mySpawnerInstanceId, this);
        if (hover)
            StopHovering();
    }

    //TODO: Leave Item if we can't reach it in 10 seconds;
    void Update()
    {
        if (triggered)
        {
            base.Update();
            if (!throwing && !dead)
            {
                if (canPickup && !carrying && currentTrigger != TriggerStates.MustMelee)
                {
                    StopHovering();
                    if (!moveTowardsObject && agent.enabled)
                    {
                        moveTowardsObject = true;
                        StartCoroutine(MoveTowardsPickupObject());
                    }
                    CheckAndTryPickup();
                    if(!betweenCarry)
                        anim.Play("Walk");
                }
                else if (carrying)
                {
                    StopHovering();
                    moveTowardsObject = false;
                    ThrowObject();
                }
                else
                {
                    if (resetHover)
                    {
                        resetHover = false;
                        hover = true;
                    }
                    moveTowardsObject = false;
                    Melee();
                }
            }
        }
        else
        {
            anim.Play("Drink");
        }
    }

    private void CheckAndTryPickup()
    {
        if(Mathf.Abs(transform.position.x - pickupItem.position.x) - pickupDistance < .15f && Mathf.Abs(transform.position.z - pickupItem.position.z) < .15 && !betweenCarry)
        {
            betweenCarry = true;
            anim.Play("Pickup");
            pickupItem.DOJump(new Vector3(transform.position.x, transform.position.y + enemyHeight, transform.position.z), pickupHeight, 1, .616f).OnComplete(SetCarry);
        }
    }

    private void SetCarry()
    {
        carrying = true;
        betweenCarry = false;
    }

    private void ThrowObject()
    {
        throwing = true;
        anim.Play("ThrowCarry");
        Invoke("FinishThrowAnim", throwingAnimLength); // Set 1.0f to be throwing anim length
        carrying = false;
        redCircleTrans.position = new Vector3(Target.transform.position.x, Target.transform.position.y - targetHeight, Target.transform.position.z);
        redCircle.SetActive(true);
        pickupItem.DOJump(Target.position, 3, 1, 1.0f).OnComplete(FinishThrow);
        pickupItem.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void FinishThrowAnim()
    {
        throwing = false;
    }

    private void FinishThrow()
    {
        Collider[] hitColliders = Physics.OverlapBox(pickupItem.position, new Vector3(1.4f, 1.52f, .1f), Quaternion.identity);
        if(hitColliders.Length > 0)
        {
            for(int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].CompareTag("Player"))
                    player.TakeDamage(10);
            }
        }
        pickupItemGO.SetActive(false);
        redCircle.SetActive(false);
        canPickup = false;
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
        setAttackStatus(Target);
        attackControl();
    }

    private void StopHovering()
    {
        if (hover)
        {
            hover = false;
            resetHover = true;
        }
    }

    public override void HitEvent()
    {
        if (!triggered)
        {
            myDialogue.CompletelyDisable();
            triggered = true;
            EnemyHeadquarters.instance.TriggerAllEnemyHitEvents(mySpawnerInstanceId);
        }
    }
}
