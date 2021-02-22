using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusBar : MonoBehaviour
{
    public Slider HealthBar;
    public Slider EXPBar;
    public Gradient healthGradient;
    public Image fill;
    public Image squash;
    public int squashIndex;
    public List<Sprite> Squashes;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI sCoinText;

    public GameObject ab1;
    public GameObject ab2;
    [SerializeField]
    private Material abilityOne, abilityTwo;

    private float abilityOneTime = 0;
    private float abilityTwoTime = 0;

    private void Awake()
    {
        if (abilityOneTime == 0)
        {
            abilityOne.SetFloat("_Fade", 1);
        }
        if (abilityTwoTime == 0)
        {
            abilityTwo.SetFloat("_Fade", 1);
        }
    }


    public void setMaxEXP(float max)
    {
        EXPBar.maxValue = max;
    }

    public void setMaxHealth(float max)
    {
        HealthBar.maxValue = max;
    }

    public void setEXP(float val)
    {
        EXPBar.value = val;
    }

    public void setHealth(float val)
    {
        HealthBar.value = val;
        fill.color = healthGradient.Evaluate(HealthBar.normalizedValue);
        setSquash();
    }

    private void setSquash()
    {
        int newIndex = -1;
        float normalVal = HealthBar.normalizedValue;

        if(normalVal > .7)
        {
            newIndex = 2;
        }
        else if(normalVal > .3)
        {
            newIndex = 1;
        }
        else
        {
            newIndex = 0;
        }

        if (newIndex != squashIndex)
        {
            squashIndex = newIndex;
            squash.sprite = Squashes[squashIndex];
        }
    }

    public void setCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void setSCoin(int coin)
    {
        sCoinText.text = coin.ToString();
    }

    public void SetAbilityOne(float t)
    {
        float currentTime = 1 + (t / abilityOneTime) * 2;
        abilityOne.SetFloat("_Fade", currentTime);
    }

    public void SetAbilityTwo(float t)
    {
        float currentTime = 1 + (t / abilityOneTime) * 2;
        abilityTwo.SetFloat("_Fade", currentTime);
    }

    public void SetAbilityOneTotal(float totalTime)
    {
        abilityOneTime = totalTime;
        abilityOne.SetFloat("_Fade", 3);
    }

    public void SetAbilityTwoTotal(float totalTime)
    {
        abilityTwoTime = totalTime;
        abilityTwo.SetFloat("_Fade", 3);
    }
}
