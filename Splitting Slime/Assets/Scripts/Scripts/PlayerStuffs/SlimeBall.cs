using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBall : MonoBehaviour
{
    [SerializeField]
    float totalSlimeTime = 2;
    float slimeTime = 0;
    public returnableObject me = null;
    public string myTag = "ranged";
    PlayerStats stats;
    Collider myCol;
    [SerializeField]
    private LayerMask mask;
    bool isGrounded = false;
    SpriteRenderer ballSprite;
    SpriteRenderer splatSprite;

    private void Awake()
    {
        if (me == null)
            me = new returnableObject(gameObject, GetComponent<Rigidbody>(), null);
        stats = PlayerManager.instance.Player.GetComponent<PlayerController>().myStats;
        myCol = GetComponent<Collider>();
        ballSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        splatSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, myCol.bounds.extents.y + .01f, mask, QueryTriggerInteraction.Ignore);
    }

    private void OnEnable()
    {
        ballSprite.enabled = true;
        splatSprite.enabled = false;
        myCol.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGrounded)
        {
            isGrounded = IsGrounded();
            if(isGrounded == true)
            {
                ballSprite.enabled = false;
                splatSprite.enabled = true;
                me.rb.velocity = Vector3.zero;
                myCol.isTrigger = false;
            }
        }
        if(slimeTime > totalSlimeTime)
        {
            slimeTime = 0;
            ObjectPooling.instance.addToPool(myTag, me);
        }
        slimeTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHitHandler>().applyDamage(stats.Range);
            slimeTime = 0;
            ObjectPooling.instance.addToPool(myTag, me);
        }
    }
}
