using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
    public enum SceneIndex
    {
        persistantScene = 0,
        ForestSceneOne = 1,
        ForestSceneTwo = 2,
        UpperForestScene = 3,
        ForestBoss = 4,
        Hub = 5,
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

