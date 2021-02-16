using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent : MonoBehaviour
{
    public PlayerController player;
    public Collider col;
    public HitEnemyBox weaponBoxScript;

    private void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        if (col == null)
        {
            col = GetComponentInChildren<Collider>();
        }
        if (weaponBoxScript == null)
        {
            weaponBoxScript = GetComponentInChildren<HitEnemyBox>();
        }
    }

    #region fastAttack
    //fast attacks
    public void beginFastAttack()
    {
        weaponBoxScript.setAdditionalMotion(0);
        player.activateAttackBox();
    }

    public void endFastAttack1()
    {
        weaponBoxScript.setAdditionalMotion(0);
        player.deactivateAttackBox();
        player.fastAttackNum = 0;
        player.weaponAnim.SetInteger(player.fastAttackHash, player.fastAttackNum);
    }

    public HitEnemyBox GetWeapBoxScript()
    {
        if(weaponBoxScript == null)
        {
            weaponBoxScript = GetComponentInChildren<HitEnemyBox>();
            return weaponBoxScript; 
        }
        else
        {
            return weaponBoxScript;
        }
    }

    public Collider GetCollider()
    {
        if(col == null)
        {
            col = GetComponentInChildren<BoxCollider>();
            return col;
        }
        else
        {
            return col;
        }
    }
    #endregion

    //#region HeavyAttacks
    //Heavy attacks
    //public void beginHeavyAttack()
    //{
    //    weaponBoxScript.setAdditionalMotion(0);
    //    player.activateAttackBox();
    //    player.attacking = true;
    //}
    //public void endHeavyAttack1()
    //{
    //    player.attacking = false;
    //    weaponBoxScript.setAdditionalMotion(0);
    //    player.deactivateAttackBox();
    //    //if (player.fastAttackNum == 1 && player.heavyAttackNum == 1)
    //    //{
    //    //    player.weaponAnim.SetTrigger("Fling");
    //    //}
    //    //if (player.heavyAttackNum == 1)
    //    //{
    //    //    player.fastAttackNum = 0;
    //    //    player.heavyAttackNum = 0;
    //    //    player.weaponAnim.SetInteger(player.heavyAttackHash, player.heavyAttackNum);
    //    //}
    //}
    //public void endHeavyAttack2()
    //{
    //    player.attacking = false;
    //    player.attackBoxScript.setAdditionalMotion(0);
    //    player.deactivateAttackBox();
    //    if (player.fastAttackNum == 1)
    //    {
    //        player.weaponAnim.SetTrigger("SlamDown");
    //    }
    //    else
    //    {
    //        player.fastAttackNum = 0;
    //    }
    //}
    //#endregion

    //#region Combos
    ////Combo Moves
    //public void BeginFling()
    //{
    //    player.deactivateAttackBox();
    //    player.attackBoxScript.setAdditionalMotion(1);
    //    player.activateAttackBox();
    //}

    //public void BeginKnockback()
    //{
    //    player.deactivateAttackBox();
    //    player.attackBoxScript.setAdditionalMotion(2);
    //    player.activateAttackBox();
    //}
    //public void EndKnockBack()
    //{
    //    endCombo();
    //}

    //public void BeginStun()
    //{
    //    player.deactivateAttackBox();
    //    player.attackBoxScript.setAdditionalMotion(3);
    //    player.activateAttackBox();
    //}
    //public void EndStun()
    //{
    //    endCombo();
    //}

    //public void BeginSlamDown()
    //{
    //    player.deactivateAttackBox();
    //    player.attackBoxScript.setAdditionalMotion(4);
    //    player.activateAttackBox();
    //}
    //public void EndSlamDown()
    //{
    //    endCombo();
    //}

    ////Universal
    //private void endCombo()
    //{
    //    player.attackBoxScript.setAdditionalMotion(0);
    //    player.deactivateAttackBox();
    //    player.fastAttackNum = 0;
    //    player.attacking = false;
    //    player.weaponAnim.SetInteger(player.fastAttackHash, player.fastAttackNum);

    //}

    //#endregion
}
