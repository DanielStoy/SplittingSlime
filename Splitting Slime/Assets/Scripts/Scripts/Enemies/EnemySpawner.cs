using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<string> tags = new List<string>(), mustSpawns = new List<string>();
    [SerializeField]
    private int enemySpawnAmount = 1, level = 1;
    private List<Transform> spawns = new List<Transform>();
    private CameraBehavior camBehavior;
    private List<GameObject> enemies = new List<GameObject>();
    private List<Enemy> enemyScripts = new List<Enemy>();
    private int instaceId;
    [SerializeField]
    private bool triggered = false, hasCinematic = false;

    [SerializeField]
    private Vector3 arenaMinPosition, arenaMaxPosition, minPosHolder, maxPosHolder, camMinPos, camMaxPos;

    private PlayerController player;

    public PlayableDirector cinematic;

    private void Start()
    {
        Gets();
        Sets();
        PreSpawnEnemies();
    }

    private void Gets()
    {
        player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        camBehavior = CameraManager.instance.mainCam.GetComponent<CameraBehavior>();
        instaceId = gameObject.GetInstanceID();
        for(int i = 0; i < transform.childCount - 4; i++)
        {
            spawns.Add(transform.GetChild(i));
        }
    }

    //TODO: Remove Camera.main ref
    private void Sets()
    {
        arenaMinPosition = transform.Find("LeftArenaPos").position;
        arenaMaxPosition = transform.Find("RightArenaPos").position;
        if(camMinPos == Vector3.zero && camMaxPos == Vector3.zero)
        {
            float mid = (arenaMaxPosition.x + arenaMinPosition.x) / 2;
            Vector3 temp = new Vector3(mid, camBehavior.minPosition.y, camBehavior.minPosition.z);
            camMaxPos = temp;
            camMinPos = temp;
            //camMaxPos.x += Helpers.instance.SetAspect();
            //camMinPos.x -= Helpers.instance.SetAspect();
            arenaMinPosition.x += Helpers.instance.SetAspect();
            arenaMaxPosition.x -= Helpers.instance.SetAspect();
        }
    }

    private void PreSpawnEnemies()
    {
        if(mustSpawns.Count > 0)
        {
            for (int i = 0; i < mustSpawns.Count; i++)
            {
                int Rand = Random.Range(0, spawns.Count - 1);
                GameObject obj = ObjectPooling.instance.getRefrenceFromPool(mustSpawns[i], spawns[Rand].position, spawns[Rand].rotation).gameObject;
                if (obj != null)
                {
                    enemies.Add(obj);
                    enemyScripts.Add(enemies[i].GetComponent<Enemy>());
                    enemyScripts[i].SetBounds(arenaMinPosition.x, arenaMaxPosition.x);
                    generateEffects(i);
                }
            }
        }
        else
        {
            for (int i = 0; i < enemySpawnAmount; i++)
            {
                int randTag = Random.Range(0, tags.Count);
                int Rand = Random.Range(0, transform.childCount);
                GameObject obj = ObjectPooling.instance.getRefrenceFromPool(tags[randTag], spawns[Rand].position, spawns[Rand].rotation).gameObject;
                if (obj != null)
                {
                    enemies.Add(obj);
                    enemyScripts.Add(enemies[i].GetComponent<Enemy>());
                    enemyScripts[i].SetBounds(arenaMinPosition.x, arenaMaxPosition.x);
                    generateEffects(i);
                }
            }
        }
        EnemyHeadquarters.instance.HandOutSides(enemyScripts, instaceId);
    }

    private void generateEffects(int i)
    {
        Transform fire = ObjectPooling.instance.getRefrenceFromPool("FireStatus", Vector3.zero, Quaternion.identity).gameObject.transform;
        VisualEffect fireE = fire.GetComponent<VisualEffect>();
        enemyScripts[i].setStatusParticles(fireE, fire, 0);
        Transform ice = ObjectPooling.instance.getRefrenceFromPool("IceStatus", Vector3.zero, Quaternion.identity).gameObject.transform;
        VisualEffect iceE = ice.GetComponent<VisualEffect>();
        enemyScripts[i].setStatusParticles(iceE, ice, 1);
        Transform poison = ObjectPooling.instance.getRefrenceFromPool("PoisonStatus", Vector3.zero, Quaternion.identity).gameObject.transform;
        VisualEffect poisonE = poison.GetComponent<VisualEffect>();
        enemyScripts[i].setStatusParticles(poisonE, poison, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            if (!triggered)
            {
                triggered = true;
                if (!hasCinematic)
                {
                    trigger();
                }
                else
                {
                    cinematic.Play();
                }
            }
        }
    }

    public void trigger()
    {
        player.setMoveBounds(arenaMinPosition.x, arenaMaxPosition.x);
        EnemyHeadquarters.instance.spawner = this;
        EnemyHeadquarters.instance.currentEnemyLevel = level;
        EnemyHeadquarters.instance.spawnerInstaceId = instaceId;
        minPosHolder = camBehavior.minPosition;
        maxPosHolder = camBehavior.maxPosition;
        //TODO: Change this later, for now it fixes bugs that happen when a trigger changes the min Y
        camMinPos.y = camBehavior.minPosition.y;
        camMaxPos.y = camBehavior.maxPosition.y;
        camBehavior.ControlledMove(camMinPos, 1);
        camBehavior.minPosition = camMinPos;
        camBehavior.maxPosition = camMaxPos;
        spawnEnemies();
    }

    private void spawnEnemies()
    {
        EnemyHeadquarters.instance.setEnemiesAndScripts(enemyScripts, enemies);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetActive(true);
        }
    }

    public void Deactivate()
    {
        player.clearMoveBounds();
        Vector3 tmp = new Vector3(PlayerManager.instance.Player.transform.position.x, camMinPos.y, camMinPos.z);
        camBehavior.ControlledMove(tmp, .5f);
        camBehavior.minPosition = minPosHolder;
        camBehavior.maxPosition = maxPosHolder;
        triggered = false;
        gameObject.SetActive(false);
    }

    public int GetMustSpawnsLength()
    {
        return mustSpawns.Count;
    }

    public int GetEnemyAmount()
    {
        return enemySpawnAmount;
    }

    public void EnablePlayer()
    {
        if (player == null)
        {
            player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        }
        player.canPlay = true;
    }

    public void DisablePlayer()
    {
        if (player == null)
        {
            player = PlayerManager.instance.Player.GetComponent<PlayerController>();
        }
        player.canPlay = false;
    }


}
