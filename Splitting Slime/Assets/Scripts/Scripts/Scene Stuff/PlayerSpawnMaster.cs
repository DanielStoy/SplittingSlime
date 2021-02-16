using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnMaster : MonoBehaviour
{
    [SerializeField]
    List<PlayerSpawner> spawners;

    private void Awake()
    {
        spawners[SceneManagerScript.instance.spawnIndex].SpawnPlayer();
        SceneManagerScript.instance.spawnIndex = 0;
    }
}
