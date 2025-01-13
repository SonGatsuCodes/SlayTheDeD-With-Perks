using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "new unit", menuName = "things/UnitData")]

public class UnitData : ScriptableObject
{
    public string nome;
    public string description;
    public int cod;
    public Sprite art;
    public List<atk> atks;
    public AI aI;
    public AI_Target target;
    public List<atributes> atributes;
    public enemyLevel level;
    public List<RewardData> AllRewards;
    public List<RewardData> Defeat;
    public List<UnitData> minions;
    public UnitData Upgrade,Downgrade;
    /*public UnitData createMinion(UnitData ud)
    {
        return new UnitData(ud);
    }*/
    /*public UnitData createMinion(string name1, string description1, int cod1, Sprite art1, List<atk> atks1, AI aI1, AI_Target target1,
     List<atributes> atributes1, enemyLevel level1, List<RewardData> AllRewards1, List<UnitData> minions1)
    {
        return new UnitData(name1,description1,cod1,art1,atks1,aI1,target1,atributes1,level1,AllRewards1,minions1);
    }*/
    public void initUnitData(UnitData ud)
    {
        nome = ud.nome;
        description = ud.description;
        cod = ud.cod; art = ud.art;
        atks = new List<atk>();
        atks.AddRange(ud.atks);
        aI = ud.aI;
        target = ud.target;
        atributes = new List<atributes>();
        atributes b ;//= new atributes();
        for (int i=0; i<ud.atributes.Count;i++)
        {/*            atributes a= ud.atributes[i];            b.nome = a.nome;            b.sprite = a.sprite;            b.text = a.text;            b.value = a.value;            b.mask = a.mask;            b.work = a.work;*/
            b= new atributes(ud.atributes[i].nome,ud.atributes[i].sprite,ud.atributes[i].text,
            ud.atributes[i].value,ud.atributes[i].mask,ud.atributes[i].work);
            atributes.Add(b);
        }        //to do: serviço porco faz direito 
        level = ud.level;
        AllRewards = new List<RewardData>();
        AllRewards.AddRange(ud.AllRewards);
        minions = new List<UnitData>();
        minions.AddRange(ud.minions);
    }
    public int GetStatus(status s)
    {

        return atributes[(int)s].value;
    }
    /*public UnitData(string name1, string description1, int cod1, Sprite art1, List<atk> atks1, AI aI1, AI_Target target1,
     List<atributes> atributes1, enemyLevel level1, List<RewardData> AllRewards1, List<UnitData> minions1)
    {
        name = name1; description = description1; cod = cod1; art = art1; atks = atks1; aI = aI1;
        target = target1; atributes = atributes1; level = level1; AllRewards = AllRewards1; minions = minions1;
    }*/
}

