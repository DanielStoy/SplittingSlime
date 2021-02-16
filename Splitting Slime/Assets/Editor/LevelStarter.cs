using UnityEngine;
using UnityEditor;

public class LevelStarter : EditorWindow
{
    public string prefabType;
    public string parentName;
    [MenuItem("Tools/Level Builder")]

    public static void ShowWindow()
    {
        GetWindow(typeof(LevelStarter));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Build Level"))
        {
            CreateLevel();
        }
    }

    private void CreateLevel()
    {
        createCamera();
        createCategories();
        createSpawn();
    }

    public void createCamera()
    {
        GameObject cam = new GameObject();
        cam.AddComponent<Camera>();
        cam.GetComponent<Camera>().orthographic = true;
        cam.transform.position = new Vector3(0, 7.2f, -15);
        cam.name = "Testing Cam";
    }

    public void createCategories()
    {
        GameObject _background = new GameObject("_background");
        GameObject _foreground = new GameObject("_foreground");
        GameObject _foliage = new GameObject("_foliage");
        GameObject _trees = new GameObject("_trees");
        GameObject _interactables = new GameObject("_interactables");
        GameObject _grounds = new GameObject("_grounds");
        GameObject _destructibles = new GameObject("_destructibles");
    }

    public void createSpawn()
    {
        GameObject spawn = new GameObject("Player Spawn");
        spawn.AddComponent<PlayerSpawner>();
        spawn.transform.position = Vector3.zero;
    }

}
