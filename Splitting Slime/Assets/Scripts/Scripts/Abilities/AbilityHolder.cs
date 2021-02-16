using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AbilityHolder : MonoBehaviour
{
    public static AbilityHolder instance;
    [Header("Grappling Hook")]
    public Material grapplingTexture;
    public LayerMask grapplingMask;
    public LayerMask grapplingItemMask;

    [Header("Grenade")]
    public LayerMask grenadeMask;
    public LayerMask wallMask;
    public GameObject grenadePrefab;

    [Header("Poison Cloud")]
    public GameObject poisonCloudPrefab;
    public LayerMask poisonCloudMask;

    [Header("Missle")]
    public GameObject misslePrefab;
    public LayerMask missleMask;

    [Header("IncreaseDamage")]
    public GameObject increaseDamagePrefab;

    [Header("Burp")]
    public GameObject burpPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void setGrapplingHook(ref LineRenderer lr, ref LayerMask lm, ref LayerMask lm2)
    {
        lr.material = grapplingTexture;
        lr.textureMode = LineTextureMode.Tile;
        lr.startWidth = .07f;
        lm = grapplingMask;
        lm2 = grapplingItemMask;
    }

    public void setGrenade(ref LayerMask lm, ref LayerMask wm, ref GameObject gm)
    {
        gm = grenadePrefab;
        lm = grenadeMask;
        wm = wallMask;
    }

    public void setPosionCloud(ref GameObject gm, ref LayerMask mask)
    {
        gm = poisonCloudPrefab;
        mask = poisonCloudMask;
    }

    public void SetMissle(ref GameObject gm, ref LayerMask mask)
    {
        gm = misslePrefab;
        mask = missleMask;
    }

    public void SetIncreaseDamage(ref GameObject gm)
    {
        gm = increaseDamagePrefab;
    }

    public void SetBurp(ref GameObject gm)
    {
        gm = burpPrefab;
    }
}
