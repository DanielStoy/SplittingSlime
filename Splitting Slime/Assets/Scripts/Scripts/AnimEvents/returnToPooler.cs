using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class returnToPooler : MonoBehaviour
{
    public string myTag;
    public bool getAgent = false;
    public bool getRigidBody = false;
    public returnableObject me;

    private void Start()
    {
        Rigidbody rb = null;
        NavMeshAgent agent = null;
        if (getAgent)
            agent = GetComponent<NavMeshAgent>();
        if (getRigidBody)
            rb = GetComponent<Rigidbody>();

        me = new returnableObject(gameObject, rb, agent);
    }

    public void returnToPool()
    {
        ObjectPooling.instance.addToPool(myTag, me);
    }
}
