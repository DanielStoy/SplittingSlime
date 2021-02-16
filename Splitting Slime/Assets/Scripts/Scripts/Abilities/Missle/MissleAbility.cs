using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleAbility : Ability
{

    private GameObject misslePrefab;
    private GameObject myMissle;
    private Missle missleControl;

    private LayerMask missleMask;
    private Transform playerTrans;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        AbilityHolder.instance.SetMissle(ref misslePrefab, ref missleMask);
        if (myMissle == null)
        {
            myMissle = Instantiate(misslePrefab);
            myMissle.SetActive(false);
            missleControl = myMissle.GetComponent<Missle>();
        }
        playerTrans = PlayerManager.instance.Player.transform;
    }

    public override void Activate()
    {
        Collider[] enems = Physics.OverlapSphere(playerTrans.position, 6, missleMask);
        if (enems.Length > 0)
        {
            Transform enemy = enems[0].transform;
            myMissle.transform.position = playerTrans.position;
            myMissle.SetActive(true);
            missleControl.ShootMissle(enemy);
        }
    }

    public override void DestroyCheck()
    {
        myMissle.SetActive(false);
    }

    public override bool ReturnType()
    {
        return false;
    }

}
