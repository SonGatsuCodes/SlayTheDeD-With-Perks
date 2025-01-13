using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[CreateAssetMenu(fileName = "/Asset/card/nova carta", menuName = "things/cartas")]
public class CardData : ScriptableObject
{
    public string nome;
    [Range(0, 99)]
    public int code;
    public int custo, buy;
    public CardData upgrade, downgrade;
    public List<AtkData> special;
    public List<int> values;
    public List<effects> efeitos;
    public List<bool> reverse;
    [TextArea(3, 10)]
    public string descricao;
    public Sprite art, costArt, backart;
    public List<tags> tags;
    public cardtype2 reusability;
    public cardtype type;
    public Trigger actvision;
    public List<UnitData> minions;
    public List<BuffsAndDebuffsData> buff;
    public AtkAnimationData anime;
    public List<SoulData> souls;

    public void Init(CardData c)
    {
        // Copy all fields from c to this instance
        this.nome = c.nome;
        this.code = c.code;
        this.custo = c.custo;
        this.buy = c.buy;
        this.upgrade = c.upgrade;
        this.downgrade = c.downgrade;
        this.values = new List<int>(c.values); // Deep copy for List<int>
        this.efeitos = new List<effects>(c.efeitos); // Deep copy for List<effects>
        this.reverse = new List<bool>(c.reverse);
        this.descricao = c.descricao;
        this.art = c.art;
        this.costArt = c.costArt;
        this.backart = c.backart;
        this.tags = new List<tags>(c.tags); // Deep copy for List<tags>
        this.reusability = c.reusability;
        this.type = c.type;
        this.actvision = c.actvision;
        this.minions = new List<UnitData>(c.minions); // Deep copy for List<UnitData>
        this.buff = new List<BuffsAndDebuffsData>(c.buff); // Deep copy for List<BuffsAndDebuffsData>
        this.souls = new List<SoulData>(c.souls); // Deep copy for List<SoulData>
        this.anime = c.anime;
    }
}

public enum tags
{
    none, fist, handle, arrow, discard, herb, turndmgnegative, coldDmg, heatDmg
}
public enum effects
{
    direct_dmg, direct_heal, direct_shield, card_cost, card_repeat, triggers_bloodTrigger, triggers_discardTrigger,
     triggers_deadTrigger, minion_summonMinion, minion_createMinion, other_buffOrDebuff,
    direct_penetrate, stats_strength, stats_dexterity, stats_constitution, stats_inteligence, stats_wisdom,
     stats_charisma, stats_stamina, direct_poison, direct_bleed, condition_Rage, condition_Unrage, condition_Exaution
    , boost_strength, boost_dexterity, boost_constitution, boost_inteligence, boost_wisdom, boost_luck, boost_stamina,
    boost_charisma, other_none, card_RandomFromDeck, card_ADHD, card_create, card_upgrade, card_upgradeAtRandom,
    condition_FullDefense
    
}
public enum cardtype
{
    combat, defense, utility
}
public enum cardtype2
{
    normal, exaust, consum
}
[Serializable]
public class AtkData
{
    public int cost;
    public effects e;
    public int v;
    public bool reverse;
    public intentions Intention_art;
    public bool4 _____self________summon____minions____boss = new bool4(true, true, true, true);
    public List<UnitData> allMinions;
}