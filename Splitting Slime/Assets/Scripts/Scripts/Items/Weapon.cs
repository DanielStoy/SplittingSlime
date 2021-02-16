using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : Item
{
    public float strengthAdd = 0;
    public float speedAdd = 0;
    public float healthAdd = 0;
    public int rangeAdd;
    public string[] carriedEnemies;
    public int typeOfWeap;

    public enum WeaponDamageType
    {
        Normal,
        Poison,
        Ice,
        Fire,
        Static
    }

    public WeaponDamageType damageType;
    public int statusEffectChance;
    public int stunChance = 0;
    public int rareness = 0;

    public Weapon(GameObject obj, int Range, int stAdd, int spAdd, int HAdd, string Name, int SC, int spawn)
    {
        name = Name;
        healthAdd = HAdd;
        model = obj;
        rangeAdd = Range;
        strengthAdd = stAdd;
        speedAdd = spAdd;
        stunChance = SC;
        spawnArea = spawn;
    }

}

public class WeaponList
{
    public List<Weapon> weapons = new List<Weapon>();
}
