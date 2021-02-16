using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemyBox : MonoBehaviour
{
    public float damage = 0;
    public bool additionalMotion = false;
    public int motionNumber;
    public Weapon weap;
    public EnemyHitHandler hitEnemy;
    public int stunAdd;

    [SerializeField]
    private string weaponName;
    [SerializeField]
    private AudioClip hitSound;

    private void Start()
    {
        weap = DroppableObjects.instance.weaponList[weaponName];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            hitEnemy = other.GetComponent<EnemyHitHandler>();
            if (additionalMotion)
            {
                Debug.Log(motionNumber);
                hitEnemy.ApplyMotionEffect(motionNumber, PlayerManager.instance.Player.transform);
                additionalMotion = false;
                motionNumber = 0;
            }
            if(weap != null && weap.damageType != Weapon.WeaponDamageType.Normal)
            {
                hitEnemy.ApplyStatusEffect(weap.damageType);
            }
            hitEnemy.applyDamage(damage);
            AudioManager.instance.PlaySFX(hitSound);
        }
        else if (other.CompareTag("AttackableItem"))
        {
            other.GetComponent<AttackableItem>().dropItem();
            AudioManager.instance.PlaySFX(hitSound);
        }
    }


    public void setDamage(float dam)
    {
        damage = dam;
    }

    public void setAdditionalMotion(int num)
    {
        if (num != 0)
        {
            additionalMotion = true;
            motionNumber = num;
        }
        else if( num == 0)
        {
            additionalMotion = false;
            motionNumber = num;
        }
    }

    public void setStun(int stun)
    {
        stunAdd = stun;
    }
}
