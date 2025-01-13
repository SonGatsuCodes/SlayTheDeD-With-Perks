using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Manager M;
    public List<UnitControl> playerTeam;
    public List<UnitControl> enemyTeam;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartCombat()
    {

    }
    

}

[Serializable]
public class CardBuffs
{
    [SerializeField]
    public int cardCod;
    [SerializeField]
    public List<mods> mods;
}
[Serializable]
public class mods
{
    [SerializeField]
    public int buff;
    [SerializeField]
    public Modifier ID;
}

public enum Modifier { none, cost, atk }