using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChoiceShower : MonoBehaviour
{
    public FakeButton button;
    public TextMeshPro text;
    public SpriteRenderer art,background,chosen;
    public List<RewardData> RD;
    public Choice choice;
    public void SetChoice(Choice c)
    {
        choice=c;
    }
    public void SetActive(bool alpha)
    {
        gameObject.SetActive(alpha);
    }
    public void UpdateData()
    {
        //text.color=new Color(1,1,1,1f);
        text.text=choice.text;
        art.sprite=choice.art;
        background.sprite=choice.background;
        RD=choice.reward;
       // button.RI.SetData(RD.r);
        //button.R.SetData(RD.)
    }
}
[Serializable]
public class Choice
{
    public string text;
    public Sprite art,background;
    public List <RewardData> reward;
    public List<Prerequisits> prerequisits;
}
[Serializable]
public class Prerequisits
{
 public ChoiceRequeriment requeriment;
 public bool not;
 public int value;
}