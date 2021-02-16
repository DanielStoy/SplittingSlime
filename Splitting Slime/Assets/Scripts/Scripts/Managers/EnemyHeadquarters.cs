using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeadquarters : MonoBehaviour
{
    public static EnemyHeadquarters instance;
    private List<GameObject> RangedEnemies = new List<GameObject>();
    private List<GameObject> Enemies = new List<GameObject>();
    private Dictionary<int, List<Enemy>> EnemyScripts = new Dictionary<int, List<Enemy>>();
    public EnemySpawner spawner;
    public int spawnerInstaceId = 0;
    public bool left = false;
    public bool right = false;
    private Dictionary<int,List<Enemy>> currentlyAttackingEnemies = new Dictionary<int,List<Enemy>>();
    public int currentEnemyLevel = 1;
    private Dictionary<int, Transform> itemsToTrack = new Dictionary<int, Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Transform getRangedEnemy()
    {
        if (RangedEnemies.Count == 0)
        {
            return null;
        }
        else
        {
            Transform returnable = RangedEnemies[0].transform;
            RangedEnemies.RemoveAt(0);
            return returnable;
        }
    }

    public void AddEnemy(int id, GameObject enemy)
    {
        EnemyScripts[id].Add(enemy.GetComponent<Enemy>());
    }


    public void RemoveRangedEnemy(GameObject enemy)
    {
        if (RangedEnemies.Count == 0)
        {
            return;
        }
        else
        {
            RangedEnemies.Remove(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        Enemy enemRef = enemy.GetComponent<Enemy>();
        EnemyScripts[spawnerInstaceId].Remove(enemRef);
        if (!enemRef.hover)
        {
            currentlyAttackingEnemies[spawnerInstaceId].Remove(enemRef);
        }
        if (EnemyScripts[spawnerInstaceId].Count == 0)
        {
            currentlyAttackingEnemies.Remove(spawnerInstaceId);
            spawner.Deactivate();
            spawnerInstaceId = 0;
        }
    }

    public void HandOutSides(List<Enemy> enem, int instanceId)
    {
        currentlyAttackingEnemies.Add(instanceId, new List<Enemy>());
        bool leftAssigned = false;
        for(int i = 0; i < enem.Count; i++)
        {
            if(i < 2)
            {
                currentlyAttackingEnemies[instanceId].Add(enem[i]);
                enem[i].hover = false;
                if (!leftAssigned)
                {
                    leftAssigned = true;
                    enem[i].leftOrRight = -1;
                }
                else
                {
                    enem[i].leftOrRight = 1;
                }
            }
            else
            {
                enem[i].hover = true;
                enem[i].leftOrRight = 0;
            }
        }
    }

    public void ChangeAttacking(Enemy enemy, float targetSide)
    {
        int enemyNumber = (targetSide < 0) ? 0 : 1;
        if (currentlyAttackingEnemies[spawnerInstaceId].Count != 2)
        {
            currentlyAttackingEnemies[spawnerInstaceId][enemyNumber].hover = true;
            currentlyAttackingEnemies[spawnerInstaceId][enemyNumber].StartCoroutine(currentlyAttackingEnemies[spawnerInstaceId][0].hoverMovement());
            enemy.leftOrRight = currentlyAttackingEnemies[spawnerInstaceId][0].leftOrRight;
            enemy.hover = false;
            currentlyAttackingEnemies[spawnerInstaceId][enemyNumber].leftOrRight = 0;
            currentlyAttackingEnemies[spawnerInstaceId][enemyNumber] = enemy;
        }
    }

    public void AssignSidesToInstance()
    {
    }

    public void handleDeath(int leftOrRight)
    {
        if (spawnerInstaceId != 0)
        {
            for (int i = 0; i < EnemyScripts[spawnerInstaceId].Count; i++)
            {
                if (EnemyScripts[spawnerInstaceId][i].leftOrRight == 0)
                {
                    EnemyScripts[spawnerInstaceId][i].leftOrRight = leftOrRight;
                    EnemyScripts[spawnerInstaceId][i].hover = false;
                    currentlyAttackingEnemies[spawnerInstaceId].Add(EnemyScripts[spawnerInstaceId][i]);
                    break;
                }
            }
        }
    }

    public void setEnemiesAndScripts(List<Enemy> e, List<GameObject> eg)
    {
        EnemyScripts.Add(spawnerInstaceId, new List<Enemy>());
        EnemyScripts[spawnerInstaceId] = e;
        Enemies = eg;
    }

    public void AddToItemDictionary(int id, Transform trans)
    {
        itemsToTrack.Add(id, trans);
    }

    public bool IsItemInDictionary(int id)
    {
        return itemsToTrack.ContainsKey(id);
    }

    public Transform RetrieveItem(int id)
    {
        Transform value = null;
        if (itemsToTrack.TryGetValue(id, out value))
            return value;
        else
            return value;
    }

    public void RemoveItem(int id)
    {
        itemsToTrack.Remove(id);
    }

    public int GetCurrentInstanceId()
    {
        if(spawnerInstaceId == 0)
        {
            bool unique = false;
            int random = Random.Range(0, 1000);
            while (!unique)
            {
                if (!currentlyAttackingEnemies.ContainsKey(random))
                {
                    unique = true;
                }
                else
                {
                    random = Random.Range(0, 1000);
                }
            }
            currentlyAttackingEnemies.Add(random, new List<Enemy>());
            EnemyScripts.Add(random, new List<Enemy>());
            spawnerInstaceId = random;
            return spawnerInstaceId;
        }
        else
        {
            return spawnerInstaceId;
        }
    }

    public void AddEnemyToScripts(int id, Enemy enem)
    {
        if(currentlyAttackingEnemies[id].Count < 2)
        {
            currentlyAttackingEnemies[id].Add(enem);
            if (currentlyAttackingEnemies[id].Count == 1)
                enem.leftOrRight = 1;
            else
                enem.leftOrRight = -1;
        }
        else
        {
            enem.hover = true;
        }
        EnemyScripts[id].Add(enem);
    }

    public void TriggerAllSpawns(int id)
    {
        for(int i = 0; i < EnemyScripts[id].Count; i++)
        {
            EnemyScripts[id][i].FlipTriggered();
        }
    }

    public void TriggerAllEnemyHitEvents(int id)
    {
        for(int i = 0; i < EnemyScripts[id].Count; i++)
        {
            EnemyScripts[id][i].HitEvent();
        }
    }

    public void SlowDownAllCurrentEnemies()
    {
        if (spawnerInstaceId != 0)
        {
            Debug.Log("Slowed");
            for (int i = 0; i < EnemyScripts[spawnerInstaceId].Count; i++)
            {
                StartCoroutine(EnemyScripts[spawnerInstaceId][i].ApplySlow());
            }
        }
    }
}
