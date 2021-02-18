using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Stats
{
    public int coins;
    public int soulCoins;
    public int xp;
    public int xpToNextLevel;
    public int level;
    public int LevelPoints;

    public List<Weapon> unlockedWeapons = new List<Weapon>();
    public List<Hat> unlockedHats = new List<Hat>();

    public Weapon equippedWeapon;
    public Hat equippedHat;

    public List<Relic> relics = new List<Relic>(3);
    int amountOfRelics = 0;
    public Ability juiceBox;
    public Ability vitamin;

    public Ability weaponAbility;
    public Ability rangedAbility;

    public PlayerStats()
    {
        equippedWeapon = null;
        equippedHat = null;
        coins = 200;
        soulCoins = 0;
        Health = 100;
        maxHealth = 100;
        Strength = 1;
        Speed = 1.5f;
        Range = 1;
        xp = 0;
        level = 0;
        LevelPoints = 0;
        xpToNextLevel = 20;
        for(int i = 0; i < relics.Count; i++)
        {
            relics[i] = null;
        }
    }

    public void LevelUp()
    {
        level++;
        xp -= xpToNextLevel;
        if (level < 10)
        {
            xpToNextLevel = 20 * Mathf.FloorToInt(Mathf.Pow(1.8f, level));
        }
        else
        {
            xpToNextLevel = 500 + (level / 100) * 1000;
        }
        LevelPoints++;
        if (level % 10 == 0 && amountOfRelics != 3)
            amountOfRelics++;
    }

    public void Equip(Item item)
    {

        if (item is Weapon)
        {
            Weapon weap = (Weapon)item;
            Health += weap.healthAdd;
            Strength += weap.strengthAdd;
            Speed += weap.speedAdd;
            Range += weap.rangeAdd;
            addWeapon((Weapon)item);
            equippedWeapon = (Weapon)item;
        }
        else
        {
            Hat hat = (Hat)item;
            Health = Health * hat.healthAdjust;
            Strength = Strength * hat.strengthAdjust;
            Speed = Speed * hat.speedAdjust;
            Range = Range * hat.rangeAdjust;
            addHat((Hat)item);
            equippedHat = (Hat)item;
            EquipAbility(hat.ability);
        }
    }

    public void unEquip(Item item)
    {
        if (item is Weapon)
        {
            Weapon weap = (Weapon)item;
            Health -= weap.healthAdd;
            Strength -= weap.strengthAdd;
            Speed -= weap.speedAdd;
            Range -= weap.rangeAdd;
            equippedWeapon = null;
        }
        else
        {
            Hat hat = (Hat)item;
            Health = Health / hat.healthAdjust;
            Strength = Strength / hat.strengthAdjust;
            Speed = Speed / hat.speedAdjust;
            Range = Range / hat.rangeAdjust;
            unEquipAbility(equippedHat.ability);
            equippedHat = null;
        }
    }

    public void addWeapon(Weapon weapon)
    {
        bool alreadyHave = false;
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (weapon.spawnArea == unlockedWeapons[i].spawnArea)
            {
                alreadyHave = true;
            }
        }
        if (!alreadyHave)
        {
            unlockedWeapons.Add(weapon);
        }
    }

    public void addHat(Hat hat)
    {
        bool alreadyHave = false;
        for (int i = 0; i < unlockedWeapons.Count; i++)
        {
            if (hat.spawnArea == unlockedHats[i].spawnArea)
            {
                alreadyHave = true;
            }
        }
        if (!alreadyHave)
        {
            unlockedHats.Add(hat);
        }
    }

    public void EquipAbility(Ability ability)
    {
        if (ability.abilityType == AbilityType.AddToAttack)
        {
            weaponAbility = ability;
        }
        else if (ability.abilityType == AbilityType.AddToRanged)
        {
            rangedAbility = ability;
        }
    }

    public void unEquipAbility(Ability ability)
    {
        if (ability.abilityType == AbilityType.AddToAttack)
        {
            weaponAbility = null;
        }
        else if (ability.abilityType == AbilityType.AddToRanged)
        {
            rangedAbility = null;
        }
    }

    public void equipRelic(Relic relic, int index)
    {
        if (relics[index] != null)
        {
            unEquipRelic(index);
            relics[index] = relic;
        }
        else
        {
            relics[index] = relic;
        }
        if (relic.ability != null)
            EquipAbility(relic.ability);
    }
    
    public void unEquipRelic(int index)
    {
        if (relics[index].ability != null)
            unEquipAbility(relics[index].ability);
        relics[index] = null;
    }
}
