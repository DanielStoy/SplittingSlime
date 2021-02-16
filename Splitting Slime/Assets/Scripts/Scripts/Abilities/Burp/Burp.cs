using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Burp : Ability
{
    GameObject createPrefab;
    Transform createPrefabTrans;
    Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        AbilityHolder.instance.SetBurp(ref prefab);
        createPrefab = Instantiate(prefab);
        createPrefabTrans = createPrefab.transform;
        createPrefab.SetActive(false);
        player = PlayerManager.instance.Player.transform;
        abilityTime = 15;
    }

    public override void Activate()
    {
        createPrefab.SetActive(true);
        createPrefabTrans.position = player.position;
        createPrefabTrans.DOScale(new Vector3(8, 8, 8), 0.6f).OnComplete(RemoveBurp);
    }

    private void RemoveBurp()
    {
        createPrefab.SetActive(false);
        createPrefabTrans.localScale = new Vector3(1, 1, 1);
    }

    public override bool ReturnType()
    {
        return false;
    }

    public override void DestroyCheck()
    {
        createPrefab.SetActive(false);
    }
}
