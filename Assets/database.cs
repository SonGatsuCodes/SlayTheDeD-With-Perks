using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName = "DataBase", menuName = "things/nevereveragain/Database")]
public class database : ScriptableObject
{
    public List<CardData> AllCards;
    public List<SaveData> saves;
    public List<RewardData> AllRewards;
    public List<atk> AllAtk;
    public List<ItemData> AllRelics;
    public List<SoulData> AllSouls;
    public List<Sprite> intentions;
    public List<Sprite> AllStatusSprites;
    public List<Sprite> eventArt;
    public List<UnitData> units;
    public AtkAnimationData standard,standardSummon;
    public Sprite GetIntention(intentions i) { return intentions[(int)i]; }
    public UnitData GetUnit() { int r = UnityEngine.Random.Range(0, units.Count); return units[r]; }
    public RewardData GetSoul()
    {
        List<RewardData> reward=new List<RewardData>();
        for(int i=0;i<AllRewards.Count;i++)
        {
            if(AllRewards[i].r.type==RewardType.Soul)
            {
                reward.Add(AllRewards[i]);
            }
        }
        int r = UnityEngine.Random.Range(0, reward.Count); return reward[r];
    }
    public UnitData GetUnit(enemyLevel EL)
    {
        List<UnitData> unitSorted = new List<UnitData>();
        for (int i = 0; i < units.Count; i++)
        { if (units[i].level == EL) { unitSorted.Add(units[i]); } }
        int r = UnityEngine.Random.Range(0, unitSorted.Count);
        if (unitSorted.Count > 0) { return unitSorted[r]; } else { Debug.Log("list null?"); return units[0]; }
    }
    public Color GetColor(RewardType r)
    {
        switch (r)
        {
            case RewardType.noReward: return Color.gray;
            case RewardType.Money: return Color.yellow;
            case RewardType.Card: return Color.green;
            case RewardType.Relic: return Color.blue;
            case RewardType.Soul: return Color.cyan;
            case RewardType.MultipleCards: return Color.green;
            case RewardType.Heal: return Color.red;
            default: return Color.black;
        }

    }
    public List<StatusData> AllStatusInfo;
    public StatusData GetStatusInfo(BuffsAndDebuffs indentifier)
    {
        foreach(StatusData sd in AllStatusInfo)
        {
            if(sd.indentifier==indentifier)
            {
                return sd;
            }
        }
        return AllStatusInfo[0];
    }
    public Color GetTeamColor(Target r)
    {
        switch (r)
        {
            case Target.Deck: return Color.gray;
            case Target.Discard: return Color.white;
            case Target.Empty: return Color.clear;
            case Target.EnemyTeam: return Color.red;
            case Target.Hand: return Color.green;
            case Target.PlayerTeam: return Color.blue;
            case Target.Random: return Color.black;
            default: return Color.yellow;
        }

    }
    public RewardData GetRandomReward()
    {
        int r = UnityEngine.Random.Range(0, AllRewards.Count);
        return AllRewards[r];
    }/*
    public Sprite GetSprite(BuffsAndDebuffs identifier)
    {
        int i = (int)identifier;
        return AllStatusSprites[i];
    }*/
    public BuffsAndDebuffs Mod(effects e)
    {
        switch (e)
        {
            case effects.direct_dmg: return BuffsAndDebuffs.strength;
            case effects.direct_shield: return BuffsAndDebuffs.dexterity;
            case effects.direct_heal: return BuffsAndDebuffs.wisdom;
            case effects.minion_summonMinion: return BuffsAndDebuffs.charisma;
            case effects.minion_createMinion: return BuffsAndDebuffs.charisma;
            case effects.card_cost: return BuffsAndDebuffs.energy;
            //case effects.heal:return BuffsAndDebuffs.constitution;
            default:
                return BuffsAndDebuffs.none;
        }        //return BuffsAndDebuffs.none;
    }
    public BuffsAndDebuffs Mod(status s)
    {
        switch (s)
        {
            case status.maxHealth: return BuffsAndDebuffs.constitution;
            case status.health: return BuffsAndDebuffs.constitution;
            case status.maxEnergy: return BuffsAndDebuffs.stamina;
            default: return BuffsAndDebuffs.none;
        }        //return BuffsAndDebuffs.none;
    }
    public float ModWeigth(status s)
    {
        return Weight(Mod(s));
    }
    public float Weight(BuffsAndDebuffs s)
    {
        float j = 0;
        switch (s)
        {
            case BuffsAndDebuffs.constitution: j = 5; break;
            case BuffsAndDebuffs.stamina: j = .5f; break;
            case BuffsAndDebuffs.inteligence: j = .5f; break;
            default: j = 0; break;
        }
        return j;
    }
    

    public const string y = "<color=#ffff00ff>", r = "<color=#ff0000ff>", g = "<color=#00ff00ff>", b = "<color=#ffffffff>", d = "</color>";
    
}
public enum intentions { none, random, atk, defend, effects, heal, summon, dead }
public enum enemyLevel { random, monster, elite, midBoss, boss }