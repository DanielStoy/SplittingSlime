using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy Who picks things up
public class PickupEnemy : Enemy
{
    [Header("Pickup Enemy Stuff")]
    [SerializeField]
    private string pickupTag;
    [SerializeField]
    private LayerMask pickupItemsLayer;
    [SerializeField]
    protected float searchTime = 3.0f, searchRadius = 5.0f, pickupDistance = .5f;

    protected bool canPickup = false;
    protected bool carrying = false;
    protected bool moveTowardsObject = false;
    protected Transform pickupItem;
    protected GameObject pickupItemGO;
    public void Start()
    {
        base.Start();
        StartCoroutine(SearchForItems());
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
    }

    public IEnumerator MoveTowardsPickupObject()
    {
        while (moveTowardsObject)
        {
            if (!currentlyAttacking && agent.enabled)
            {
                if (!hover)
                {
                    if (!canAttack)
                    {
                        Vector3 destination = new Vector3(pickupItem.position.x + (pickupDistance) * returnTargetSide(pickupItem), transform.position.y, pickupItem.position.z);
                        destination.x = Mathf.Clamp(destination.x, minXBounds - 1, MaxXBounds + 1);
                        agent.SetDestination(destination);
                        anim.Play("Walk");
                    }
                    else
                    {
                        anim.Play("Idle");
                    }
                }
            }
            yield return new WaitForSeconds(5.0f);
        }
        yield break;
    }

    //Watches objects, if it disappears then we remove that item from everything,
    //If player picks up the item or if another enemy destroys it
    //Works for coins
    private IEnumerator SearchForItems()
    {
        while (true)
        {
            if (!canPickup && !carrying)
            {
                Collider[] searchItems = Physics.OverlapSphere(transform.position, searchRadius,pickupItemsLayer);
                if (searchItems.Length > 0)
                {
                    foreach(Collider col in searchItems)
                    {
                        if (col.CompareTag(pickupTag)) //TODO: Potential performance increase
                        {
                            Transform item = col.transform;
                            GameObject GO = col.gameObject;
                            int id = item.GetInstanceID();
                            if (!EnemyHeadquarters.instance.IsItemInDictionary(id) && GO.activeSelf)
                            {
                                EnemyHeadquarters.instance.AddToItemDictionary(id, item);
                                canPickup = true;
                                pickupItem = item;
                                pickupItemGO = GO;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!pickupItemGO.activeSelf) {
                    EnemyHeadquarters.instance.RemoveItem(pickupItem.GetInstanceID());
                    canPickup = false;
                    pickupItem = null;
                    pickupItemGO = null;
                }

            }
            yield return new WaitForSeconds(searchTime);
        }
    }
}
