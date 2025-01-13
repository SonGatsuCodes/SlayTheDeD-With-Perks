using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    public Manager M;
    public List<CardCode> cards;
    public List<RelicShower> relics;
    public void PopulateShop()
    {
        print("populated dhop?");
        PopulateShop(M.DB.AllCards);
        PopulateShop(M.DB.AllRelics);
    }

    public void Buy(CardData card,int value,DragAndDrop DAD)
    {
        if(M.gold>=value){M.AddCard(card);M.AddGold(-value);DAD.buyable=false;DAD.SetValueVisible(false);}
        else{print("too poor to buy");}
    }
    public void Remove(CardData card,int value,DragAndDrop DAD)
    {
        if(M.gold>=value){
        M.RemoveCard(card);M.AddGold(-value);DAD.removable=false;DAD.SetValueVisible(false);
        M.allButtons.SolveCast(classes.RemoveCard);
        }
    }
    public void Upgrade(CardData New,int value,DragAndDrop DAD,CardData Old)
    {
        if(M.gold>=value){
            M.UpgradeCard(New,Old);M.AddGold(-value);/*DAD.removable=false;DAD.SetValueVisible(false);*/
        M.allButtons.SolveCast(classes.UpgradeCard);}
    }
    public void Buy(ItemData item,int value,RelicShower relic)
    {
        if(M.gold>=value){M.AddRelic(item);M.AddGold(-value);relic.button.classe=classes.noClass;relic.SetValueVisible(false);}
        else{print("too poor to buy");}
     //   print( "relic buy ?");
    }
    public Transform origin; 
    public GameObject cardRemover,cardUpgrader;
    public void PopulateShop(List<CardData> list)
    {
        GameObject a;
        CardCode card;
        Vector3 v= origin.position;
        for (int i = 0;       cards.Count< list.Count; i++)
        {
            a = Instantiate(M.prefab);
            card = a.GetComponent<CardCode>();
            cards.Add(card);
        }
        cardRemover.SetActive(true);
        cardUpgrader.SetActive(true);
        for (int i = 0; i < cards.Count && i<20; i++)//to do:magic number solve in future
        {
            card = cards[i];
            /*if (i >= many)            {                card.transform.position = new Vector3(-3000, 0, 0);///todo: magic number fix it ....pretty please?            }*/
            //else
            {
               //r=RandomList[i]; 
                card.code.CIS.info = list[i];
                card.code.CIS.NewInfo();
                card.sortingGroup.sortingOrder = 180;
                card.transform.position =new Vector3(v.x+(i-((i/cardperline)*cardperline))*width,v.y-(i/cardperline)*height); 
                //new Vector3(1920 / 2 + i * 220, 1080 / 2, 0);//todo: magic number fix it 
                card.transform.SetParent(origin, true);
                card.code.buyable = true;
                card.code.SetValue(true);
                card.code.SetValueVisible(true);
                //card.code.ri = ri;
            }
        }
    //}
    }
    public void PopulateShop(List<ItemData> list)
    {
        GameObject a;
        RelicShower card;
        Vector3 v= origin.position;
        v=new Vector3(v.x+(20-((20/cardperline)*cardperline))*width,v.y-(20/cardperline)*height);// to do: freaking magic numbers         //CardRewardShower.gameObject.SetActive(true);        //int r=0;        //List<int> RandomList=M.PRS.RandomNumberNoRepeat(many,0,list.Count);
        for (int i = 0;         //list.Count 
        relics.Count< list.Count; i++)
        {
            a = Instantiate(M.relicPrefab.gameObject);
            card = a.GetComponent<RelicShower>();
            relics.Add(card);
        }
        for (int i = 0; i < relics.Count && i<(4*Relicperline); i++)//to do:magic number solve in future
        {
            card = relics[i];            /*if (i >= many)            {                card.transform.position = new Vector3(-3000, 0, 0);///todo: magic number fix it ....pretty please?            }*/            //else
            {               //r=RandomList[i];
               card.item=list[i];                 //card.code.CIS.info = list[i];
                card.SetArt();                //card.code.CIS.NewInfo();
                card.SG.sortingOrder = 180;
                card.transform.position =new Vector3(v.x+(i-((i/Relicperline)*Relicperline))*widthRelic,v.y-(i/Relicperline)*heightRelic);                 //new Vector3(1920 / 2 + i * 220, 1080 / 2, 0);//todo: magic number fix it 
                card.transform.SetParent(origin, true);
                card.button.classe = classes.BuyRelic;
                card.SetValue(true);
                card.SetValueVisible(true);                //card.code.ri = ri;
            }
        }
    }
    public int width, height,cardperline;
    public int widthRelic, heightRelic,Relicperline;
    
}
