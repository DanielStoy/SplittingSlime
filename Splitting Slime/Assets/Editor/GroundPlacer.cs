using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundPlacer : EditorWindow
{

    public List<Object> source = new List<Object>();
    public List<float> amountOfSource = new List<float>();
    public float amountOfPrefabs = 0;

    public Vector3 minPosition;
    public Vector3 maxPosition;
    public List<Vector3> adjustment = new List<Vector3>();

    public Vector3 currentPosition;

    private List<GameObject> placedObjects = new List<GameObject>();
    [MenuItem("Tools/GroundPlacer")]
    public static void ShowWindow()
    {
        GroundPlacer myWindow = (GroundPlacer)GetWindow(typeof(GroundPlacer));
        myWindow.titleContent.text = "Ground Placer";
        myWindow.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        minPosition = EditorGUILayout.Vector3Field("Min Position", minPosition);
        maxPosition = EditorGUILayout.Vector3Field("Max Position", maxPosition);
        EditorGUILayout.Space(20);
        amountOfPrefabs = EditorGUILayout.FloatField("Amount of prefabs", amountOfPrefabs);
        for (int i = 0; i < amountOfPrefabs; i++)
        {
            source.Add(new Object());
            source[i] = EditorGUILayout.ObjectField("Prefab " + i, source[i], typeof(Object), true);
            amountOfSource.Add(0);
            amountOfSource[i] = EditorGUILayout.FloatField("Prefab " + i + " Amount", amountOfSource[i]);
            adjustment.Add(Vector3.zero);
            adjustment[i] = EditorGUILayout.Vector3Field("Adjustment of prefab " + i, adjustment[i]);
            EditorGUILayout.Space(15);
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Place adders"))
        {
            currentPosition = minPosition;
            placeAdders();
        }
        if(GUILayout.Button("Destroy Adders") && placedObjects.Count > 0)
        {
            destroyAdders();
        }
    }

    private void placeAdders()
    {
        bool zSide = true;
        bool xSide = true;
        for (int i = 0; i < amountOfPrefabs; i++)
        {
            for (int y = 0; y < amountOfSource[i]; y++)
            {
                float randX = Random.Range(minPosition.x, maxPosition.x);
                float randZ = Random.Range(minPosition.z, maxPosition.z);

                currentPosition = new Vector3(randX + adjustment[i].x, minPosition.y + adjustment[i].y, randZ + adjustment[i].z);
                GameObject obj = Instantiate((GameObject)source[i], currentPosition, Quaternion.identity);
                placedObjects.Add(obj);
            }
        }
    }

    private void destroyAdders()
    {
        for(int i = 0; i < placedObjects.Count; i++)
        {
            DestroyImmediate(placedObjects[i]);
        }
    }
}

