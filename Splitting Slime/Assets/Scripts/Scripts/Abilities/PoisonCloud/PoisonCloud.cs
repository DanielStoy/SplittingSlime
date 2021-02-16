using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PoisonCloud : Ability
{
    Transform player;
    VisualEffect cloudEffect;
    GameObject obj;
    LayerMask mask;
    void Start()
    {
        AbilityHolder.instance.setPosionCloud(ref prefab, ref mask);
        player = PlayerManager.instance.Player.transform;
        obj = Instantiate(prefab, player.transform.position, Quaternion.identity);
        cloudEffect = obj.GetComponent<VisualEffect>();
        cloudEffect.Stop();
        obj.SetActive(false);
    }

    public override void Initialize()
    {

    }

    public override void Activate()
    {
        obj.transform.position = player.position;
        startPoison();
        obj.SetActive(true);
        StartCoroutine(activatePoison());
    }

    private void startPoison()
    {
        Collider[] enems = Physics.OverlapSphere(player.position, 3, mask);
        foreach (Collider enem in enems)
        {
            enem.GetComponent<EnemyHitHandler>().ApplyStatusEffect(Weapon.WeaponDamageType.Poison);
        }
    }

    public IEnumerator activatePoison()
    {
        cloudEffect.Play();
        yield return new WaitForSeconds(3);
        cloudEffect.Stop();
        yield break;
    }

    public override void DestroyCheck()
    {
        cloudEffect.Stop();
    }

    public override bool ReturnType()
    {
        return false;
    }
}
