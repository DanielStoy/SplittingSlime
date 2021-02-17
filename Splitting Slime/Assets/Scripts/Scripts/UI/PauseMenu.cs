using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isActive = false;

    public void DeactivateOrActivate()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void ClickContinue()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    public void QuitToTitle()
    {
        ClickContinue();
        SceneManagerScript.instance.QuitToTitle();
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
