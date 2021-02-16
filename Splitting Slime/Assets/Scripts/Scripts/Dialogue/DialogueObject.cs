using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class DialogueObject
{
    public GameObject obj;
    public Transform dialogue;
    public List<SpriteRenderer> sprites;
    public List<Transform> transformAreas;
    public List<TextMeshPro> textMeshProObject;
    public DialogueObject( GameObject o, Transform d, List<SpriteRenderer> s, List<Transform> t, List<TextMeshPro> te)
    {
        obj = o;
        dialogue = d;
        sprites = s;
        transformAreas = t;
        textMeshProObject = te;
    }
}
