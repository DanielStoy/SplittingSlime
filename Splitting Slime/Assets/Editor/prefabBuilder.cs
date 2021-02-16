using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

public class PrefabBuilder : EditorWindow
{
    public string[] options = new string[] { "Coin", "Misc", "Food", "Weapon", "Enemy" };
    public int index = 0;
    public string parentName;
    public string objName;
    [MenuItem("Tools/Prefab Builder")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabBuilder));
    }

    private void OnGUI()
    {
        index = EditorGUILayout.Popup(index, options);
        parentName = EditorGUILayout.TextField("Parent Name", parentName);
        objName = EditorGUILayout.TextField("Object Name", objName);
        if (GUILayout.Button("Prefabify"))
        {
            createPrefab();
        }
    }

    private void createPrefab()
    {
        for(int i = 0; i < Selection.gameObjects.Length; i++)
        {
            switch (index)
            {
                case 0:
                    createCoin(i);
                    break;
                case 1:
                    createMisc(i);
                    break;
                case 2:
                    createFood(i);
                    break;
                case 3:
                    createWeapon(i);
                    break;
                case 4:
                    createEnemy(i);
                    break;
                default:
                    Debug.Log("Object not in index");
                    break;
            }
        }
    }

    private void createCoin(int i)
    {
        GameObject parent = new GameObject();
        if (parentName != null)
        {
            parent.name = parentName;
        }
        Selection.gameObjects[i].AddComponent<Animator>();
        parent.AddComponent<BoxCollider>();
        parent.GetComponent<BoxCollider>().isTrigger = true;
        parent.AddComponent<PickUpCoin>();
        Selection.gameObjects[i].transform.parent = parent.transform;
        if (objName != null)
        {
            Selection.gameObjects[i].name = objName;
        }
        Selection.gameObjects[i].transform.localPosition = Vector3.zero;
    }

    private void createMisc(int i)
    {
        Selection.gameObjects[i].AddComponent<BoxCollider>();
        if (objName != null)
        {
            Selection.gameObjects[i].name = objName;
        }
        Selection.gameObjects[i].transform.localPosition = Vector3.zero;
    }

    private void createFood(int i)
    {
        GameObject parent = new GameObject();
        if (parentName != null)
        {
            parent.name = parentName;
        }
        parent.AddComponent<PickUpFood>();
        parent.AddComponent<BoxCollider>();
        parent.GetComponent<BoxCollider>().isTrigger = true;
        Selection.gameObjects[i].AddComponent<Animator>();
        Animator anim = Selection.gameObjects[i].GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load("Animators/Food_default") as RuntimeAnimatorController;
        Selection.gameObjects[i].transform.parent = parent.transform;
        if(objName != null)
        {
            Selection.gameObjects[i].name = objName;
        }
        Selection.gameObjects[i].transform.localPosition = Vector3.zero;
    }

    private void createWeapon(int i)
    {
        GameObject parent = new GameObject();
        parent.AddComponent<BoxCollider>();
        parent.GetComponent<BoxCollider>().isTrigger = true;
        parent.AddComponent<WeaponTrigger>();
        parent.AddComponent<AttackEvent>();
        parent.AddComponent<Animator>();
        if (parentName != null)
        {
            parent.name = parentName;
            parent.GetComponent<WeaponTrigger>().lookupName = parentName;
        }
        Animator anim = parent.GetComponent<Animator>();
        anim.runtimeAnimatorController = Resources.Load("Animators/DefaultAnim") as RuntimeAnimatorController;
        Selection.gameObjects[i].AddComponent<SpriteRenderer>();
        Selection.gameObjects[i].transform.parent = parent.transform;
        Selection.gameObjects[i].transform.localPosition = Vector3.zero;
        if(objName != null)
        {
            Selection.gameObjects[i].name = objName;
        }
    }

    private void createEnemy(int i)
    {
        GameObject parent = new GameObject();
        parent.AddComponent<NavMeshAgent>();
        parent.AddComponent<Rigidbody>();
        parent.AddComponent<BoxCollider>();
        parent.AddComponent<EnemyHitHandler>();
        Selection.gameObjects[i].AddComponent<EnemyGeneralEvents>();
        Selection.gameObjects[i].AddComponent<SpriteRenderer>();
        Selection.gameObjects[i].AddComponent<Animator>();
    }
}
