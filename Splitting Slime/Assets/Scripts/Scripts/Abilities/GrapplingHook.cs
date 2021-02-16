using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrapplingHook : Ability
{
    Transform playerTrans;
    GameObject player;
    PlayerController playerControl;
    Transform enemy;
    Enemy enemyScript;
    LineRenderer lr;
    public float lineTime = 1;
    LayerMask mask;
    LayerMask grappleMask;
    private void Start()
    {
        playerTrans = PlayerManager.instance.Player.transform;
        player = PlayerManager.instance.Player;
        playerControl = player.GetComponent<PlayerController>();
        lr = player.AddComponent<LineRenderer>();
        lr.enabled = false;
        AbilityHolder.instance.setGrapplingHook(ref lr, ref mask, ref grappleMask);
        juiceBox = true;
        abilityTime = 5;
    }

    public override void Initialize()
    {

    }

    public override void Activate()
    {
        getCollider();
        if (enemy != null && enemyScript != null)
        {
            lr.enabled = true;
            attachLine(enemy);
            enemyScript.applyForce(6, getDirection(), lineTime, true);
            StartCoroutine(controlLine(lineTime, enemy));

        }
    }

    private void getCollider()
    {
        Collider[] enems = Physics.OverlapSphere(playerTrans.position, 6, mask);
        Collider[] grappleItems = Physics.OverlapSphere(playerTrans.position, 9, grappleMask);
        if(grappleItems.Length > 0 && !playerControl.getInCombat())
        {
            lr.enabled = true;
            StartCoroutine(controlLine(1f, grappleItems[0].transform));
            grappleItems[0].GetComponent<GrappleItem>().launchPlayer();
            return;
        }

        if (enems.Length > 0)
        {
            enemy = enems[0].transform;
            enemyScript = enemy.GetComponent<Enemy>();
        }
    }

    private Vector3 getDirection()
    {
        Vector3 dir = (playerTrans.position - enemy.position).normalized;
        dir.y = 0;
        return dir;
    }

    private void attachLine(Transform transToAttachTo)
    {
        lr.SetPosition(0, playerTrans.position);
        lr.SetPosition(1, transToAttachTo.position);
    }

    private IEnumerator controlLine(float lineappearance, Transform transToAttachTo)
    {
        float timeElapsed = 0;
        while (timeElapsed < lineappearance)
        {
            timeElapsed += Time.deltaTime;
            attachLine(transToAttachTo);
            yield return new WaitForEndOfFrame();
        }
        resetSkill();
        yield break;
    }

    private void resetSkill()
    {
        lr.enabled = false;
        enemy = null;
        enemyScript = null;
    }

    public override void DestroyCheck()
    {
        resetSkill();
    }

    public override bool ReturnType()
    {
        return true;
    }

}
