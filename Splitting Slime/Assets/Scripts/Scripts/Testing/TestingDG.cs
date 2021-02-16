using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestingDG : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Sequence mySequence = DOTween.Sequence();
        // Add a movement tween at the beginning
        mySequence.Append(transform.DOMove(transform.position + new Vector3(0, 2, 0), 2));
        // Add a rotation tween as soon as the previous one is finished
        mySequence.Insert(0,transform.DOScale(new Vector3(.3f, .3f, .3f), 2));

        mySequence.OnComplete(destroyMe);
    }

    private void destroyMe()
    {
        Debug.Log("Destroyed");
    }
}
