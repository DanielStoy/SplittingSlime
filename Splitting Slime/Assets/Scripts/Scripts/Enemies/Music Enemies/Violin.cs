using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Violin : MeleeEnemy
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
        for (int i = 3; i < 6; i++)
        {
            float angle = i * Mathf.PI * 2f / 8;
            Vector3 anglePos = new Vector3(Mathf.Cos(angle) * enemyRangedRange * transform.localScale.x * (-1), 0, Mathf.Sin(angle) * enemyRangedRange * transform.localScale.x * (-1));
            returnableObject throwable = ObjectPooling.instance.SpawnFromPool(tag, throwHolder.transform.position, Quaternion.identity, true, true, transform.localScale.x);
            Transform throwableTrans = throwable.gameObject.transform;
            throwableTrans.DOMove(throwHolder.transform.position + anglePos, 1).OnComplete(() => returnItemToStack(tag, throwable));
        }
    }
}
