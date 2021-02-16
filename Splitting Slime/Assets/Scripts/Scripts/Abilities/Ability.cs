using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public AbilityType abilityType { get; set; }

    public GameObject prefab;

    public bool juiceBox;
    public float abilityTime;

    public abstract void Activate();
    public abstract void Initialize();
    public abstract bool ReturnType();
    public abstract void DestroyCheck();
    public void Remove() {
        Destroy(this);
    }
}
