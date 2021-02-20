using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class Options : MonoBehaviour
{
    bool fullscreen;
    [SerializeField]
    GameObject fullscreenMid;
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    private void Start()
    {
        fullscreen = Screen.fullScreen;
        fullscreenMid.SetActive(Screen.fullScreen);
        SetupResolutions();
    }

    void SetupResolutions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currResolutionIndex = i;
            }
        }

        foreach (string t in options)
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = t });

        resolutionDropdown.value = currResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        fullscreenMid.SetActive(Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int i)
    {
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, Screen.fullScreen);
    }


}
