using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StatusControl : MonoBehaviour
{
    public UnitControl UC;
    public SpriteRenderer prefabTest, origin;
    public StatusItem prefabTest2D;
    public List<StatusItem> allItems;
    public List<StatusItem> pool;
    //public List<> virtualStatus;

    public int height, width, statusPerLine, max;
    public void SetWidthHeight(int a)
    {
        height = height / a; width = width / a;
    }
    public StatusItem CreateStatus(BuffsAndDebuffsData badd)
    {
        StatusItem s;
        prefabTest2D.gameObject.SetActive(true);
        if (pool.Count == 0) { s = (Instantiate(prefabTest2D, origin.transform)); }
        else { s = (pool[0]); pool.RemoveAt(0); }
        prefabTest2D.gameObject.SetActive(false);
        s.SetData(badd);
        s.updateInfo();
        allItems.Add(s);
        return s;
    }
    public void SetPosition(int i, Transform t, Vector3 v)
    {
        t.position = new Vector3(v.x + (i - ((i / statusPerLine) * statusPerLine)) * width, v.y
        + (i / statusPerLine) * height);
    }
    public void SetAllPosition()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            SetPosition(i, allItems[i].transform, origin.transform.position);
        }
    }
    public void ResetStatus()
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            SetPosition(1000, allItems[i].transform, origin.transform.position);
            pool.Add(allItems[i]);
            allItems[i].buffs.value = 0;
        }
        if (UC == UC.M.player) { UC.M.UpdateAllCards(); }
        allItems.Clear();
    }
    public void RemoveUsed()
    {
        List<StatusItem> removed = new List<StatusItem>();
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].buffs.value == 0)
            {
                SetPosition(1000, allItems[i].transform, origin.transform.position);
                pool.Add(allItems[i]);
                removed.Add(allItems[i]);
            }
        }
        for (int i = 0; i < removed.Count; i++)
        { allItems.Remove(removed[i]); }
        SetAllPosition();
        //allItems.RemoveRange(removed);        //allItems.Clear();
    }
    public void RemoveThisStatus(BuffsAndDebuffs bad)
    {
        StatusItem r;
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].buffs.identifier == bad)
            {
                r = allItems[i]; SetPosition(1000, r.transform, origin.transform.position);
                pool.Add(r); allItems.Remove(r); i = 1000;
            }
        }
    }
    public BuffsAndDebuffsData StatusFromEffect(effects e, int v, bool posi)
    {
        BuffsAndDebuffsData badd = new BuffsAndDebuffsData(posi, v, BuffsAndDebuffs.none, null, Trigger.None, null);
        var t = e switch
        {

            effects.stats_strength => BuffsAndDebuffs.strength,
            effects.stats_dexterity => BuffsAndDebuffs.dexterity,
            effects.stats_constitution => BuffsAndDebuffs.constitution,
            effects.stats_inteligence => BuffsAndDebuffs.inteligence,
            effects.stats_wisdom => BuffsAndDebuffs.wisdom,
            effects.stats_charisma => BuffsAndDebuffs.charisma,
            effects.stats_stamina => BuffsAndDebuffs.stamina,
            effects.direct_poison => BuffsAndDebuffs.poison,
            effects.direct_bleed => BuffsAndDebuffs.bleed,
            effects.condition_Rage => BuffsAndDebuffs.rage,
            effects.condition_Unrage => BuffsAndDebuffs.unrage,
            effects.boost_strength => BuffsAndDebuffs.boost_strength,
            effects.boost_dexterity => BuffsAndDebuffs.boost_dexterity,
            effects.boost_constitution => BuffsAndDebuffs.boost_constitution,
            effects.boost_inteligence => BuffsAndDebuffs.boost_inteligence,
            effects.boost_wisdom => BuffsAndDebuffs.boost_wisdom,
            effects.boost_luck => BuffsAndDebuffs.boost_luck,
            effects.boost_stamina => BuffsAndDebuffs.boost_stamina,
            effects.boost_charisma => BuffsAndDebuffs.boost_charisma,

            _ => BuffsAndDebuffs.none,
        };
        badd.SetData(UC.M.DB.GetStatusInfo(t));
        return badd;
    }
    public void AddStatus(BuffsAndDebuffsData BADD)
    {
        StatusItem s;
        s = Contains(BADD);
        if (s == null) { s = CreateStatus(BADD); }
        s.updateInfo();        //print("status test");
        if (UC == UC.M.player) { UC.M.UpdateAllCards(); } else { UC.SetIntention(); }
        UC.UpdateAtri();
        StartCoroutine(UC.Blink1(s.SR, .2f));
        SetAllPosition();
    }
    public StatusItem Contains(BuffsAndDebuffsData BADD)
    {
        int v, v1;
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].buffs.identifier == BADD.identifier)
            {
                v1 = BADD.value;
                v = allItems[i].buffs.value;                    //print("v="+v+" v1="+v1+"bad.posite="+BADD.positive);
                v = v + v1;
                allItems[i].buffs.value = v;
                return allItems[i];
            }
        }        //return CreateStatus();
        return null;
    }
    public int Contains(BuffsAndDebuffs BAD)
    {
        int v = 0;
        for (int i = 0; i < allItems.Count; i++)
        {
            if(BAD==BuffsAndDebuffs.energy)
            {
                //i=allItems
                return UC.GS(status.mana).value1(status.mana, UC.M, this);
            }
            if (allItems[i].buffs.identifier == BAD)
            {               // mod=allItems[i].buffs.positive?1:-1;
                v = allItems[i].buffs.value;                //v=v+BADD.value;                //allItems[i].buffs.value=v;
                return v;
            }
        }
        return v;
    }
    public List<StatusItem> GetList(Trigger t)
    {
        List<StatusItem> SI = new List<StatusItem>();
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].buffs.activator == t) { SI.Add(allItems[i]); }
        }
        return SI;
    }


}
public enum BuffsAndDebuffs
{
    none, strength, dexterity, constitution, poison, inteligence, wisdom, luck, stamina, charisma
, bleed, rage, unrage, boost_strength, boost_dexterity, boost_constitution, boost_inteligence, boost_wisdom, boost_luck
, boost_stamina, boost_charisma, energy
}
[Serializable]
public class BuffsAndDebuffsData
{
    public bool positive;
    public int value;
    public BuffsAndDebuffs identifier;
    public StatusItem shower;
    public Trigger activator;
    public StatusData originalInfo;
    /*public void AddValue(int v)    {        value=value+v;    }*/
    public void SetData(BuffsAndDebuffsData b)
    {
        // int mod=(b.positive)?1:-1;
        positive = b.positive;
        value = b.value;
        identifier = b.identifier;
        shower = b.shower;
        activator = b.activator;
        originalInfo = b.originalInfo;
        //Invoke = b.Invoke;
    }
    public void SetData(StatusData s)
    {
        identifier = s.indentifier;
        activator = s.activator;
        originalInfo = s;

    }
    public BuffsAndDebuffsData(bool positive1, int value1, BuffsAndDebuffs identifier1,
    StatusItem shower1, Trigger activator1, StatusData origin)
    {
        //int mod=(positive1)?1:-1;
        positive = positive1;
        //Debug.Log(mod);
        value = value1;
        identifier = identifier1;
        shower = shower1;
        activator = activator1;
        originalInfo = origin;
        //Invoke = Invoke1;
    }
}