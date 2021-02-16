using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class DroppableObjects : MonoBehaviour
{
    public static DroppableObjects instance;
    public List<Food> foodPrefabs;
    public Dictionary<string, WeaponList> enemyWeapons = new Dictionary<string, WeaponList>();
    public WeaponList weapons;
    public Dictionary<string, Weapon> weaponList = new Dictionary<string, Weapon>();



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        setWeaponsLists();
    }

    private void setWeaponsLists()
    {
        TextAsset file = Resources.Load("Weapons") as TextAsset;
        weapons = JsonUtility.FromJson<WeaponList>(file.text);
        foreach (var weapon in weapons.weapons)
        {
            weapon.model = (GameObject)Resources.Load("Weapons/" + weapon.name);
            weaponList.Add(weapon.name, weapon);
            for (int i = 0; i < weapon.carriedEnemies.Length; i++)
            {
                if (enemyWeapons.ContainsKey(weapon.carriedEnemies[i]))
                {
                    enemyWeapons[weapon.carriedEnemies[i]].weapons.Add(weapon);
                }
                else
                {
                    enemyWeapons.Add(weapon.carriedEnemies[i], new WeaponList());
                    enemyWeapons[weapon.carriedEnemies[i]].weapons.Add(weapon);
                }
            }

        }
    }

}
