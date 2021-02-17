using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;
    private SceneIndex currentScene;
    [SerializeField]
    private GameObject statusCanvas, loadingScreen;
    AsyncOperation op;
    public int spawnIndex = 0;

    private void Awake()
    {
        instance = this;
        op = SceneManager.LoadSceneAsync((int)SceneIndex.mainMenu, LoadSceneMode.Additive);
        currentScene = SceneIndex.mainMenu;
        StartCoroutine(LoadAndSetActive(false));
        statusCanvas.SetActive(false);
    }

    public void loadScene(SceneIndex index)
    {
        statusCanvas.SetActive(false);
        SceneManager.UnloadSceneAsync((int)currentScene);
        op = SceneManager.LoadSceneAsync((int)index, LoadSceneMode.Additive);
        currentScene = index;
        StartCoroutine(LoadAndSetActive(true));
    }

    private IEnumerator LoadAndSetActive(bool activateCanvas)
    {
        while (!op.isDone)
            yield return null;
        statusCanvas.SetActive(activateCanvas);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex((int)currentScene));
    }

    public int GetCurrentScene()
    {
        return (int)currentScene;
    }

    public void QuitToTitle()
    {
        SceneManager.UnloadSceneAsync((int)currentScene);
        op = SceneManager.LoadSceneAsync((int)SceneIndex.mainMenu, LoadSceneMode.Additive);
        currentScene = SceneIndex.mainMenu;
        statusCanvas.SetActive(false);
        StartCoroutine(LoadAndSetActive(false));
        PlayerManager.instance.Player.GetComponent<PlayerController>().Cleanup();
    }
}
