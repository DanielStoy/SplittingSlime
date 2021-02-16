using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject Player;
    public GameObject PlayerGfx;
    public GameObject PlayerPrefab;
}
