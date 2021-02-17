using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class IncreaseAttack : Ability
{
    PlayerController player;
    Transform playerTrans;
    private float increaseAttackTime = 8;
    private bool AttackIncreased = false;
    private Coroutine currentCore;
    VisualEffect effect;
    GameObject effectGM;
    Transform effectTrans;

    public override void Initialize()
    {
        abilityTime = 30;
        playerTrans = PlayerManager.instance.Player.transform;
        player = playerTrans.GetComponent<PlayerController>();
        AbilityHolder.instance.SetIncreaseDamage(ref prefab);
        effectGM = Instantiate(prefab);
        effectTrans = effectGM.transform;
        effect = effectTrans.GetComponent<VisualEffect>();
    }

    public override bool ReturnType()
    {
        return true;
    }

    public override void Activate()
    {
        currentCore = StartCoroutine(IncreaseAttackOverTime());
        player.myStats.Strength += 3;
        player.PlayAnimation("IncreaseAttack");
        effectTrans.position = playerTrans.position;
        effect.Play();
    }


    IEnumerator IncreaseAttackOverTime()
    {
        AttackIncreased = true;
        float currentTime = 0;
        while(increaseAttackTime > currentTime)
        {
            effectTrans.position = playerTrans.position;
            currentTime += Time.deltaTime;
            yield return null;
        }
        effect.Stop();
        player.myStats.Strength -= 3;
        AttackIncreased = false;
        yield break;
    }

    public override void DestroyCheck()
    {
        if (AttackIncreased)
        {
            StopCoroutine(currentCore);
            player.myStats.Strength -= 3;
        }
        Destroy(effectGM);
    }
}
