using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;
    private DialogueObject dialogueObject;

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private bool alreadySetup = false;

    private bool triggered = false, locked = false;

    private LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Player");
        if (prefab == null)
            dialogueObject = DialogueManager.instance.giveDialog(dialogue.type);
        else
            dialogueObject = DialogueManager.instance.giveDialog(dialogue.type, prefab);

        if (dialogue.dialogObjScale == Vector3.zero)
            dialogue.dialogObjScale = dialogueObject.dialogue.localScale;
        prepareDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered && !locked)
        {
            triggered = true;
            dialogueObject.obj.SetActive(true);
            dialogueObject.dialogue.DOScale(dialogue.dialogObjScale, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && triggered && !locked)
        {
            triggered = false;
            dialogueObject.dialogue.DOScale(Vector3.zero, 1).OnComplete(DisableTween);
        }
    }

    private void DisableTween()
    {
        if(!triggered)
            dialogueObject.obj.SetActive(false);
    }

    private void prepareDialogue()
    {
        if (!alreadySetup)
        {
            switch (dialogue.type)
            {
                case DialogueType.Dialogue:
                    dialogueObjectCase();
                    break;
                case DialogueType.ItemShop:
                    itemCase();
                    itemShopCase();
                    break;
                case DialogueType.ItemDescription:
                    itemCase();
                    break;
            }
        }
        else
        {
            dialogueObject.dialogue.position = new Vector3(transform.position.x, dialogue.PosToAddToDialogue.y, transform.position.z);
            dialogueObject.obj.SetActive(false);
        }
        dialogueObject.dialogue.localScale = Vector3.zero;
    }

    private void dialogueObjectCase()
    {
        dialogueObject.textMeshProObject[0].text = dialogue.sentances;
        dialogueObject.dialogue.position = new Vector3(transform.position.x + dialogue.PosToAddToDialogue.x, dialogue.PosToAddToDialogue.y, transform.position.z + dialogue.PosToAddToDialogue.z);
        for (int i = 0; i < dialogueObject.sprites.Count; i++)
        {
            if (i < dialogue.sprites.Count)
            {
                dialogueObject.sprites[i].sprite = dialogue.sprites[i];
                if(i < dialogue.spriteScale.Count)
                {
                    dialogueObject.transformAreas[i].localScale = dialogue.spriteScale[i];
                }
            }
        }
        dialogueObject.obj.SetActive(false);
    }

    private void itemShopCase()
    {
        Weapon myWeap = DroppableObjects.instance.weaponList[dialogue.name];
        dialogueObject.textMeshProObject[4].text = "Cost: " + myWeap.cost.ToString();
    }

    //TODO: May need refactoring
    private void itemCase()
    {
        Weapon myWeap = DroppableObjects.instance.weaponList[dialogue.name];
        dialogueObject.textMeshProObject[0].text = "Strength: " + myWeap.strengthAdd.ToString();
        dialogueObject.textMeshProObject[1].text = "Range: " + myWeap.rangeAdd.ToString();
        dialogueObject.textMeshProObject[2].text = "Health: " + myWeap.healthAdd.ToString();
        dialogueObject.textMeshProObject[3].text = "Speed: " + myWeap.speedAdd.ToString();
        dialogueObject.dialogue.position = new Vector3(transform.position.x, dialogue.PosToAddToDialogue.y + transform.position.y, transform.position.z);
        for (int i = 0; i < dialogueObject.sprites.Count; i++)
        {
            if (i < dialogue.sprites.Count)
                dialogueObject.sprites[i].sprite = dialogue.sprites[i];
        }
        dialogueObject.obj.SetActive(false);
    }

    public void rePositionDialogue()
    {
        dialogueObject.dialogue.position = new Vector3(transform.position.x, dialogue.PosToAddToDialogue.y, transform.position.z);
    }

    public void disableDialogueObject()
    {
        if (triggered)
        {
            dialogueObject.obj.SetActive(false);
        }
    }

    public void forceCheck()
    {
        triggered = false;
        Collider[] co = Physics.OverlapSphere(transform.position, transform.GetComponent<SphereCollider>().radius, mask);
        if(co.Length > 0)
        {
            triggered = true;
            dialogueObject.obj.SetActive(true);
        }
    }

    public void changeToItemDialogue()
    {
        dialogueObject.sprites[0].enabled = false;
        dialogueObject.textMeshProObject[4].enabled = false;
        dialogue.type = DialogueType.ItemDescription;
    }

    public void CompletelyDisable()
    {
        triggered = true;
        locked = true;
        dialogueObject.dialogue.DOScale(Vector3.zero, 1).OnComplete(TurnOffDialogue);
    }

    public void TurnOffDialogue()
    {
        dialogueObject.obj.SetActive(false);
    }
}
