using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance { get; private set; }
    public GameObject pauseMenu;
    public GameObject playerStatus;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
