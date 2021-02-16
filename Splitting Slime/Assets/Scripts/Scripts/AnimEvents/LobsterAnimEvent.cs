using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LobsterAnimEvent : MonoBehaviour
{
    public GameObject lazers;
    public VisualEffect smoke;
    void Start()
    {
        lazers = transform.GetChild(0).gameObject;
        smoke = GetComponentInChildren<VisualEffect>();
        lazers.SetActive(false);
    }

    public void lazersOnEvent()
    {
        lazers.SetActive(true);
        smoke.Play();
    }

    public void lazersOffEvent()
    {
        lazers.SetActive(false);
        smoke.Stop();
    }
}
