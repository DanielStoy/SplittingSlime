using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    private PlayerController player;
    [SerializeField]
    private bool stun, knockback;
    [SerializeField]
    private int stunTime = 0;
    private Transform parent;
    public EnemyStats stats;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        parent = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (stunTime != 0)
                player.applyStun(0);
            if (knockback)
                player.applyKnockback(20, false, parent.right * parent.localScale.x);

            player.TakeDamage(stats.Strength);
        }
    }
}
