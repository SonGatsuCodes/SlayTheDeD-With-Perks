using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "new item", menuName = "things/Item")]
public class ItemData : ScriptableObject
{
    public int cod; public List<int> value; public int times;
    public List<effects> efeito;
    public Target target;
    public AI_Target alvo;
    public Trigger activation;
    public List<bool> reverse;
    public Sprite art;
    public int gold;
    public List<UnitData> minions;
    public List<BuffsAndDebuffsData> buff;
    public ItemData Upgrade, Downgrade;
    public AtkAnimationData anime;
    public SoulData soul;
    public void Init(ItemData item)
    {
        cod = item.cod;
        value = new List<int>(item.value);
        times = item.times;
        efeito = new List<effects>(item.efeito);
        target = item.target;
        alvo = item.alvo;
        activation = item.activation;
        reverse = new List<bool>(item.reverse);
        art = item.art;
        gold = item.gold;
        minions = new List<UnitData>(item.minions);
        buff = new List<BuffsAndDebuffsData>(item.buff);
        anime= item.anime;
        Upgrade = item.Upgrade;
        Downgrade = item.Downgrade;
        soul=item.soul;
    }
}
