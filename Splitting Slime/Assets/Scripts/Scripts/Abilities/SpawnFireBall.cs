using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireBall : Ability
{
    public GameObject fireball;
    private Rigidbody rb;

    public void Awake()
    {
        abilityType = AbilityType.AddToAttack;
    }

    private void Start()
    {
        if (fireball == null)
        {
            fireball = Resources.Load("Abilities/coin_0") as GameObject;
        }
    }

    public override void Initialize()
    {

    }

    public override void Activate()
    {
        int rand = Random.Range(0, 100);
        if(rand < 21)
        {
            GameObject obj = Instantiate(fireball, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), Quaternion.identity);
            rb = obj.GetComponent<Rigidbody>();
            rb.AddForce(transform.right * transform.localScale.x * 100, ForceMode.Impulse);
        }
    }

    public override void DestroyCheck()
    {
        
    }

    public override bool ReturnType()
    {
        return false;
    }

}
