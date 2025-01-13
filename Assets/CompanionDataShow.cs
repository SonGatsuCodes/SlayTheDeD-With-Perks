using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;

public class CompanionDataShow : MonoBehaviour
{
    public Manager M;
    public UnitData Data;

    public SpriteRenderer image;
    public TextMeshPro desc, number,maybename;
    //public CompanionDataShow CDS;
    //public List<UnitData> MinionList;
    public SoulData TempSoul;
    public SpriteRenderer soul;
    public FakeButton buttonAdder,Button;
    public void SetSoulImage(SoulData s)
    {
        TempSoul=s;
        if(s!=null)
        {soul.sprite=TempSoul.art;}
    }
    public void updata(int i,int max,bool cds,bool upMinionList,bool numberChange)
    {
        //print(this.gameObject.name);
        image.sprite = Data.art;
        desc.text = DescDealer(i,max);
        if(numberChange){number.text = i+"/"+max;}
        maybename.text= Data.nome.ToShortString(11);
        //cds.Data=Data.minions[0];
        if(upMinionList){M.CC.SetMinions(Data.minions);}
       //if(cds){ M.CC.ResetSub();}
    }
    public void SetData(UnitData D)
    {
        Data=D;
    }
    public string DescDealer(int i1 ,int max)
    {
        
        string s ="base info:"+M.CC.GetTeam(Data)+"\nname: "+Data.nome+".  position=("+ i1+"/"+max +")\n";
        s =s+ "MaxHP=" + Data.GetStatus(status.maxHealth);
        s = s + " MaxMP=" + Data.GetStatus(status.maxEnergy) + "\n";
        s = s + "atk list:\n";
        for (int i = 0; i < Data.atks.Count; i++)
        {
            s=s+"atk("+i+") = ";
            for (int j = 0; j < Data.atks[i].efeito.Count; j++)
            {
                s = s +(Data.atks[i].EnemyTarget?r:blue)+"{"+ Data.atks[i].value[j] + " " + Data.atks[i].efeito[j] +"}."+d;
            }
            s=s+"cost="+Data.atks[i].cost+"\n";            
        }
            print((TempSoul==null)+"/"+(!M.player.UD.minions.Contains(Data)));
            if(TempSoul==null || (!M.player.UD.minions.Contains(Data)))
            {
                buttonAdder.gameObject.SetActive(false);
                soul.gameObject.SetActive(false);
            }
            else
            {s=s+cyan+TempSoul.AtkName+":\n";buttonAdder.gameObject.SetActive(true);soul.gameObject.SetActive(true);
            for (int j = 0; j < TempSoul.SigntureMove.efeito.Count; j++)
            {
                s = s +(TempSoul.SigntureMove.EnemyTarget?r:blue)+"{"+ TempSoul.SigntureMove.value[j] + " " + TempSoul.SigntureMove.efeito[j] +"}."+d;
            }
            s=s+"cost="+TempSoul.SigntureMove.cost+d+"\n";            
            }
        s=s+"enemy priority = "+Data.target+"\n";
        /*if(Data.minions.Count > 0 || !(CDS==null)) // this show minions in a diferent way maybe delete
        {
            CDS.Data=Data.minions[0];
            CDS.MinionList=Data.minions;//s=s+"summon = "+Data.minions[0].nome+"\n";
            //CDS.updata();
        }*/
        return s;
    }
    public const string y = "<color=#ffff00ff>", r = "<color=#ff0000ff>", g = "<color=#00ff00ff>", b = "<color=#ffffffff>",
     d = "</color>",blue="<color=#0000ffff>",cyan="<color=#00FFC7ff>";
}
