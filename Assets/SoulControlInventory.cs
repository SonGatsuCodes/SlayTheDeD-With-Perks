using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class SoulControlInventory : MonoBehaviour
{
    public Manager M;
    public SpriteRenderer background, art;
    public TextMeshPro desc, pageText, pageRelicText;
    public List<SoulContainer> allSoulContainer;
    public List<SoulData> datas;
    public int page, page1;

    public void AddSoulToData()
    {
        datas = M.GetSave().souls;
    }
    public SoulData GetActualSoul()
    {
        if (index < 0)
        { index = 0; }
        //index=((page-1)*12)+i;

        return (datas.Count > 0) ? datas[index] : nulo;
    }
    public SoulData nulo;
    public void RemoveSoul()
    {
        print(GetActualSoul());
        datas.Remove(GetActualSoul());
        index = 0;
        Reset();
    }

    public bool buttonSet = false;
    public void SetAllButtons()
    {
        if (buttonSet) { }
        else
        {
            AddSoulToData();
            int j = 0;
            for (int i = 0; allSoulContainer.Count > i; i++)
            {
                j = ((page - 1) * 12) + i;                //print("i="+i+" j"+j);
                allSoulContainer[i].index = i;                //print("i="+i+" j"+j);
                if (-1 < j && j < datas.Count)
                {                    //print("i="+i+" j"+j);
                    allSoulContainer[i].art.sprite = datas[j].art;
                    allSoulContainer[i].gameObject.SetActive(true);
                }
                else { allSoulContainer[i].gameObject.SetActive(false); }
            }
        }
    }
    public GameObject show;
    public void SetVisible()
    {
        show.SetActive(!IsVisible());
    }
    public bool IsVisible()
    {
        return show.activeInHierarchy;
    }
    public void AddSoul(AdderTarget t)
    {
        print(t);
        RelicShowerA.SetActive(false);
        if (datas == null)
        { }
        else
        {
            if (datas.Count > 0)
            {
                switch (t)
                {
                    case AdderTarget.Card:
                        M.cardMode=CardMode.Soulable;
                        M.actualdeck = shower.fullDeck;
                        M.DisActive();
                        M.ShowAllCards(false);
                        break;
                    case AdderTarget.Relic:
                        RelicShowerA.SetActive(true);
                        NextSoulPageRelic(true);                //kkj SetRelicsButtons();
                        break;
                    case AdderTarget.Atk:
                        M.ShowTeam();
                        M.CC.SetAllCompanions(GetActualSoul());
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public ItemData test;
    public RelicShower prefab;
    public List<RelicShower> allrelics;
    public Vector4 offset;
    public void SetRelicsButtons()
    {
        int Max = 8, d = 4;
        Vector3 v = RelicShowerA.transform.position;
        if (allrelics.Count < Max)
        {
            for (int i = 0; i < Max; i++)
            {
                allrelics.Add(Instantiate(prefab, RelicShowerA.transform));
                allrelics[i].gameObject.SetActive(true);
                allrelics[i].transform.localScale = new Vector3(0.15f, .2f, 1);

                //big.item = item; big.SetArt(); big.showDesc(); setBigValue();
            }
        }
        int j = 0;
        for (int i = 0; i < Max; i++)
        {
            j = ((page1 - 1) * 8) + i;
            if (j < 0) { j = 0; }
            if (M.DB.saves[M.saveSlot].items.Count > j)
            {
                v.x = offset.z + ((i - ((i / d) * d)) * offset.x);
                v.y = offset.y + (((i / d)) * offset.w);
                allrelics[i].transform.position = v;
                //allrelics[i].item = ItemData.CreateInstance<ItemData>();
                //allrelics[i].item.Init(M.DB.saves[M.saveSlot].items[j]);
                allrelics[i].soul = GetActualSoul();//M.DB.saves[M.saveSlot].items[j];
                allrelics[i].gameObject.SetActive(true);

                //allrelics[i].item.activation=GetActualSoul().relicTrigger;
                //allrelics[i].item.efeito.AddRange(GetActualSoul().RelicEfeitos);
                //allrelics[i].item.value.AddRange(GetActualSoul().valueRelic);
                //allrelics[i].item.reverse.AddRange(GetActualSoul().reverseRelic);

                allrelics[i].button.classe = classes.SoulAdderRelic;
                allrelics[i].button.value = j;
                allrelics[i].item = M.DB.saves[M.saveSlot].items[j];
                allrelics[i].SetArt();
                allrelics[i].showDesc(true);
            }
            else
            {
                allrelics[i].gameObject.SetActive(false);
            }
        }
    }


    public void SubstituteRelic()
    {
        //M.RemoveCard
    }
    public void ShowData(int i)
    {
        index = ((page - 1) * 12) + i;
        desc.text = GenerateDesc();
        if (datas.Count > 0) { art.sprite = datas[index].art; } else { art.sprite = null; }
        if (RelicShowerA.activeInHierarchy) { SetRelicsButtons(); }
    }
    public string GenerateDesc()
    {
        string s = "";
        if (datas.Count > 0)
        {
            for (int i = 0; i < datas[index].CardEfeitos.Count; i++)
            { s = "add " + datas[index].CardEfeitos[i] + " " + datas[index].valueCard[i] + " to card.\n"; }
            s = s + "\n";
            //s=s+"or change card trigger to "+datas[index].CardUtilityTrigger+" (if utility card).\n\n";
            for (int i = 0; i < datas[index].RelicEfeitos.Count; i++)
            { s = s + "add " + datas[index].RelicEfeitos[i] + " " + datas[index].valueRelic[i] + " to relic.\n"; }
            //s = s + "change relic trigger to " + datas[index].relicTrigger + ".\n\n";
            s = s + "\nteach signature move to minion.\n" + datas[index].AtkName + ":\n";
            for (int i = 0; i < datas[index].SigntureMove.efeito.Count; i++)
            { s = s + datas[index].SigntureMove.efeito[i] + " " + datas[index].SigntureMove.value[i] + ".\n"; }
        }
        else { s = "no soul to show"; }
        return s;
    }
    public int index;
    public void Reset()
    {
        RelicShowerA.SetActive(false);
        page = -1;
        NextSoulPage(true);
    }
    public void NextSoulPage(bool increment)
    {
        if (increment) { page++; } else { page--; }
        if (page < 1) { page = GetMax(); }
        if (page > GetMax()) { page = 1; }
        pageText.text = "page:\n" + page + "/" + GetMax();
        SetAllButtons();
        ShowData(0);
    }
    public void NextSoulPageRelic(bool increment)
    {
        if (increment) { page1++; } else { page1--; }
        if (page1 < 1) { page1 = GetMaxRelic(); }
        if (page1 > GetMaxRelic()) { page1 = 1; }
        pageRelicText.text = "page:\n" + page1 + "/" + GetMaxRelic();
        SetRelicsButtons();
        //    SetAllButtons();
        //    ShowData(0);
    }
    public int GetMaxRelic()
    {
        return 1 + ((M.DB.saves[M.saveSlot].items.Count - 1) / 8);
    }
    public int GetMax()
    {
        return 1 + ((datas.Count - 1) / 12);
    }
    public GameObject RelicShowerA;
}
