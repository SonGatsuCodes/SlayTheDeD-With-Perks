using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PrizeRewardShower : MonoBehaviour
{
    /*// Start is called before the first frame update    void Start()    {            }    // Update is called once per frame    void Update()   {            }*/
    public Manager M;
    public List<Rewards> item;
    public List<RewardItem> Showers;
    public Transform rewardholder;
    public GameObject prefabReward;
    public Vector2 offset;
    public void SetRewards()
    {
        if (Showers.Count == 0)
        {
            CreateAllRewards();
        }
        else
        {
            organizeRewards();
        }
    }
    public void CreateAllRewards()
    {
        GameObject g; RewardItem RI;// Vector3 v=rewardholder.position;
        for (int i = 0; i < item.Count; i++)
        {
            g = Instantiate(prefabReward, rewardholder);
            RI = g.GetComponent<RewardItem>();
            RI.SetData(item[i]);// v.y=offset.x+(-offset.y*i);//  g.transform.position = v;
            Showers.Add(RI);
        }
        organizeRewards();
    }
    public void CreateItem(RewardData data)
    {
        //to do: manage pooling items (not instantiate)
        GameObject g; RewardItem RI;
        g = Instantiate(prefabReward, rewardholder);
        RI = g.GetComponent<RewardItem>();
        RI.SetData(data.r);
        Showers.Add(RI);
        organizeRewards();
    }
    public int page = 0;
    public TextMeshPro ActualPageText;
    public void PageCounter(bool increment)
    {
        page = increment ? page + 1 : page - 1;
        if (page < 1) { page = ((Showers.Count - 1) / 7) + 1; }
        if (page > 1 + ((Showers.Count - 1) / 7)) { page = 1; }
        ActualPageText.text = "page:\n" + page.ToString() + "/" + (Showers.Count / 7);
        //ShowAllCards(true);
        organizeRewards();
    }
    public void organizeRewards()
    {
        int mod = (page - 1) * 7;
        if (Showers.Count == 0) {M.allButtons.SolveCast(classes.ShowMap);
         M.MC.SetPathstrue();M.HBC.button(buttonNames.reward).StartShining(false); }
        Vector3 v = rewardholder.position;
        for (int i = 0; Showers.Count > i + mod && i < 7 + 1; i++)
        {
            v.y = offset.x + (-offset.y * i);            //print("index="+i+"mod="+mod);
            Showers[i].transform.position = v;
        }
    }
    public void Break(RewardItem ri)
    {//   bool breakable=false;
        switch(ri.Data.type)
        {
            case RewardType.MultipleCards:
            ShowCards(ri.Data.pool, ri,ri.Data.many);
            break;
            case RewardType.Scene:
            M.DisActive();
            M.SetDialogue();
            M.DS.SetSourceItem(ri);
            M.HBC.button(buttonNames.reward).StartShining(false);
            break;
            case RewardType.Battle:
            M.allButtons.SolveCast(classes.CloseAllTabs);
            M.SetItemDes(ri);
            //M.HBC.button()
            break;
            case RewardType.UpgradeCard:
            //M.allButtons.SolveCast(classes.CloseAllTabs);
            //M.SetItemDes(ri);
            //M.HBC.button()
            case RewardType.RemoveCard:
            //M.allButtons.SolveCast(classes.CloseAllTabs);
            //M.SetItemDes(ri);
            //M.HBC.button()
            M.RI=ri;
            break;
            default :
            BreakForReal(ri); 
            break;
           // BreakForReal(ri);
            //Break;
        }
        /*if(ri.Data.type == RewardType.MultipleCards || ri.Data.type == RewardType.Scene)
        {breakable=true;}
        if (breakable) {  }
        else
        {
            BreakForReal(ri);
        }*/
    }
    public void BreakForReal(RewardItem ri)
    {
        Showers.Remove(ri);
        organizeRewards();
        Destroy(ri.gameObject);
        
    }
    public Transform CardRewardShower;
    public TextMeshPro DisplayText;
    public List<CardCode> CardRewards;

    public void ShowCards(List<CardData> list, RewardItem ri,int many)
    {
        GameObject a;
        Vector3 v=M.ShopShower.origin.position;
        int cardperline=M.ShopShower.cardperline,height=M.ShopShower.height,width=M.ShopShower.width;
        CardCode card;
        CardRewardShower.gameObject.SetActive(true);        
        closeDiplay.classe=classes.CloseCardSelection2;
        DisplayText.text = "click any card to add";
        int r=0;
        List<int> RandomList=RandomNumberNoRepeat(many,0,list.Count);
        for (int i = 0; 
        //list.Count 
        many> CardRewards.Count; i++)
        {
            a = Instantiate(M.prefab);
            card = a.GetComponent<CardCode>();
            CardRewards.Add(card);
        }
        for (int i = 0; i < CardRewards.Count; i++)
        {
            card = CardRewards[i];
            if (i >= many)
            {
                card.transform.position = new Vector3(-3000, 0, 0);///todo: magic number fix it ....pretty please?
            }
            else
            {

               r=RandomList[i]; 
                card.code.CIS.info = list[r];
                card.code.CIS.NewInfo();
                card.sortingGroup.sortingOrder = 180;
                card.transform.position = new Vector3(v.x+(i-((i/cardperline)*cardperline))*width,v.y-(i/cardperline)*height); 
        //        card.transform.position = new Vector3(1920 / 2 + i * 220, 1080 / 2, 0);//todo: magic number fix it 
                card.transform.SetParent(CardRewardShower, true);
                card.code.addable = true;
                card.code.ri = ri;
            }
        }
    }
    public FakeButton closeDiplay;
    public void ShowCards(List<CardData> list,bool remove,int many)
    {
        GameObject a;
        Vector3 v=M.ShopShower.origin.position;
        CardCode card;
        CardRewardShower.gameObject.SetActive(true);
        closeDiplay.classe=classes.CloseCardSelection;
        if(remove){DisplayText.text = "click any card to remove";}
        else{DisplayText.text = "click any card to upgrade";}
        int cardperline=M.ShopShower.cardperline,height=M.ShopShower.height,width=M.ShopShower.width;
        //int r=0;        //List<int> RandomList=RandomNumberNoRepeat(many,0,list.Count);
        for (int i = 0; 
        //list.Count 
        many> CardRewards.Count; i++)
        {
            a = Instantiate(M.prefab);
            card = a.GetComponent<CardCode>();
            CardRewards.Add(card);
        }
        for (int i = 0; i < CardRewards.Count; i++)
        {
            card = CardRewards[i];
            if (i >= many)
            {
                card.transform.position = new Vector3(-3000, 0, 0);///todo: magic number fix it ....pretty please?
            }
            else
            {          //     r=RandomList[i]; 
                card.code.CIS.info = list[i];
                card.code.CIS.NewInfo();
                card.sortingGroup.sortingOrder = 180;
                card.transform.position = new Vector3(v.x+(i-((i/cardperline)*cardperline))*width,v.y-(i/cardperline)*height); 
                //new Vector3(1920 / 2 + i * 220, 1080 / 2, 0);//todo: magic number fix it 
                card.transform.SetParent(CardRewardShower, true);
                if(remove){card.code.removable = true;card.code.upgradable = false;}
                else{card.code.upgradable = true;}                //card.code.ri = ri;
            }
        }
    }
    
    public List<int> RandomNumberNoRepeat(int size,int min,int max)
    {//put this shit in DB , i dont put now because tired so can be a bad decision
        List<int> a=new List<int>() ;
        int r=UnityEngine.Random.Range(min,max),loophole=0;

        for (int i = 0;a.Count<size;i++)
        {
            r=UnityEngine.Random.Range(min,max);
            if(a.Contains(r)){loophole++;}
            else{a.Add(r);}
            if(loophole>999){a.Add(0);print("this gonna come to bite you in the ass someday do better next time");}//failsafe against infinite loop
        }
        return a;
    }
    public void CardSelected()
    {
        CardRewardShower.gameObject.SetActive(false);
    }
}
[Serializable]
public class Rewards
{
    public Sprite art;
    [TextArea(4, 10)]
    public string description;
    public RewardType type;
    [Range(0, 100)]
    public float chance;
    public int value;
    public int many;
    public ItemData item;
    public CardData card;
    public List<CardData> pool;
    public DialogData DD;
    public UnitData enemy;
    public SoulData soul;
    public DeckData deck;
    public SaveData profession;
    public bool Skipable=true;
}
/*public class CardDatas{    public CardData card;    public float chance;}*/
public enum RewardType
{
    noReward, Money, Relic, Card, Soul, MultipleCards, Heal,Scene,Battle,MaxHP,Minion,DungeonReroll,UpgradeCard,RemoveCard,DeckChange,SaveChange
}