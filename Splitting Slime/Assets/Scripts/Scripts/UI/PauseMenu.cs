using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool isActive = false;
    PlayerController player;

    public void DeactivateOrActivate()
    {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void ClickContinue()
    {
        isActive = false;
        gameObject.SetActive(false);
        PlayerManager.instance.Player.GetComponent<PlayerController>().paused = false;
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
