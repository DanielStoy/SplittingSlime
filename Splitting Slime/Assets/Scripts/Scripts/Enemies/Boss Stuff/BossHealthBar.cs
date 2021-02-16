using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField]
    Transform healthBar;
    GameObject healthBarObj;
    Boss bossControl;
    Slider slider;

    void Start()
    {
        bossControl = GetComponent<Boss>();
        healthBarObj = healthBar.gameObject;
        healthBar.localScale = new Vector3(0, 1, 1);
        slider = healthBar.GetComponent<Slider>();
        slider.maxValue = bossControl.health;
        slider.value = bossControl.health;
    }

    public void setHealth(float health)
    {
        slider.value = health;
    }

    public void SpawnHealthBar()
    {
        healthBar.DOScale(new Vector3(1, 1, 1), 3);
    }

    public void deactivate()
    {
        healthBarObj.SetActive(false);
    }
}
