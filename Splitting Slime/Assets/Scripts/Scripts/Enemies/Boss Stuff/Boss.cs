using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    public float triggerZero;
    public float triggerOne;
    public float triggerTwo;
    public float bossOffset = 0;
    public WaitForSeconds wait = new WaitForSeconds(1.0f);
    public int currentTrigger = 0;
    public Animator anim;
    public float health = 100;
    public bool dead = false;
    public BossHealthBar healthBar;
    public void Start()
    {
        //player = PlayerManager.instance.Player.transform;
        StartCoroutine(getTrigger());
    }

    //Getting how far away the player is from the boss, useful for triggering attacks
    public IEnumerator getTrigger()
    {
        while (true)
        {
            float TargetDistance = Mathf.Abs(player.position.x - (transform.position.x - bossOffset));
            if (TargetDistance <= triggerTwo)
            {
                currentTrigger = 2;
            }
            else if (TargetDistance <= triggerOne)
            {
                currentTrigger = 1;
            }
            else
            {
                currentTrigger = 0;
            }
            yield return wait;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
    }
}
