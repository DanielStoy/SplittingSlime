using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicSound : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> clips = new List<AudioClip>();

    public void PlayAudio(int i)
    {
        if(i < clips.Count)
            AudioManager.instance.PlaySFX(clips[i]);
    }
}
