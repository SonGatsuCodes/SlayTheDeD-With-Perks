using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "00-newBorn", menuName = "things/SoulData")]
public class SoulData : ScriptableObject
{
    public string nome;
    [TextArea(4,20)]
    public string desc;
    public Sprite art;
    public List<effects> CardEfeitos;
    public List<int> valueCard;
    public List<bool> reverse;
    public List<effects> RelicEfeitos;
    public List<int> valueRelic;
    public List<bool> reverseRelic;
    public Trigger relicTrigger;
    public Trigger CardUtilityTrigger;
    public string AtkName;
    public atk SigntureMove;
    public List<UnitData> unidades;
    public List<BuffsAndDebuffsData> buff;
    public SoulData Upgrade,Downgrade;
}
