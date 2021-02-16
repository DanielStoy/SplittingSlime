using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyAdder : EditorWindow
{
    private int enemyAmount = 0;


    [MenuItem("Tools/EnemyHandler")]
    public static void ShowWindow()
    {
        EnemyAdder myWindow = (EnemyAdder)GetWindow(typeof(EnemyAdder));
        myWindow.titleContent.text = "Enemy Handler";
        myWindow.Show();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Handle Enemies"))
        {
            enemyAmount = 0;
            HandleEnemies();
        }
    }

    private void HandleEnemies()
    {
        GameObject enemySpawnerParent = GameObject.Find("_EnemySpawners");
        if (enemySpawnerParent != null)
        {
            CountEnemies(enemySpawnerParent);
            SetupObjectPooler();
        }
        else
        {
            Debug.Log("Place _EnemySpawners in scene as all enemy spawners parent");
        }
    }

    private void CountEnemies(GameObject enemySpawnerParent)
    {
        EnemySpawner[] spawns = enemySpawnerParent.GetComponentsInChildren<EnemySpawner>();

        foreach (EnemySpawner spawn in spawns)
        {
            if (spawn.GetMustSpawnsLength() > 0)
            {
                enemyAmount += spawn.GetMustSpawnsLength();
            }
            else
            {
                enemyAmount += spawn.GetEnemyAmount();
            }
        }
    }

    private void SetupObjectPooler()
    {
        GameObject objectPooler = GameObject.Find("ObjectPooler");
        if (objectPooler != null)
        {
            ObjectPooling objectPoolerScript = objectPooler.GetComponent<ObjectPooling>();
            for (int i = 0; i < objectPoolerScript.pools.Count; i++)
            {
                if (objectPoolerScript.pools[i].tag == "FireStatus" ||
                    objectPoolerScript.pools[i].tag == "IceStatus" ||
                    objectPoolerScript.pools[i].tag == "PoisonStatus")
                {
                    objectPoolerScript.pools[i].size = enemyAmount;
                }
            }
        }
        else
        {
            Debug.Log("Please Add an object pooler");
        }
    }
}
