using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UIElements;

public class DialogSystem : MonoBehaviour
{
    public Manager M;
    public GameObject shower;
    public TextMeshPro text, page;
    public SpriteRenderer art, background;
    public List<ChoiceShower> AllChoices;
    public FakeButton next, back;
    public DialogData DD;
    public int pageCount, last;
    public Choice nulo;
    //public ChoiceShower last;
    public void SetScene(DialogData scene)
    {
        if (scene != DD) { DD = scene; }
    }
    public void SetSourceItem(RewardItem Item)
    {
        RI = Item;
    }
    public void UpdateData()
    {
        text.text = DD.desc;
        art.sprite = DD.art;
        background.sprite = DD.background;
        //SetUpChoices(DD.choices);
        last = -1;
        pageCount = 0;
        ChangePreview(true);
        SetInteractable(true);
        M.HBC.button(buttonNames.dialogue).StartShining(true);
    }
    public void Solve(ChoiceShower c)
    {
        last = AllChoices.IndexOf(c) + ((pageCount - 1) * 6);
        c.text.color = Color.green;
        for (int i = 0; i < c.choice.reward.Count; i++)
        {
            M.PRS.CreateItem(c.choice.reward[i]);
        }
        M.SetRewards();
        M.HBC.button(buttonNames.dialogue).StartShining(false);
        M.HBC.button(buttonNames.reward).StartShining(true);
        SetInteractable(false);
        SetUpChoices(DD.choices);
        if (RI == null) { } else { M.PRS.BreakForReal(RI); }
        //print(c.choice.reward);
    }
    public RewardItem RI;

    public void SetInteractable(bool inte)
    {
        if (inte)
        {
            for (int i = 0; i < AllChoices.Count; i++)
            {
                AllChoices[i].button.SetInteractable(AvailableChoice(AllChoices[i].choice));
            }
        }
        else
        {
            for (int i = 0; i < AllChoices.Count; i++)
            {
                AllChoices[i].button.SetInteractable(inte);
            }
        }
    }
    public bool AvailableChoice(Choice c)
    {
        bool result = true;
        int max = c.prerequisits.Count;
        if (max > 0)
        {
            bool not; int value, value2; ChoiceRequeriment cr;
            for (int i = 0; i < max; i++)
            {
                not = c.prerequisits[i].not;
                value = c.prerequisits[i].value;
                cr = c.prerequisits[i].requeriment;
                result = false;
                switch (cr)
                {
                    case ChoiceRequeriment.RelicCount:
                        value2 = M.GetSave().items.Count;
                        result = value > value2;
                        result = not ? result : !result;
                        break;
                    case ChoiceRequeriment.none:
                        result = true;
                        break;
                    case ChoiceRequeriment.CardCount:
                        value2 = M.GetSave().deck.all.Count;
                        result = value > value2;
                        result = not ? result : !result;
                        break;
                    case ChoiceRequeriment.Relic:
                        result = M.GetSave().items.Contains(M.DB.AllRelics[value]);
                        result = not ? !result : result;
                        break;
                    case ChoiceRequeriment.Card:
                        result = M.GetSave().deck.all.Contains(M.DB.AllCards[value]);
                        result = not ? !result : result;
                        break;
                    case ChoiceRequeriment.CompanionCount:
                        value2 = M.GetSave().player.minions.Count;
                        result = value > value2;
                        result = not ? result : !result;
                        break;
                    case ChoiceRequeriment.Companion:
                        result = M.GetSave().player.minions.Contains(M.DB.units[value]);
                        result = not ? !result : result;
                        break;
                    case ChoiceRequeriment.SurrenderCount:
                    case ChoiceRequeriment.VictoryCount:
                    case ChoiceRequeriment.EnemyCount:
                    case ChoiceRequeriment.KillEnemy:
                    default:
                        print("not implemented yet");
                        break;
                }
                if (!result) { return result; }
            }
        }
        return result;
    }
    public void ChangePreview(bool increase)
    {
        if (increase)
        {
            pageCount++;
            if (pageCount > GetMax())
            { pageCount = 1; }
        }
        else
        {
            pageCount--;
            if (pageCount < 1) { pageCount = GetMax(); }
        }
        UpdatePage();
        SetUpChoices(DD.choices);
    }
    public int GetMax()
    {
        return (1 + (DD.choices.Count / 6));
    }
    public void UpdatePage()
    {   //page.text=page.text+'p';
        //page.text="";
        page.text = "page\n" + (pageCount.ToString()) + "/" + GetMax().ToString();
    }
    public void SetUpChoices(List<Choice> c)
    {
        int mod = (pageCount - 1), j = 0;
        for (int i = 0; i < AllChoices.Count; i++)
        {
            j = i + (6 * mod);
            if (j < c.Count) { AllChoices[i].SetChoice(c[j]); AllChoices[i].SetActive(true); }
            else { AllChoices[i].SetChoice(nulo); AllChoices[i].SetActive(false); }
            AllChoices[i].UpdateData();
            if (last < 0)
            {
                if (AvailableChoice(AllChoices[i].choice)) { AllChoices[i].text.color = new Color(1, 1, 1, 1f); }
                else { AllChoices[i].text.color = new Color(1, 0, 0, 1f); }
            }
            else
            {
                if (AvailableChoice(AllChoices[i].choice)) { AllChoices[i].text.color = new Color(1, 1, 1, .5f); }
                else { AllChoices[i].text.color = new Color(1, 0, 0, .5f); }
                if (last == j) { AllChoices[i].text.color = new Color(0, 1, 0, 1); }
            }
        }
        SetInteractable(last < 0);
    }
    public void SetUp()
    {
        shower.SetActive(!shower.activeInHierarchy);
    }
}
