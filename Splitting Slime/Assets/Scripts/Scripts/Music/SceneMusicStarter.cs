using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicStarter : MonoBehaviour
{

    [SerializeField]
    private AudioClip sceneMusic;

    void Start()
    {
        SceneManager.sceneUnloaded += StopMusic;
        if (sceneMusic != null)
        {
            ReplayMusic();
        }
    }

    private void ReplayMusic()
    {
        AudioManager.instance.PlayMusic(sceneMusic);
        Invoke("ReplayMusic", sceneMusic.length);
    }

    private void StopMusic<Scene>(Scene scene)
    {
        AudioManager.instance.StopMusic();
    }
}
