using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    public SceneIndex chosenIndex;
    public int spawnIndex = 0;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManagerScript.instance.spawnIndex = spawnIndex;
            SceneManagerScript.instance.loadScene(chosenIndex);
        }
    }
}
