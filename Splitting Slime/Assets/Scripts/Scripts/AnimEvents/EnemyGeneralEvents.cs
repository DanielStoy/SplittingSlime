using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyGeneralEvents : MonoBehaviour
{
    public Rigidbody rb;
    public string tagName;
    public returnableObject me;
    public string droppableTag;
    public string throwableTag;
    public bool flipThrowable;
    public GameObject attackBox;
    public float dropDeltaHeight;
    [SerializeField]
    private Animator anim;
    private Vector3 dropPos;
    private Enemy enem;
    private NavMeshAgent agent;

    public void Awake()
    {
        enem = transform.parent.GetComponent<Enemy>();
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    public void SetAgent(NavMeshAgent a)
    {
        agent = a;
    }

    public void Die()
    {
        enem.die();
    }

    //Ignore for now
    public void activateAttack()
    {
        attackBox.SetActive(true);
    }

    public void deactivateAttack()
    {
        attackBox.SetActive(false);
    }
    public void endAttack()
    {
        if (enem.canEnableAgent())
        {
            agent.enabled = true;
            enem.currentlyAttacking = false;
            anim.Play("Idle");
        }
    }
    public void endRanged()
    {
        if (enem.canEnableAgent())
        {
            agent.enabled = true;
            enem.currentlyAttacking = false;
            anim.Play("Idle");
        }
    }

    public void throwObject()
    {
        enem.throwItem(throwableTag, flipThrowable);
    }

    public void endStun()
    {
        enem.endStun();
    }

    public void endDamage()
    {
        enem.endDamage();
    }


}
