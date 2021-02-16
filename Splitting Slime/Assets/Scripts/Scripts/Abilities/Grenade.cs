using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using UnityScript.Steps;

public class Grenade : Ability
{
    returnableObject thrownPrefab;
    Transform trans;
    Transform player;
    LayerMask mask;
    LayerMask wallMask;
    Collider[] enems;

    private void Start()
    {
        player = PlayerManager.instance.Player.transform;
        AbilityHolder.instance.setGrenade(ref mask, ref wallMask, ref prefab);
        juiceBox = false;
        abilityTime = 5;
    }

    public override void Initialize()
    {

    }

    //Change to using Initialize in the future
    public override void Activate()
    {
        thrownPrefab = ObjectPooling.instance.SpawnFromPool("Grenade", player.position, Quaternion.identity, false);
        trans = thrownPrefab.gameObject.transform;
        Vector3 throwDis = trans.position;
        throwDis.x += 3 * player.localScale.x;
        throwDis.y += .2f;
        trans.DOJump(throwDis, 3, 1, 1).OnComplete(explode);
    }

    private void explode()
    {
        Collider[] enems = Physics.OverlapSphere(trans.position, 3, mask);
        Collider[] destroyableWalls = Physics.OverlapSphere(trans.position, 3, wallMask);
        foreach(Collider enem in enems)
        {
            Vector3 dir = (enem.transform.position - trans.position).normalized;
            dir.y = 0;
            Enemy enemScript = enem.GetComponent<Enemy>();
            EnemyHitHandler enemDamageHandler = enem.GetComponent<EnemyHitHandler>();
            enemScript.applyForce(2, dir, .3f, true);
            enemDamageHandler.ApplyStatusEffect(Weapon.WeaponDamageType.Poison);
        }
        foreach(Collider wall in destroyableWalls)
        {
            wall.GetComponent<DestroyableWall>().takeDamage();
        }
        ObjectPooling.instance.addToPool("Grenade", thrownPrefab);
    }

    public override void DestroyCheck()
    {
        if(thrownPrefab != null)
        {
            ObjectPooling.instance.addToPool("Grenade", thrownPrefab);
        }
    }

    public override bool ReturnType()
    {
        return false;
    }
}
