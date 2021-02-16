using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogPrefab;
    public GameObject shopDialogPrefab;
    public GameObject itemDialogPrefab;
    public static DialogueManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private DialogueObject createDialogObject(GameObject prefab)
    {
        Transform dialog = Instantiate(prefab).transform;
        SceneManager.MoveGameObjectToScene(dialog.gameObject, SceneManager.GetSceneByBuildIndex(SceneManagerScript.instance.GetCurrentScene()));
        List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        List<Transform> transforms = new List<Transform>();
        List<TextMeshPro> text = new List<TextMeshPro>();
        text = dialog.GetComponentsInChildren<TextMeshPro>().ToList();
        for(int i = 0; i < dialog.childCount; i++)
        {
            Transform obj = dialog.GetChild(i);
            if(obj.GetComponent<SpriteRenderer>() != null)
            {
                sprites.Add(obj.GetComponent<SpriteRenderer>());
                transforms.Add(dialog.GetChild(i));
            }
        }
        return new DialogueObject(dialog.gameObject,dialog,sprites,transforms, text);
    }

    public DialogueObject giveDialog(DialogueType type, GameObject prefab = null)
    {
        DialogueObject d = null;
        if (prefab == null)
        {
            switch (type)
            {
                case DialogueType.Dialogue:
                    d = createDialogObject(dialogPrefab);
                    break;
                case DialogueType.ItemDescription:
                    d = createDialogObject(itemDialogPrefab);
                    break;
                case DialogueType.ItemShop:
                    d = createDialogObject(shopDialogPrefab);
                    break;

            }
        }
        else
        {
            d = createDialogObject(prefab);
        }
        return d;
    }

    public void returnDialog(DialogueObject d)
    {
    }


}
