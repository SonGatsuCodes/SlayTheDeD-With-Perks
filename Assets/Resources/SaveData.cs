using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(fileName ="Save",menuName="things/nevereveragain/Saves")]
public class SaveData : ScriptableObject
{
    public string identification;
    public List<ItemData> items;
    public List<SoulData> souls;
    public DeckData deck;
    public UnitData player;
    //public List<UnitData> minions;
    public void init(List<ItemData> allItems,DeckData deck1,UnitData unidade,List<SoulData> souls1)
    {
        items.Clear();
        for(int i=0; i<allItems.Count;i++)
        {
            items.Add(allItems[i]);
        }
        //DeckData d=DeckData.CreateInstance("DeckData") as DeckData;
        deck.all.Clear();
        for(int i=0; i<deck1.all.Count;i++)
        {
            deck.all.Add(deck1.all[i]);
        }
        //player=UnitData.CreateInstance("UnitData") as UnitData;
        player.initUnitData(unidade);
        souls.Clear();
        for(int i=0; i<souls1.Count;i++)
        {
            souls.Add(souls1[i]);
        }
    }
}
