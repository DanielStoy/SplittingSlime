using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats : Stats
{
    public Weapon carriedWeapon;
    int coinAmount;
    public int xpWorth;
    public float blockChance = 0;
    public bool canDropWeapon;

    
    public EnemyStats()
    {
        Health = 5;
        Speed = 2;
        xpWorth = 10;
    }

    public EnemyStats(int Level)
    {
        Health = 15 + (Level * 10);
        Speed = 3 + Level / 10;
        Strength = 10 + (10 * Level);
        xpWorth = 10 + (10 * Level);
    }

    public EnemyStats(int Level, EnemyType type)
    {
        if(type == EnemyType.Melee)
        {
            Health = 15 + (Level * 10);
            Speed = 3 + Level / 10;
            Strength = 10 + (10 * Level);
            xpWorth = 10 + (10 * Level);
        }
        else
        {
            Health = 15 + (Level * 10);
            Speed = 2 + Level / 10;
            Strength = 10 + (10 * Level);
            xpWorth = 10 + (10 * Level);
        }
    }

    public string DroppableObject()
    {
        int random = Random.Range(0, 100);
        if (canDropWeapon)
        {
            if (random < 80)
            {
                return "Coin";
            }
            else if (random < 85)
            {
                return "Apple";
            }
            else if (random < 90)
            {
                return "Peach";
            }
            else if (random < 92)
            {
                return "Ban";
            }
            else if (random < 95)
            {
                return "Sandwich";
            }
            else if (random < 98)
            {
                return "Weapon";
            }
            else
            {
                return "SCoin";
            }
        }
        else
        {
            if (random < 80)
            {
                return "Coin";
            }
            else if (random < 85)
            {
                return "Apple";
            }
            else if (random < 90)
            {
                return "Peach";
            }
            else if (random < 92)
            {
                return "Ban";
            }
            else if (random < 98)
            {
                return "Sandwich";
            }
            else
            {
                return "SCoin";
            }
        }
    }
}
