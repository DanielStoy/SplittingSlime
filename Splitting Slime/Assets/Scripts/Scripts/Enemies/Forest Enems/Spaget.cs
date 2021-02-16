using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spaget : RangedEnemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void throwItem(string tag, bool flipThrowableX = false)
    {
        float Rand = Random.Range(transform.position.z - .5f, transform.position.z + .5f);
        returnableObject throwable = ObjectPooling.instance.SpawnFromPool(tag, throwHolder.transform.position, Quaternion.identity, true, true, transform.localScale.x);
        Transform throwableTrans = throwable.gameObject.transform;
        throwableTrans.DOMove(new Vector3(transform.position.x + (enemyRangedRange * transform.localScale.x), Target.position.y, Rand), 1).OnComplete(() => returnItemToStack(tag, throwable));
        //anim.SetInteger(stateHash, 0);
    }
}
