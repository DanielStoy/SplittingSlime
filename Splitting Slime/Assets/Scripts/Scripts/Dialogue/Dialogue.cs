using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{

    public string name;

    [TextArea(0, 5)]
    public string sentances;
    public List<Sprite> sprites = new List<Sprite>();
    public List<Vector3> spriteScale = new List<Vector3>();
    public Vector3 dialogObjScale = Vector3.zero;
    public DialogueType type;

    public Vector3 PosToAddToDialogue;

}
