using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : Ability
{
    private PlayerController player;
    private SpriteRenderer playerRender;
    void Start()
    {
        Transform playerTrans = PlayerManager.instance.Player.transform;
        player = playerTrans.GetComponent<PlayerController>();
        playerRender = playerTrans.GetChild(0).GetComponent<SpriteRenderer>();
        abilityTime = 30;
    }

    public override void Initialize()
    {

    }

    public override void Activate()
    {
        StartCoroutine(SetInvincible());
    }


    private IEnumerator SetInvincible()
    {
        player.PlayAnimation("Invincibility");
        player.invincible = true;
        yield return new WaitForSeconds(6.0f);
        player.invincible = false;
        yield break;
    }

    public override void DestroyCheck()
    {
        player.invincible = false;
    }



    public override bool ReturnType()
    {
        return true;
    }
}
