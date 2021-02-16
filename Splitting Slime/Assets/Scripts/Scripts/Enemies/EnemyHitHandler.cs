using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour
{
    // Start is called before the first frame update
    Enemy EnemyController;
    [SerializeField]
    private bool callEvent = false;
    void Start()
    {
        EnemyController = GetComponent<Enemy>();
    }

    //TODO: Remove Comments
    public virtual void ApplyMotionEffect(int motionNum, Transform attacker)
    {
        switch (motionNum)
        {
            case 1:
                EnemyController.applyFling();
                break;
            case 2:
                EnemyController.applyKnockback(attacker);
                break;
            //case 3:
            //    break;
            //case 4:
            //    break;
            default:
                break;
        }
    }

    public virtual void ApplyStatusEffect(Weapon.WeaponDamageType weapDamageType)
    {
        if (!EnemyController.underStatusEffect)
        {
            EnemyController.underStatusEffect = true;
            switch (weapDamageType)
            {
                case Weapon.WeaponDamageType.Fire:
                    StartCoroutine(EnemyController.ApplyFireDamage());
                    break;
                case Weapon.WeaponDamageType.Ice:
                    StartCoroutine(EnemyController.ApplyIceDamage());
                    break;
                case Weapon.WeaponDamageType.Poison:
                    StartCoroutine(EnemyController.ApplyPoisonDamage());
                    break;
            }
        }
    }

    public virtual void applyDamage(float damage)
    {
        EnemyController.applyDamage(damage);
        if (callEvent)
            EnemyController.HitEvent();
    }

}
