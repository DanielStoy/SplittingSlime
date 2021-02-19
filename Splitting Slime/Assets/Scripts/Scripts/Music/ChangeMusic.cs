using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip music;

    //TODO: Remove the error check maybe
    public void ChangeSceneMusic()
    {
        if (music != null)
        {
            AudioManager.instance.PlayMusic(music);
            Invoke("ChangeSceneMusic", music.length);
        }
    }


}
