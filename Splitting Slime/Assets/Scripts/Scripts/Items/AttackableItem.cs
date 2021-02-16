using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackableItem : MonoBehaviour
{
    public int Tag;
    public string dropTag;
    public bool canDropItem = true;
    private Vector3 itemHeight;
    private bool hit = false;
    private Collider col;
    public float groundPos = 0.5f;
    public List<string> droppableTagList = new List<string>();

    public void populateList()
    {
        droppableTagList.Add("Apple");
        droppableTagList.Add("Ban");
        droppableTagList.Add("Peach");
        droppableTagList.Add("Sandwich");
        droppableTagList.Add("Coin");
    }

    public void Start()
    {
        populateList();
        if (dropTag == null)
        {
            int random = Random.Range(0, droppableTagList.Count - 1);
            dropTag = droppableTagList[random];
        }
            

        itemHeight = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 1f);
        col = GetComponent<Collider>();
    }


    public void dropItem()
    {
        if(Tag == 0)
        {
            attackTree();
        }
        else if(Tag == 1)
        {
            attackGrass();
        }
    }

    private void attackTree()
    {
        if (!hit)
        {
            hit = true;
            returnableObject item = ObjectPooling.instance.SpawnFromPool(dropTag, itemHeight, Quaternion.identity, true);
            StartCoroutine(falling(item.gameObject.transform));
        }
    }

    IEnumerator falling(Transform item)
    {
        float time = 0;
        while(time < 1)
        {
            item.position = Vector3.MoveTowards(item.position, new Vector3(item.position.x,  groundPos, item.position.z), Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    private void attackGrass()
    {
        //Set some particles as active OR USE STATE ANIMATOR
        if (canDropItem)
        {
            ObjectPooling.instance.SpawnFromPool(dropTag, transform.position, Quaternion.identity, true);
        }
        gameObject.SetActive(false);
    }
}
