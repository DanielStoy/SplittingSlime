using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public event Func<Transform,Transform> OnRangeCallout;
    public Transform RangeCallout(Transform enem)
    {
        if(OnRangeCallout != null)
        {
           return OnRangeCallout(enem);
        }
        return null;
    }
}
