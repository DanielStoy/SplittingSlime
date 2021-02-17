using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimateMainMenu : MonoBehaviour
{
    [SerializeField]
    Transform sword_1, sword_2;

    [SerializeField]
    Transform[] menus;
    Transform currentMenu;
    int currentNum = 0;
    [SerializeField]
    Button[] mainMenuButtons, startButtons, optionsButtons, creditButtons;

    public List<Button[]> Buttons = new List<Button[]>();
    void Start()
    {
        transform.DOMoveY(transform.position.y - 1555, 1.5f).OnComplete(Callback_2);
        currentMenu = transform;
        Buttons.Add(mainMenuButtons);
        Buttons.Add(optionsButtons);
        Buttons.Add(startButtons);
        Buttons.Add(creditButtons);
        for(int i = 0; i < Buttons[0].Length; i++)
        {
            Buttons[0][i].interactable = false;
        }
    }

    void Callback_2()
    {
        sword_1.DOMove(new Vector3(transform.position.x - 122, transform.position.y + 1500, 0),2);
        sword_2.DOMove(new Vector3(transform.position.x + 122, transform.position.y + 1500, 0), 2).OnComplete(Callback_3);
    }

    void Callback_3()
    {
        for (int i = 0; i < Buttons[0].Length; i++)
        {
            Buttons[0][i].interactable = true;
        }
    }

    public void SlideOutCurrent(int i)
    {
        for(int c = 0; c < Buttons[currentNum].Length; c++)
        {
            Buttons[currentNum][c].interactable = false;
        }
        currentMenu.DOMoveX(currentMenu.position.x - 3000, 1.5f).OnComplete(()=> SlideOutCallback(i));
    }

    void SlideOutCallback(int i)
    {
        currentMenu.position = new Vector3(currentMenu.position.x + 6000, currentMenu.position.y + 0, 0);
        SlideInItem(i);
    }

    public void SlideInItem(int i)
    {
        currentMenu = menus[i];
        currentNum = i;
        for (int c = 0; c < Buttons[currentNum].Length; c++)
        {
            Buttons[currentNum][c].interactable = false;
        }
        menus[i].DOMoveX(transform.position.x - 3000, 1.5f).OnComplete(SlideInCallBack);
    }

    private void SlideInCallBack()
    {
        for (int c = 0; c < Buttons[currentNum].Length; c++)
        {
            Buttons[currentNum][c].interactable = true;
        }
    }

    public void NewGame()
    {
        SceneManagerScript.instance.loadScene(SceneIndex.ForestSceneOne);
    }

    //TODO: Do something here
    public void LoadGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
