using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FakeButton : MonoBehaviour
{
    public Manager code;
    public classes classe;
    public RelicShower relic;
    public EventType evento;
    public MapCreator MC;
    public GameObject RS;
    public PrizeRewardShower PRS;
    public RewardItem RI;
    public bool updater=false;
    public bool interectable=true;
    public TextMeshPro textButton;
   // public SpriteRenderer sprite;
    public Collider2D collider1;
    public ChoiceShower choice;
    ///<summary> update interactability of the button
    public void SetInteractable(bool b)
    {
//        print(this.name+"="+b);
        interectable=b;
    }
    public void OnMouseEnter()
    {
        //print("mouse entered");
        //Vector3 v=transform.position;
        //v.z=v.z-.1f;
        updater=true;
    }
    public void OnMouseExit()
    {
     //   M.show.SetActive(false);
     updater=false;
    }
    public void OnMouseDown()
    {
        if (updater && interectable)
        { triggedButton(); }
    }
    public void SolveCast(classes c)
    {
        print(c);
        switch (c)
        {
            case classes.EndTurn:
                code.EndTurn();
                break;
            case classes.ChangePreview:
                code.ChangePreview();
                break;
            case classes.Next:
                code.PageCounter(true);
                break;
            case classes.Previous:
                code.PageCounter(false);
                break;
            case classes.DeckShower:
                //updater=false;
                code.DisActive();
                code.ShowAllCards();
                code.HBC.button(buttonNames.deck).StartShining(false);
                break;
            case classes.Relic:
                print(relic.item.name);
                //code.ChangeRelicPreviewPage();
                break;
            case classes.RelicNext:
                code.ChangeRelicPreviewPage(true);
                break;
            case classes.RelicPrevious:
                code.ChangeRelicPreviewPage(false);
                break;
            case classes.TriggerEvent:
                evento.TriggerEventor(gameObject, false);
                SR.color = new Color(1,1,1,.2f);
                code.ResolveEvents(evento.eventEnum);
                break;
            case classes.ShowMap:
                code.DisActive();
                //code.HBC.button(buttonNames.map).StartShining(false);
                MC.PopulateMap();
                break;
            case classes.CloseAllTabs:
                code.DisActive();
                code.EnableCollider();
                //code.HBC.button(buttonNames.battle).StartShining(false);
                break;
                case classes.CallShop:
                code.DisActive();//code.HBC.button(buttonNames.shop).StartShining(false);
                //code.ShopShower.PopulateShop();
                code.ShopOn();
                break;
            case classes.Rewards:
                code.DisActive();
                code.SetRewards();
                //code.HBC.button(buttonNames.reward).StartShining(false);
                break;
            case classes.DialogShow:
                code.DisActive();
                code.SetDialogue();
                //code.HBC.button(buttonNames.reward).StartShining(false);
                break;
            case classes.GetReward:
                code.AddReward(RI.Data);
                /*GenericPropertyJSON:{"name":"item","type":-1,"arraySize":5,"arrayType":"Rewards","children":[{"name":"Array","type":-1,"arraySize":5,"arrayType":"Rewards","children":[{"name":"size","type":12,"val":5},{"name":"data","type":-1,"children":[{"name":"art","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"15436b04d8e1b3149ba923da02167eec\",\"localId\":21300000,\"type\":3,\"instanceID\":30066}"},{"name":"description","type":3,"val":"the relic is here"},{"name":"type","type":7,"val":"Enum:Relic"},{"name":"chance","type":2,"val":27.4},{"name":"value","type":0,"val":3},{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"6d43e9fa6b761214eaa712a4f6f220b7\",\"localId\":11400000,\"type\":2,\"instanceID\":30142}"},{"name":"card","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"}]},{"name":"data","type":-1,"children":[{"name":"art","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"e9ae77645bd9d194b984afc419f7fe40\",\"localId\":21300000,\"type\":3,\"instanceID\":30466}"},{"name":"description","type":3,"val":"25 gold coins for you"},{"name":"type","type":7,"val":"Enum:Money"},{"name":"chance","type":2,"val":36.1},{"name":"value","type":0,"val":25},{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"},{"name":"card","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"}]},{"name":"data","type":-1,"children":[{"name":"art","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"d04646af800fea64b825ec89c638a5b8\",\"localId\":21300000,\"type\":3,\"instanceID\":30462}"},{"name":"description","type":3,"val":"a new card of gura"},{"name":"type","type":7,"val":"Enum:Card"},{"name":"chance","type":2,"val":35.4},{"name":"value","type":0,"val":3},{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"},{"name":"card","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"dddde5f24c7e6ea4482404e4a9ed6c9b\",\"localId\":11400000,\"type\":2,\"instanceID\":30050}"}]},{"name":"data","type":-1,"children":[{"name":"art","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"84f659ac695e55944a7c547409646b43\",\"localId\":21300000,\"type\":3,\"instanceID\":30798}"},{"name":"description","type":3,"val":"a soul of the necromancer"},{"name":"type","type":7,"val":"Enum:Soul"},{"name":"chance","type":2,"val":35.4},{"name":"value","type":0,"val":3},{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"},{"name":"card","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"dddde5f24c7e6ea4482404e4a9ed6c9b\",\"localId\":11400000,\"type\":2,\"instanceID\":30050}"}]},{"name":"data","type":-1,"children":[{"name":"art","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"d04646af800fea64b825ec89c638a5b8\",\"localId\":21300000,\"type\":3,\"instanceID\":30462}"},{"name":"description","type":3,"val":"this can be a curse maybe?"},{"name":"type","type":7,"val":"Enum:No Reward"},{"name":"chance","type":2,"val":35.4},{"name":"value","type":0,"val":3},{"name":"item","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"\",\"localId\":0,\"type\":0,\"instanceID\":0}"},{"name":"card","type":5,"val":"UnityEditor.ObjectWrapperJSON:{\"guid\":\"dddde5f24c7e6ea4482404e4a9ed6c9b\",\"localId\":11400000,\"type\":2,\"instanceID\":30050}"}]}]}]}*/
                code.PRS.Break(RI);
                //Destroy(gameObject);
                break;
            case classes.noClass:
                break;
                case classes.RewardListNext:
                PRS.PageCounter(true);
                break;
                case classes.RewardListBack:
                PRS.PageCounter(false);
                break;
                case classes.BuyRelic:
                code.ShopShower.Buy(relic.item,relic.gold,relic);
                break;
                case classes.SellReward:
                //code.ShopShower.Buy(relic.item,relic.gold,relic);
                if(RI.Data.Skipable){
                if(RI.Data.type==RewardType.Money){}
                else{code.AddGold(RI.Data.value);}
                PRS.BreakForReal(RI);}
                break;
                case classes.RemoveCard:
                code.DisActive();
                code.ShowSelection(CardMode.Removable,50);
                //code.SetRewards();
                //List<CardData> all=code.DB.saves[code.saveSlot].deck.all;
                //code.PRS.ShowCards(all,true,all.Count);                
                break;
                case classes.UpgradeCard:
                code.DisActive();
                code.ShowSelection(CardMode.Upgradable,50);
                //code.SetRewards();
                //List<CardData> all1=code.DB.saves[code.saveSlot].deck.all;
                //code.PRS.ShowCards(all1,false,all1.Count);                
                break;
                case classes.CloseCardSelection:
                code.PRS.CardRewardShower.gameObject.SetActive(false);
                SolveCast(classes.CallShop);
                break;
                case classes.CloseCardSelection2:
                code.PRS.CardRewardShower.gameObject.SetActive(false);
                SolveCast(classes.Rewards);
                break;
                case classes.DialogNext:
                code.DS.ChangePreview(true);
                break;
                case classes.DialogBack:
                code.DS.ChangePreview(false);                
                break;
                case classes.DialogSolver:
                code.DS.Solve(choice);
                break;
                case classes.TestMinion:
                //code.DS.Solve(choice);
                //CDS.updata();
                code.CC.test(CDS);
                break;
                case classes.NextCom:
                code.CC.NextCompanionPage(true);
                break;
                case classes.BackCom:
                code.CC.NextCompanionPage(false);
                break;
                case classes.NextSub:
                code.CC.NextCompanionMinion(true);
                break;
                case classes.BackSub:
                code.CC.NextCompanionMinion(false);
                break;
                case classes.ShowCompanions:
                code.DisActive();
                code.ShowTeam();
                code.CC.SetAllCompanions(null);
                break;
                case classes.ShowSouls:
                if(code.BattleStop)
                {code.DisActive();
                code.ShowSoul();
                code.SCI.SetAllButtons();
                code.SCI.Reset();}
                break;
                case classes.BackSoul:
                code.SCI.NextSoulPage(false);
                break;
                case classes.nextSoul:
                code.SCI.NextSoulPage(true);
                break;
                case classes.BackSoulRelic:
                code.SCI.NextSoulPageRelic(false);
                break;
                case classes.nextSoulRelic:
                code.SCI.NextSoulPageRelic(true);
                break;
                case classes.SoulSolver:
                code.SCI.ShowData(soul.index);
                break;
                case classes.SoulAdder:
                code.SCI.AddSoul(target);
                break;
                case classes.SoulAdderRelic:
                code.ChangeRelic(value,relic.item);
                break;
                case classes.SoulAdderAtk:
                code.CC.AddSoulToAtk(value);
                SolveCast(classes.ShowCompanions);
                break;                
            case classes.Surrender:
                code.Surrender();
                break;
            case classes.NewGame:
            RS.SetActive(false); 
                code.NewGame();
                break;
            case classes.Quit:
                code.Exit();
                break;
                case classes.Draw:
                code.Draw(value);
                break;
            default: break;
        }
    }
    public int value=-1;
    public AdderTarget target;
    public SoulContainer soul;
    public CompanionDataShow CDS;
    public void triggedButton()
    {
        SolveCast(classe);
        //print("it works!!!!");
    }
    public void StartShining(bool b)
    {
        work = b;Color c=SR.color;c.a=1;
        if (b) { StartCoroutine(Shining2(c)); }
        else { SR.color=c; }
        //button.SetInteractable(b);
    }
    public bool work;
    public SpriteRenderer SR;
    public void SetWork(bool Work)
    {
        work = !Work;
        if(SR==null){
            SR = GetComponent<SpriteRenderer>();
        StartCoroutine(Shining(SR));}
        else{}
    }
    public IEnumerator Shining(SpriteRenderer sprite)
    {
        Color old=sprite.color;
        float f = Time.time, t;
        while (work)
        {
            t = Time.time - f;
            sprite.color = Color.Lerp(new Color(old.r,old.g,old.b, 0), new Color(old.r,old.g,old.b, 1), t);
            yield return new WaitForSeconds(0);
            if (Time.time > (f + 1)) { f = Time.time; work = false; }
            work=false;
        }
    }
    
    public IEnumerator Shining2(Color c)
    {
        float f = Time.time, t;
        while (work)
        {
            t = Time.time - f;
            SR.color = Color.Lerp(new Color(c.r,c.g,c.b, 0), new Color(c.r,c.g,c.b, 1), t);
            yield return new WaitForSeconds(0);
            if (Time.time > (f + 1)) { f = Time.time; }
        }
    }
}
public enum classes{noClass,EndTurn,DeckShower,Next,Previous,Relic,ChangePreview,TriggerEvent,ShowMap,CloseAllTabs,
RelicNext,RelicPrevious,Rewards,GetReward,RewardListNext,RewardListBack,CallShop,BuyRelic,SellReward,RemoveCard,
CloseCardSelection,CloseCardSelection2,UpgradeCard,DialogShow,DialogNext,DialogBack,DialogSolver,TestMinion,NextCom,
NextSub,BackCom,BackSub,ShowCompanions,ShowSouls,nextSoul,BackSoul,SoulSolver,SoulAdder,nextSoulRelic,BackSoulRelic,
SoulAdderRelic,SoulAdderAtk,Surrender,Quit,NewGame,Draw}

public enum AdderTarget{noTarget,Relic,Card,Atk}