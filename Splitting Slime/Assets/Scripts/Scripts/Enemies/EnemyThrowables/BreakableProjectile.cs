using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildObj
{
    public GameObject obj { get; set; }
    public Transform tran { get; set; }
}

//Breaks apart any projectile into multiple parts. The projectiles then use DG tween to move. Use radius to control
public class BreakableProjectile : MonoBehaviour
{
    private List<ChildObj> children = new List<ChildObj>();
    private SpriteRenderer sRenderer;
    private BoxCollider col;

    [SerializeField]
    private float radius = 1, distance = 1;
    private int amount;
    [SerializeField]
    private string returnTag;
    private returnableObject me;
    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider>();
        amount = transform.childCount;
        for(int i = 0; i < amount; i++)
        {
            children.Add(new ChildObj());
            children[i].obj = transform.GetChild(i).gameObject;
            children[i].tran = transform.GetChild(i);
            children[i].obj.SetActive(false);
        }
        sRenderer.enabled = false;
        col.enabled = false;
        me = new returnableObject(gameObject, null, null);
    }

    //Move Farther
    private void OnEnable()
    {
        sRenderer.enabled = true;
        col.enabled = true;
        transform.DOMove(transform.position + (Vector3.right * transform.localScale.x * distance), 1).OnComplete(BreakApart);
    }

    //Have to save our variables like this for the callBack
    private void BreakApart()
    {
        sRenderer.enabled = false;
        col.enabled = false;
        for(int i = 0; i < amount; i++)
        {
            int num = i;
            float angle = i * Mathf.PI * 2f / amount;
            Vector3 anglePos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            ChildObj child = children[i];
            child.obj.SetActive(true);
            Transform fireBallTran = child.tran;
            fireBallTran.DOMove(fireBallTran.position + anglePos, .75f).OnComplete(() => DisableObject(child, num));
        }
    }

    private void DisableObject(ChildObj child, int i)
    {
        child.obj.SetActive(false);
        child.tran.localPosition = Vector3.zero;

        if (i == amount - 1)
            ObjectPooling.instance.addToPool(returnTag, me);
    }
}
