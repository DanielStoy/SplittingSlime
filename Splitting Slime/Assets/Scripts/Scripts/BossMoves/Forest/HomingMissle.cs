using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    public returnableObject me;
    private Transform Target;
    private PlayerController targetCont;
    private Vector3 targetUpdatedPos;
    private bool breakOut = false;
    [SerializeField]
    private float updateTime = 1f, totalTime = 10.0f;
    

    private void Awake()
    {
        Target = PlayerManager.instance.Player.transform;
        targetCont = Target.GetComponent<PlayerController>();
        if(me == null)
        {
            me = new returnableObject(gameObject, null, null);
        }
    }
    private void OnEnable()
    {
        if(Target == null)
        {
            Target = PlayerManager.instance.Player.transform;
            targetCont = Target.GetComponent<PlayerController>();
        }
        breakOut = false;
        targetUpdatedPos = Target.position;
        StartCoroutine(updateTargetPosition());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetUpdatedPos, 2.0f * Time.deltaTime);
    }

    private IEnumerator updateTargetPosition()
    {
        float currentTime = 0;
        while (currentTime < totalTime && !breakOut)
        {
            targetUpdatedPos = Target.position;
            targetUpdatedPos.y += .2f;
            currentTime += updateTime;
            yield return new WaitForSeconds(updateTime);
        }
        ObjectPooling.instance.addToPool("ProjectileShot", me);
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !breakOut)
        {
            breakOut = true;
            targetCont.TakeDamage(5);
        }
    }
}
