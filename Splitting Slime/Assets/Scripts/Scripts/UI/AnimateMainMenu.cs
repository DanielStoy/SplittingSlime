using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnimateMainMenu : MonoBehaviour
{
    [SerializeField]
    RectTransform sword_1, sword_2;

    [SerializeField]
    RectTransform[] menus;
    RectTransform currentMenu;
    int currentNum = 0;
    [SerializeField]
    Button[] mainMenuButtons, startButtons, optionsButtons, creditButtons;

    public List<Button[]> Buttons = new List<Button[]>();
    void Start()
    {
        menus[0].DOAnchorPosY(0, 2).OnComplete(Callback_2);
        currentMenu = menus[0];
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
        sword_1.DOAnchorPos(new Vector2(-122, -112),2);
        sword_2.DOAnchorPos(new Vector2(122, -112), 2).OnComplete(Callback_3);
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
        RectTransform menuToMove = currentMenu;
        for(int c = 0; c < Buttons[currentNum].Length; c++)
        {
            Buttons[currentNum][c].interactable = false;
        }
        currentMenu.DOAnchorPosX(-3000, 1.3f).OnComplete(()=> SlideOutCallback(menuToMove));
        SlideInItem(i);
    }

    void SlideOutCallback(RectTransform menuToMove)
    {
        menuToMove.anchoredPosition = new Vector3(3000, 0, 0);
    }

    public void SlideInItem(int i)
    {
        currentMenu = menus[i];
        currentNum = i;
        for (int c = 0; c < Buttons[currentNum].Length; c++)
        {
            Buttons[currentNum][c].interactable = false;
        }
        menus[i].DOAnchorPosX(0, 1.5f).OnComplete(SlideInCallBack);
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
