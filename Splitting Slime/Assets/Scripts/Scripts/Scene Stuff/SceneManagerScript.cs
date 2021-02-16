using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;
    private SceneIndex currentScene;
    AsyncOperation op;
    public int spawnIndex = 0;

    private void Awake()
    {
        instance = this;
        op = SceneManager.LoadSceneAsync((int)SceneIndex.ForestSceneOne, LoadSceneMode.Additive);
        currentScene = SceneIndex.ForestSceneOne;
        StartCoroutine(LoadAndSetActive());
    }

    public void loadScene(SceneIndex index)
    {
        SceneManager.UnloadSceneAsync((int)currentScene);
        op = SceneManager.LoadSceneAsync((int)index, LoadSceneMode.Additive);
        currentScene = index;
        StartCoroutine(LoadAndSetActive());
    }

    private IEnumerator LoadAndSetActive()
    {
        while (!op.isDone)
            yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)currentScene));
    }

    public int GetCurrentScene()
    {
        return (int)currentScene;
    }
}
