using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool getRigidBody = false;
        public bool setreturnable = false;
        public bool getAgent = false;
    }

    public static ObjectPooling instance;
    GameObject sceneParent;

    private void Awake()
    {
        instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<returnableObject>> poolDictionary;
    public Dictionary<string, Rigidbody> rigidbodyDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<returnableObject>>();
        CreateParent();
        Transform sceneParentTransform = sceneParent.transform;
        foreach(Pool pool in pools)
        {
            Queue<returnableObject> objectPool = new Queue<returnableObject>();
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, sceneParentTransform);
                Rigidbody rb = null;
                NavMeshAgent agent = null;
                if (pool.getRigidBody)
                {
                    rb = obj.GetComponent<Rigidbody>();
                }
                if (pool.getAgent)
                {
                    agent = obj.GetComponent<NavMeshAgent>();
                }
                returnableObject ourReturnable = new returnableObject(obj, rb, agent);
                if (pool.setreturnable)
                {
                    if (obj.CompareTag("Enemy"))
                    {
                        obj.GetComponent<Enemy>().me = ourReturnable;
                    }
                    else if (obj.CompareTag("Coin"))
                    {
                        obj.GetComponent<PickUpCoin>().me = ourReturnable;
                    }
                    else if (obj.CompareTag("Ranged"))
                    {
                        obj.GetComponent<SlimeBall>().me = ourReturnable;
                    }
                    else if (obj.CompareTag("Food"))
                    {
                        obj.GetComponent<PickUpFood>().me = ourReturnable;
                    }
                    else if (obj.CompareTag("Throwable"))
                    {
                        obj.GetComponent<ThrowableObject>().me = ourReturnable;
                    }
                    else if (obj.CompareTag("HomingMissle"))
                    {
                        obj.GetComponent<HomingMissle>().me = ourReturnable;
                    }
                }

                obj.SetActive(false);

                objectPool.Enqueue(ourReturnable);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    private void CreateParent()
    {
        sceneParent = new GameObject();
        sceneParent.name = "ObjectPoolingParent";
        SceneManager.MoveGameObjectToScene(sceneParent, SceneManager.GetSceneByBuildIndex(SceneManagerScript.instance.GetCurrentScene()));
    }

    //TODO: requeue is not actually used
    public returnableObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, bool reQueue, bool oppositeScaleX = false, float scalex = 0)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " Doesn't exist");
            return null;
        }

        returnableObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.gameObject.transform.position = position;
        objToSpawn.gameObject.transform.rotation = rotation;

        if (oppositeScaleX)
        {
            Vector3 flip = objToSpawn.gameObject.transform.localScale;
            flip.x = Mathf.Abs(flip.x) * scalex;
            objToSpawn.gameObject.transform.localScale = flip;
        }

        objToSpawn.gameObject.SetActive(true);
        return objToSpawn;

    }

    public returnableObject getRefrenceFromPool(string tag, Vector3 position, Quaternion rotation) {

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " Doesn't exist");
            return null;
        }
        if (poolDictionary[tag].Count > 0)
        {
            returnableObject objToReference = poolDictionary[tag].Dequeue();
            objToReference.gameObject.transform.position = position;
            objToReference.gameObject.transform.rotation = rotation;
            return objToReference;
        }
        else
        {
            return null;
        }
    }

    public void addToPool(string tag, returnableObject objToAdd)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " Doesn't exist");
            return;
        }
        poolDictionary[tag].Enqueue(objToAdd);
        objToAdd.gameObject.SetActive(false);
    }
}

public class returnableObject
{
    public GameObject gameObject;
    public Rigidbody rb;
    public NavMeshAgent agent;

    public returnableObject(GameObject obj, Rigidbody RB, NavMeshAgent Agent)
    {
        gameObject = obj;
        rb = RB;
        agent = Agent;
    }
}
