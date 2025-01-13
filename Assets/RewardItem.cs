using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardItem : MonoBehaviour
{
    public PrizeRewardShower PRZ;
    public SpriteRenderer back, art, skipArt;
    public TextMeshPro text, ValueSkip;
    public Rewards Data;
    public FakeButton button;

    public void SetData(Rewards d)
    {
        Data = d;
        UpdateData();
    }
    public void UpdateData()
    {
        string S = "", s1 = "";
        int value = Data.value;
        switch (Data.type)
        {
            case RewardType.noReward:
                break;
            case RewardType.Money:
                S = " (" + value + ")";
                value = 0;
                break;
            case RewardType.Relic:
                break;
            case RewardType.Card:
                break;
            case RewardType.Soul:
                break;
            case RewardType.MultipleCards:
                break;
            case RewardType.Heal:
                break;
            default:
                break;
        }
        art.sprite = Data.art;
        text.text = Data.description + S;
        back.color = PRZ.M.DB.GetColor(Data.type);
        if (Data.Skipable)
        {
            if (value > 0)
            {
                s1 = "skip\n+" + value + " gold";
            }
            if (value < 0)
            {
                s1 = "skip\n-" + value + " gold";
            }
            if (value == 0)
            {
                s1 = "skip\n" + value;
            }
        }
        else
        {
            s1 = "Unskippable";
        }
            ValueSkip.text = s1;
        
    }
}
