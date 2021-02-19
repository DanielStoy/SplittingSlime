using DG.Tweening;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Animations;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Video;

public class TreeBossMoveController : Boss
{
    //Weapon swing and lazer at about halfway
    //Applefall always
    //Projectile shot only at halfway and at back
    //can also sometimes happen upfront

    private int stateHash = Animator.StringToHash("State");

    private float attackWait = 3.0f;
    private int attackPercent = 0;

    public float[] stages = new float[3];

    //Apple Drop
    private bool appleFallActive = false;
    public Vector3 AppleDropMin; //Y And Z
    public Vector3 AppleDropMax; //Y and Z
    private float appleDropTotalDistanceZ;
    private float appleDropTotalDistanceY;
    private float appleDropTime = .25f;
    [SerializeField]
    private int appleTotal = 30;
    public float[] applePercentageChance = new float[3] { 51, 31, 2 };

    //Lazer
    public bool LazerActive = false;
    [SerializeField]
    private GameObject Lazer;
    public float[] lazerPercentageChance = new float[3] { 31, 0, 10 };

    //WeaponSwing
    private bool swingActive = false;
    public Animator swordAnim;
    public float[] WeaponSwingPercentageChance = new float[3] { 10, 20, 30};

    //Projectile shot
    private bool shotActive = false;
    public float[] projectilePercentageChance = new float[3] { 40, 61, 60};

    private PlayerController playerCont;


    #region start and boss loop
    public new void Start()
    {
        player = PlayerManager.instance.Player.transform;
        playerCont = player.GetComponent<PlayerController>();
        //player = GameObject.Find("TargetBoi").transform;
        appleDropTotalDistanceZ = AppleDropMin.z + (AppleDropMax.z * (-1));
        appleDropTotalDistanceY = AppleDropMax.y + (AppleDropMin.y * (-1));
        anim = transform.GetChild(0).GetComponent<Animator>();
        healthBar = GetComponent<BossHealthBar>();
    }

    public void StartFight()
    {
        base.Start();
        StartCoroutine(bossLoop());
        playerCont.setMoveBounds(.3f, 14.72f);
    }

    public void EndFight()
    {
        playerCont.clearMoveBounds();
    }

    private IEnumerator bossLoop()
    {
        while (!dead)
        {
            attackPercent = Random.Range(0, 100);
            switch (currentTrigger)
            {
                case 0:
                    farTrigger();
                    break;
                case 1:
                    middleTrigger();
                    break;
                case 2:
                    closeTrigger();
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(attackWait);
        }
        yield break;
    }

    #endregion

    #region triggers
    private void farTrigger()
    {
        if (anim.GetInteger(stateHash) == 0) {
            if (!appleFallActive && attackPercent < applePercentageChance[0])
            {
                applePercentageChance[0] -= 10;
                appleFallActive = true;
                StartCoroutine(appleDrop());
            }
            else if(!shotActive)
            {
                applePercentageChance[0] += 10;
                shotActive = true;
                anim.SetInteger(stateHash, 3);
            }
        }
    }

    private void middleTrigger()
    {
        if(anim.GetInteger(stateHash) == 0)
        {
            if(!appleFallActive && attackPercent < applePercentageChance[1])
            {
                applePercentageChance[1] -= 10;
                appleFallActive = true;
                StartCoroutine(appleDrop());
            }
            else if (!shotActive && attackPercent < projectilePercentageChance[1])
            {
                projectilePercentageChance[1] -= 10;
                applePercentageChance[1] += 10;
                shotActive = true;
                anim.SetInteger(stateHash, 3);
            }
            else if(!LazerActive)
            {
                projectilePercentageChance[1] += 10;
                LazerActive = true;
                anim.SetInteger(stateHash, 1);
            }
        }
    }

    private void closeTrigger()
    {
        if (anim.GetInteger(stateHash) == 0)
        {
            if (!appleFallActive && attackPercent < applePercentageChance[2])
            {
                applePercentageChance[2] -= 10;
                appleFallActive = true;
                StartCoroutine(appleDrop());
            }
            else if (!shotActive && attackPercent < projectilePercentageChance[2])
            {
                projectilePercentageChance[2] -= 10;
                applePercentageChance[2] += 10;
                shotActive = true;
                anim.SetInteger(stateHash, 3);
            }
            else if (!LazerActive && attackPercent < lazerPercentageChance[2])
            {
                lazerPercentageChance[2] -= 10;
                projectilePercentageChance[2] += 10;
                LazerActive = true;
                anim.SetInteger(stateHash, 1);
            }
            else if (!swingActive)
            {
                lazerPercentageChance[2] += 10;
                swingActive = true;
                anim.SetInteger("State", 2);
                swordAnim.SetInteger("State", 1);
            }
        }
    }

    #endregion

    #region Attacks and resets

    public void resetLazer()
    {
        anim.SetInteger(stateHash, 0);
    }

    public void resetSwing()
    {
        anim.SetInteger(stateHash, 0);
        swordAnim.SetInteger("State", 0);
        swingActive = false;
    }

    public void endProjectileShot()
    {
        anim.SetInteger(stateHash, 0);
        shotActive = false;
    }

    //Add in that it drops apples on the player position randomly as well
    private IEnumerator appleDrop()
    {
        int tempTotal = appleTotal;
        bool fallOnPlayer = false;
        //Go from y = 9.41 to 5.19 and z = -4.89 to 4.783
        //Go a percentage on the z and then that same percentage on the Y to keep
        //The sprite on the screen for the apple fall. then just have the apple fall straight down
        while(appleTotal != 0)
        {
            fallOnPlayer = appleTotal % 5 == 0;
            appleTotal--;
            spawnApple(fallOnPlayer);
            yield return new WaitForSeconds(appleDropTime);
        }

        appleTotal = tempTotal;
        appleFallActive = false;
        yield break;
    }

    private void spawnApple(bool fallOnPlayer)
    {
        float zVal = fallOnPlayer ? player.position.z : Random.Range(AppleDropMin.z, AppleDropMax.z);
        float dis = AppleDropMin.z - zVal;
        float percentageDis = dis / appleDropTotalDistanceZ;
        float yVal = (percentageDis * appleDropTotalDistanceY) + AppleDropMin.y;
        float xVal = fallOnPlayer ? player.position.x : Random.Range(AppleDropMin.x, AppleDropMax.x);
        ObjectPooling.instance.SpawnFromPool("AppleDrop", new Vector3(xVal, yVal, zVal), Quaternion.identity, false);
    }

    #endregion


    public override void TakeDamage(float damage)
    {
        health -= damage;
        if(health < stages[0])
        {
            appleDropTime = .2f;
            appleTotal = 40;
        }
        else if(health < stages[1])
        {
            appleDropTime = .15f;
            appleTotal = 50;
        }
        else if(health < stages[2])
        {
            appleDropTime = .1f;
            appleTotal = 60;
        }

        healthBar.setHealth(health);
        //TODO: Check for bugs hardcore
        if(health <= 0)
        {
            dead = true;
            StopAllCoroutines();
            Lazer.SetActive(false);
            anim.SetInteger(stateHash, 4);
            healthBar.deactivate();
        }
        else
        {
            anim.Play("Damage");
        }
    }


    //TODO: REMOVE WHEN DONE TESTING
    private void testFunction()
    {
        if (!appleFallActive)
        {
            appleFallActive = true;
            StartCoroutine(appleDrop());
        }
        if (!LazerActive)
        {
            LazerActive = true;
            anim.SetInteger("State", 1);
        }
        if (!swingActive)
        {
            swingActive = true;
            anim.SetInteger("State", 2);
            swordAnim.SetInteger("State", 1);
        }
        if (!shotActive)
        {
            shotActive = true;
            anim.SetInteger("State", 3);
        }
    }
}
