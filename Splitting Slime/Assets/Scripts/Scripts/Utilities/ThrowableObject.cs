using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    public returnableObject me;
    public string poolTag;
    public bool enemyObj;
    [SerializeField]
    private bool noAddToPool;
    public int stunTime = 0;
    public float damage = 0;
    public bool knockBack = false;
    private PlayerController player;
    private void Awake()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        if (me == null)
            me = new returnableObject(gameObject, null, null);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (enemyObj) {
            if (col.CompareTag("Player"))
            {
                if (stunTime != 0)
                    player.applyStun(stunTime);
                if (knockBack)
                    player.applyKnockback();
                player.TakeDamage(damage);
                if (!noAddToPool)
                    ObjectPooling.instance.addToPool(poolTag, me);
                else
                    gameObject.SetActive(false);
            }
        }
        else
        {
            if(col.CompareTag("Enemy")){
                Enemy enemy = col.GetComponent<Enemy>();
                enemy.applyDamage(damage);
                ObjectPooling.instance.addToPool(poolTag, me);
            }
        }
    }

}
