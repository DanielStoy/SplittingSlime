using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
    public enum SceneIndex
    {
        persistantScene = 0,
        mainMenu = 1,
        ForestSceneOne = 2,
        ForestSceneTwo = 3,
        UpperForestScene = 4,
        ForestBoss = 5,
        Hub = 6,
    }

public enum EnemyType
{
    Melee,
    Range,
    Tank,
}

public enum AbilityType
{
    AddEffect,
    Buff,
    AddAttack,
    AddToAttack,
    AddToRanged,
    Summons,
    justStats,
}

public enum weaponType 
{
    Standard,
    Heavy,
    Quick,
}

public enum TriggerStates
{
    MustMelee,
    Melee,
    Ranged,
    None,
}

public enum DialogueType
{
    ItemDescription = 0,
    ItemShop = 1,
    Dialogue = 2,
}

