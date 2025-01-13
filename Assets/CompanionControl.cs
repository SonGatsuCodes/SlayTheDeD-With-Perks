using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanionControl : MonoBehaviour
{
    public Manager M;
    public GameObject Tab;
    public CompanionDataShow prefab;
    public List<CompanionDataShow> ListStatusWindows;
    public List<UnitData> minions;
    public List<bool> team;
    public List<UnitData> subminions;
    public CompanionDataShow MinionShower;
    public Transform parent;
    public TextMeshPro pageText, minionText;
    public int index, page;
    public void AddSoulToAtk(int v)
    {
        print("addding Soul to atk please wait");
        UnitData subject=minions[v],improved;
        improved=UnitData.CreateInstance<UnitData>();
        improved.initUnitData(subject);
        if(TempSoul!=null)
        {improved.atks.Add(TempSoul.SigntureMove);
        //improved.minions
        }
        else{print("null soul?");}
        int inde=M.DB.saves[M.saveSlot].player.minions.IndexOf(subject);
        if(inde>-1)
        {
        M.DB.saves[M.saveSlot].player.minions[inde]=improved;
        }
        M.SCI.RemoveSoul();
    
    }
    public void SetAllCompanions(SoulData s)
    {
        minions.Clear();
        if (M.BattleStop)
        {
            minions.Add(M.player.UD);
            minions.AddRange(M.DB.saves[M.saveSlot].player.minions);
        }
        else
        {
            for (int i = 0; i < M.everybody.Count; i++) { minions.Add(M.everybody[i].UD); 
            team.Add(M.everybody[i].areaType.type == Target.PlayerTeam); }

        }
        TempSoul=s;
        page = -1;
        NextCompanionPage(true);
    }
    public bool isVisible()
    {
        return Tab.activeInHierarchy;
    }
    public void SetVisible()
    {
        Tab.SetActive(!Tab.activeInHierarchy);
    }
    public string GetTeam(UnitData ud)
    {
        int index = minions.IndexOf(ud);
        if (team.Count > index && (!M.BattleStop))
        {
            return ((team[index])?"<color=#0000ffff>ally</color>":"<color=#ff0000ff>enemy</color>");
        }
        else
        {
            return "";
        }
    }
    public void NextCompanionPage(bool increment)
    {
        if (increment) { page++; } else { page--; }
        if (page < 1)
        {
            page = GetMax();
        }
        if (page > GetMax())
        {
            page = 1;
        }
        pageText.text = "page:\n" + page + "/" + GetMax();
        SetPosition(TempSoul);
    }
    public SoulData TempSoul;
    public void ResetSub()
    {
        index = -1;
        NextCompanionMinion(true);
    }
    public void NextCompanionMinion(bool increment)
    {

        if (increment)
        { index++; }
        else
        { index--; }
        if (index < 0)
        {
            index = subminions.Count - 1;
        }
        if (index >= subminions.Count)
        {
            index = 0;
        }
        minionText.text = "minion:\n" + index + "/" + (subminions.Count - 1);
        if (subminions.Count > 0)
        {
            MinionShower.gameObject.SetActive(true);
            MinionShower.SetData(subminions[index]);
            MinionShower.updata(index, subminions.Count - 1, false, false,true);
        }
        else { MinionShower.gameObject.SetActive(false); }
        //SetPosition();
    }
    public int GetMax()
    {
        return 1+ ((minions.Count-1) / 4);// to do: magic number 4 is limit of status windows per page
    }


    public void SetMinions(List<UnitData> a)
    {
        if (a == null) { subminions.Clear(); }
        else { subminions = a; }
    }
    public void test(CompanionDataShow cds)
    {
        
        cds.updata(cds.Button.value, minions.Count - 1, false, true,true);
        //minions.IndexOf(cds.Data)
    }
    public void Set4()
    {
        for (int i = 0; i < 4; i++)
        {
            ListStatusWindows.Add(Instantiate(prefab, parent));
        }
    }
    public Vector2 offset;
    public void SetPosition(SoulData s)
    {
        int j = 0;
        for (int i = 0; i < ListStatusWindows.Count; i++)
        {
            j = i + ((page - 1) * 4);
            //print("j="+j);
            if (j < minions.Count)
            {
                ListStatusWindows[i].SetData(minions[j]);
                ListStatusWindows[i].buttonAdder.value=j;
                ListStatusWindows[i].Button.value=j;
                ListStatusWindows[i].SetSoulImage(s);
                ListStatusWindows[i].updata(j, minions.Count - 1, true, true,true);
                ListStatusWindows[i].gameObject.SetActive(true);
            }
            else
            {
                ListStatusWindows[i].gameObject.SetActive(false);
            }
        }
    }
}
