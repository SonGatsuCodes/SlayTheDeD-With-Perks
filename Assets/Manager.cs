using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Manager : MonoBehaviour
{
    public database DB;

    //public List<Power> startturn;2600-1000-300=1300
    public GameObject prefab, show, deckShower,showUpgradeInBattle;
    public CardCode showUpgrade;
    public StatusItem ShowStatus;
    public RelicShower relicPrefab;
    public LineRenderer L2;
    public UnitControl player, enemy, unitPrefab;
    public List<UnitControl> everybody;

    public List<Collider2D> PlayAreas;
    public CombatManager CM;
    public List<CardCode> Deck, Discard, Hand, consumed, Full, allCards;
    [Serialize]
    public List<AllLists> triggers;
    public List<ItemData> relics;
    public int basicDraw;
    public DeckData BasicDeck;
    public List<Collider2D> disabled;

    public void NewGame()
    {
        MC.PopulateMap();
        DisActive();
        DisActive();
        MC.PopulateMap();
        DS.UpdateData();
        DisActive();
        SetDialogue();
        //DS.UpdateData();

    }
    public SoulControlInventory SCI;
    public bool DisableColliders()
    {
        Collider2D bc2d;
        foreach (CardCode card in Hand)
        {
            bc2d = card.code.collider1; if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }
        }
        bc2d = (endTurnButton.collider1); if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }
        bc2d = (SurrenderButton.collider1); if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }

        foreach (UnitControl unit in everybody)
        {
            bc2d = unit.BC2d; if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }
            //disabled.Add(unit.BC2d);
        }
        foreach (RelicShower r in relics2)
        {
            bc2d = r.collider1; if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }
            //disabled.Add(r.collider1);
        }
        foreach (Collider2D p in PlayAreas)
        {
            bc2d = p; if (disabled.Contains(bc2d)) { } else { disabled.Add(bc2d); }
        }
        //disabled.AddRange(PlayAreas);
        foreach (Collider2D c in disabled)
        {
            c.enabled = false;
        }
        return true;
    }
    public void UpdateAllCards()
    {
        for (int i = 0; i < allCards.Count; i++) { if (allCards[i] == null) { } else { allCards[i].code.CIS.NewInfo(); } }
    }
    public bool EnableCollider()
    {
        foreach (Collider2D c in disabled)
        {
            if (c != null)
            {
                c.enabled = true;
            }
        }
        disabled.Clear();
        return true;
    }
    public ShopHandler ShopShower;
    public void ChangePreview()
    {
        int i = (int)actualdeck + 1;
        if (i >= Enum.GetValues(typeof(shower)).Length)
        { i = 0; }
        actualdeck = (shower)i;
        page = 1;
        CleanTempSouls();
        ShowAllCards(true);
    }
    public SaveData GetSave()
    {
        return DB.saves[saveSlot];
    }
    public void ShopOn()
    {
        HBC.button(buttonNames.shop).SetInteractable(true);
        ShopShower.gameObject.SetActive(!ShopShower.gameObject.activeInHierarchy);
    }
    // Start is called before the first frame update
    void Start()
    {
        //   CalcRect();        sadsad
        if (saveSlot == 0) { InstanciateSave(); }
        ClearCards();
        OrganizeHand();
        BasicDeck = DB.saves[saveSlot].deck;
        Install(BasicDeck);
    }
    public void InstanciateSave()
    {
        //SaveData.CreateInstance("SaveData");
        //SaveData s=SaveData.CreateInstance<SaveData>();//SaveData.CreateInstance("SaveData") as SaveData; 
        //DB.saves.Add(s);
        saveSlot = DB.saves.Count - 1;
        DB.saves[saveSlot].init(DB.saves[0].items, DB.saves[0].deck, DB.saves[0].player, DB.saves[0].souls);

    }
    public int turn = 0;
    public void PageCounter(bool increment)
    {
        page = increment ? page + 1 : page - 1;
        if (page < 1) { page = ((CardSelection().Count - 1) / 36) + 1; }
        if (page > 1 + ((CardSelection().Count - 1) / 36)) { page = 1; }
        ShowAllCards(true);
    }
    public List<CardCode> CardSelection()
    {
        switch (actualdeck)
        {
            case shower.deck:
                CloseAllCards(Discard);
                CloseAllCards(consumed);
                CloseAllCards(Full);
                return Deck;
            case shower.discard:
                CloseAllCards(Deck);
                CloseAllCards(consumed);
                CloseAllCards(Full);
                return Discard;
            case shower.hand:
                ChangePreview();
                return CardSelection();
            case shower.consume:
                CloseAllCards(Discard);
                CloseAllCards(Deck);
                CloseAllCards(Full);
                return consumed;
            case shower.fullDeck:
                CloseAllCards(Discard);
                CloseAllCards(Deck);
                CloseAllCards(consumed);
                return Full;
            default:
                print("????? how??");
                break;
        }
        return null;
    }
    public void FullDeckShower(DeckData deck)
    {
        CardCode card;
        pool.AddRange(Full);
        Full.Clear();
        for (int i = 0; deck.all.Count > Full.Count; i++)
        {
            GameObject g;
            if (pool.Count == 0) { g = Instantiate(prefab); card = g.GetComponent<CardCode>(); }
            else { card = pool[0]; pool.Remove(card); }
            Full.Add(card);
        }/*
        if(Full.Count>deck.all.Count)
        {
            for(int i=deck.all.Count;Full.Count<i;i++)
            {
            card=Full[i];

            }
        }*/
        for (int i = 0; i < Full.Count; i++)
        {
            card = Full[i];

            if (i < deck.all.Count)
            {
                card.code.CIS.info = deck.all[i]; card.code.CIS.NewInfo();
            }
            else
            {
                card.code.CIS.info = null;
            }
        }
    }
    public float heightDivisor, widthDivisor, heightoffset, widthoffset, depth;
    public shower actualdeck;    //public TextMeshPro 
    public int page = 1;
    public TextMeshPro PageText, selectedText;
    //to do: redo this shit from zero add fulldeck option
    public void ShowAllCards()
    {
        if (!deckShower.activeInHierarchy)
        {
            ShowAllCards(true);
            //DisableColliders();            //print("active");
        }
        else
        {
            CloseAllCards(Deck);
            CloseAllCards(consumed);
            CloseAllCards(Discard);            //CloseAllCards(Full);
            Target d = Target.Deck;
            if (CardSelection() == Discard) { d = Target.Discard; }
            if (CardSelection() == consumed) { d = Target.PlayerTeam; }
            for (int i = 0; i < CardSelection().Count; i++)
            {
                CardSelection()[i].code.CardMovement(CardSelection()[i].transform.position, PlayAreas[(int)d].transform.position, new Vector3(-3000, 0, 0));//to do: magic number -3000== offscreen???
                CardSelection()[i].SetVisible(false);
            }
            deckShower.SetActive(false);
            endTurnButton.collider1.enabled = true;
            SurrenderButton.collider1.enabled = true;
            //EnableCollider();
            //            background.color = Color.white;
        }
    }
    public void CloseAllCards(List<CardCode> l)
    {
        for (int i = 0; i < l.Count; i++)
        {
            l[i].code.CardMovement(l[i].transform.position, new Vector3(-3000, 0, 0));
            l[i].SetVisible(false);            //deckShower.SetActive(false);
        }
    }
    public MapCreator MC;
    public SpriteRenderer background;
    public void DisActive()
    {        /*deckShower.SetActive(false);        MapShower.Disabler();*/        //if(dec)
        DisableColliders();
        if (deckShower.activeInHierarchy) { ShowAllCards(); }
        if (MC.SG.sortingOrder == 160)
        {            //HBC.button(buttonNames.map).StartShining(false);
            MC.PopulateMap();
        }
        if (RewardShower.activeInHierarchy)
        {
            SetRewards();
        }
        if (DS.shower.activeInHierarchy)
        {
            DS.SetUp();
        }
        if (ShopShower.gameObject.activeInHierarchy)
        {
            ShopOn();            //print("got shop?");
        }        //if(PRS.CardRewardShower.gameObject.activeInHierarchy)
        if (CC.isVisible())
        {
            ShowTeam();
        }
        if (SCI.IsVisible())
        {
            ShowSoul();
        }
        CleanTempSouls();
        PRS.CardRewardShower.gameObject.SetActive(false);       //else        //{print("no shop?");}
    }
    public void ShowSoul()
    {
        SCI.SetVisible();
    }
    public void ShowTeam()
    {
        CC.SetVisible();
        //CC.SetAllCompanions();

    }
    public void Exit()
    {
        /*#if UNITY_EDITOR
        .isPlaying=false;
        #endif
        */
        Application.Quit();
    }
    public CardMode cardMode;
    public int CardValueUpgradeOrRemove;
    public void ShowAllCards(bool check)
    {
        deckShower.SetActive(true);
        string s;
        switch (actualdeck)
        {
            case shower.deck: s = "Battle Deck"; break;
            case shower.discard: s = "Battle Discard"; break;
            case shower.consume: s = "Battle Consumed"; break;
            case shower.fullDeck: s = "Completed Deck"; break;
            default: s = ""; break;
        }
        print(s);
        Vector3 v = new(0, 0, depth);
        float width = 1920 / widthDivisor;
        float height = 1080 / heightDivisor, h, w;
        h = 1920 / heightoffset;
        w = 1080 / widthoffset;
        int colun = 0, line = 0, limitPerPage = 9 * 4, mod = (page - 1) * limitPerPage;
        //deckShower.SetActive(true);
        endTurnButton.collider1.enabled = false;
        SurrenderButton.collider1.enabled = false;
        PageText.text = "page\n" + page.ToString() + "/" + (((CardSelection().Count - 1) / limitPerPage) + 1);
        Target d = Target.Deck;
        if (CardSelection() == Discard) { d = Target.Discard; }
        if (CardSelection() == consumed) { d = Target.PlayerTeam; }
        if (CardSelection() == Full) { FullDeckShower(DB.saves[saveSlot].deck); }
        for (int i = 0; i < CardSelection().Count; i++)
        {
            CardSelection()[i].code.CardMovement
            (CardSelection()[i].transform.position, PlayAreas[(int)d].transform.position, new Vector3(-3000, 0, 0)); //to do: magic number -3000 only means outside screen???
            CardSelection()[i].SetVisible(false);
        }
        for (int i = 0; i + mod < CardSelection().Count && i < limitPerPage; i++)
        {            //                print(mod+"+"+i);
            line = (int)(i / 9);
            colun = (i) - (9 * line);
            v.x = (colun * width) + w;
            v.y = 1920 - ((line * height) + h);
            CardSelection()[i + mod].code.CardMovement(PlayAreas[(int)d].transform.position, v);
            CardSelection()[i + mod].SetVisible(true);
            //            print("test");
        }        /*}        else        {            for (int i = 0; i < Deck.Count; i++)            {                Deck[i].code.CardMovement(Deck[i].transform.position, new Vector3(-3000, 0, 0));                Deck[i].SetVisible(false);                deckShower.SetActive(false);            }        }*/
        CardCode card;
        int cm = 0;
        if (check) { }
        else
        {
            switch (cardMode)
            {
                case CardMode.Soulable: s = "Soul Upgrade"; cm = 1; break;
                case CardMode.none: break;
                case CardMode.Addable: s = "Card Upgrade"; cm = 2; break;
                case CardMode.Removable: s = "Card Remove"; cm = 3; break;
                case CardMode.Upgradable: s = "Card Upgrade"; cm = 2; break;
                case CardMode.Buyable: s = "buyable\n?????"; break;
                default: break;
            }
            for (int i = 0; i < CardSelection().Count; i++)
            {

                card = CardSelection()[i];
                card.code.enabledDrag = true;
                card.code.CIS.soul.Clear();
                card.code.upgradable = false;
                card.code.removable = false;
                card.code.SetValueVisible(false);
                card.code.ri = RI;
                switch (cm)
                {
                    case 1:
                        card.code.CIS.soul.Add(SCI.GetActualSoul());
                        card.code.Soulable = true;
                        break;
                    case 2://add
                        if (card.code.CIS.info.upgrade == null)
                        { }
                        else
                        {
                            //card.code.CIS.info=card.code.CIS.info.upgrade;
                            card.code.upgradable = true;
                            card.code.SetValue(CardValueUpgradeOrRemove);
                            card.code.SetValueVisible(true);
                        }
                        break;
                    case 3://remove
                        card.code.SetValue(CardValueUpgradeOrRemove);
                        card.code.SetValueVisible(true);
                        card.code.removable = true;
                        break;
                    default: break;
                }
                card.code.CIS.NewInfo();
            }
        }
        selectedText.text = "Viewing: \n" + s;//discard only apearing after next button

    }
    public void CleanTempSouls()
    {
        shower old = actualdeck;
        //cardMode=CardMode.none;
        actualdeck = shower.fullDeck;
        CardCode card;
        for (int i = 0; i < CardSelection().Count; i++)
        {
            card = CardSelection()[i];
            card.code.CIS.soul.Clear();
            //card.code.CIS.soul.Add(SCI.GetActualSoul());                //card.code.CIS.NewInfo();
            //
            card.code.ri = null;
            card.code.enabledDrag = false;
            card.code.Soulable = false;
            card.code.upgradable = false;
            card.code.removable = false;
            card.code.SetValueVisible(false);
            show.SetActive(false);
            showUpgradeInBattle.SetActive(false);
            ShowStatus.gameObject.SetActive(false);
        }
        actualdeck = old;
        //ShowAllCards();
    }
    public bool Draw(int qtd)
    {
        CardCode CardDataTransfer;

        if (qtd < 1)
        {
            if (pool.Count > 0) { CardDataTransfer = pool[0]; pool.RemoveAt(0); print("pool>0"); }
            else { CardDataTransfer = Instantiate(prefab).GetComponent<CardCode>(); print("pool<0"); }
            CardDataTransfer.code.CIS.info = DB.AllCards[15];
            CardDataTransfer.code.CIS.NewInfo();
            CardDataTransfer.code.enabledDrag = true;
            Hand.Add(CardDataTransfer);
        }
        else
        {
            for (int i = 0; i < qtd; i++)
            {
                if (Deck.Count == 0)
                {
                    for (int j = 0; j < Discard.Count; j++)
                    {
                        CardDataTransfer = Discard[j];
                        Deck.Add(CardDataTransfer);
                        CardDataTransfer.code.CardMovement(PlayAreas[(int)Target.Discard].transform.position,
                        PlayAreas[(int)Target.Deck].transform.position, new Vector3(-3000, 0, 0));
                    }
                    Discard.Clear();
                }
                if (Deck.Count > 0)
                {
                    TriggerEffectList(Trigger.Draw);
                    int r = UnityEngine.Random.Range(0, Deck.Count);
                    Deck[r].code.enabledDrag = true;
                    Hand.Add(Deck[r]);
                    Deck.RemoveAt(r);
                }
            }
        }
        updateDeckDraworDiscard();

        return true;
    }

    public TextMeshProUGUI te;
    public void HitWho()
    {

        foreach (Collider2D t in PlayAreas)
        {
            te.text = (t.name + "//" + t.bounds + "//contains? " + t.bounds.Contains(Input.mousePosition) + " p:" + Input.mousePosition);
            print(t.name + "//" + t.bounds + "//contains? " + t.bounds.Contains(Input.mousePosition) + " p:" + Input.mousePosition);

        }

    }
    public PlayAreaType HitWhoTarget()
    {
        Vector3 input;
        foreach (Collider2D t in PlayAreas)
        {
            //te.text = (t.name + "//" + t.bounds + "//contains? " + t.bounds.Contains(Input.mousePosition) + " p:" + Input.mousePosition);
            //print(t.name + "//" + t.bounds + "//contains? " + t.bounds.Contains(Input.mousePosition) + " p:" + Input.mousePosition);
            input = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
            input.z = 0;
            if (t.bounds.Contains(input))
            {
                return t.GetComponent<PlayAreaType>();
            }
        }
        return null;
    }

    public Vector3 Inside()
    {
        Vector3 v = Vector3.zero;
        Vector3 input = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        input.z = 0;
        //        print("mouse: "+input);
        //      print(PlayAreas[0]);
        foreach (Collider2D t in PlayAreas)
        {
            if (t.bounds.Contains(input))
            {
                v = t.bounds.center;
            }
        }
        return v;
    }/*
    public void ()
    {

    }*/
    public void Reset()
    {
        turn = 0; Start(); BattleStop = false;        //to do: set player shield to zero? and mana to max??        //StartTurn();
        endTurnButton.SetInteractable(true);
        SurrenderButton.SetInteractable(true);
        CleanArea();        //ResetMinion(); //solve later
        MinionsToBattlefield();
        StartCoroutine(StartBattle());
    }
    public void CleanArea()
    {
        List<Collider2D> temp = new List<Collider2D>();
        temp.Add(PlayAreas[0]);
        temp.Add(PlayAreas[1]);
        temp.Add(player.BC2d);
        temp.Add(enemy.BC2d);
        PlayAreas.Clear();
        PlayAreas.AddRange(temp);
    }
    public List<UnitControl> minionPool;
    public void ResetMinion()
    {
        for (int i = 2; i < everybody.Count; i++)
        {
            minionPool.Add(everybody[i]);
            everybody[i].transform.position = new Vector3(-3000, 0, 0);
        }
        everybody.Clear();
        everybody.Add(player);
        everybody.Add(enemy);
        player.SC.ResetStatus();
        enemy.SC.ResetStatus();
        //        print("minions resettadis");

    }
    public IEnumerator StartBattle()
    {
        ChangeRelicPreviewPage(true);
        StartCoroutine(TeamStartTurn(Target.PlayerTeam));
        yield return StartCoroutine(TeamStartTurn(Target.EnemyTeam));
        yield return StartCoroutine(TriggerEffectList1(Trigger.StartBattle));
        StartCoroutine(StartTurn());
    }
    public IEnumerator TeamStartTurn(Target team)
    {
        List<UnitControl> teamlist = GetUnitList(team);
        for (int i = 0; i < teamlist.Count; i++)
        {
            yield return StartCoroutine(UnitStartTurn(teamlist[i]));
        }
    }
    public IEnumerator StartTurn()
    {
        if (turn == 0) { } else { yield return StartCoroutine(TeamStartTurn(Target.PlayerTeam)); }
        //StartCoroutine(UnitStartTurn(player));
        turn++;
        yield return StartCoroutine(TriggerEffectList1(Trigger.StartTurn));
        SetEnemyintetion();//fix this sheidat
        RecoverEnergy();
        Draw(basicDraw + (player.SC.Contains(BuffsAndDebuffs.inteligence) / two));
        OrganizeHand();
        endTurnButton.SetInteractable(true); SurrenderButton.SetInteractable(true);
    }

    public void SetEnemyintetion()
    {
        List<UnitControl> allenemy = new List<UnitControl>(), targets = new List<UnitControl>();
        allenemy = GetUnitList(Target.EnemyTeam);
        targets = GetUnitList(Target.PlayerTeam);
        for (int i = 0; allenemy.Count > i; i++)
        {
            UnitAtk(allenemy[i]);
        }
        for (int i = 0; targets.Count > i; i++)
        {
            if (targets[i] == player) { }
            else
            {
                UnitAtk(targets[i]);
            }
        }

    }
    public void RecoverEnergy()
    {
        print(-player.GS(status.maxEnergy).value1(status.maxEnergy, this, player.SC));
        StartCoroutine(DealDamage(-player.GS(status.maxEnergy).value1(status.maxEnergy, this, player.SC), player, status.mana));
    }
    public FakeButton endTurnButton, SurrenderButton;
    public void EndTurn()
    {
        StartCoroutine(EndTurn1());
    }
    public IEnumerator EndTurn1()
    {
        endTurnButton.SetInteractable(false);
        SurrenderButton.SetInteractable(false);
        HandDiscard();        //CheckBattleEnd();
        yield return TriggerEffectList1(Trigger.EndTurn);
        yield return StartCoroutine(UnitEndTurn(player));        //StatusHandler(player,player.SC.GetList(Trigger.EndTurn));
        SummonTurn();
    }

    public IEnumerator StatusHandler(UnitControl u, List<StatusItem> l)
    {
        BuffsAndDebuffsData badd;
        List<effects> e = new();
        List<int> v = new();
        float temp = 0f;
        List<bool> r = new();
        for (int i = 0; i < l.Count; i++)
        {
            badd = l[i].buffs;
            print(badd.identifier);
            switch (badd.identifier)
            {
                case BuffsAndDebuffs.poison:
                    e.Add(effects.direct_penetrate); v.Add(badd.value); r.Add(false);
                    StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));
                    if (badd.value > 0) { u.SC.AddStatus(new BuffsAndDebuffsData(false, -1, badd.identifier, badd.shower, badd.activator, null)); }
                    else { u.SC.RemoveThisStatus(badd.identifier); }
                    e.Clear(); v.Clear(); r.Clear();
                    break;
                case BuffsAndDebuffs.bleed:
                    e.Add(effects.direct_penetrate); v.Add(badd.value); r.Add(false);
                    StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));
                    if (badd.value > 0) { u.SC.AddStatus(new BuffsAndDebuffsData(false, -1, badd.identifier, badd.shower, badd.activator, null)); }
                    else { u.SC.RemoveThisStatus(badd.identifier); }
                    e.Clear(); v.Clear(); r.Clear();
                    break;
                case BuffsAndDebuffs.rage:

                    int rageValue = 4;
                    e.Add(effects.stats_strength); v.Add(rageValue); r.Add(false);
                    e.Add(effects.stats_inteligence); v.Add(-(rageValue / 4)); r.Add(false);
                    if (badd.value > 0)
                    {
                        StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));
                        u.SC.AddStatus(new BuffsAndDebuffsData(false, -1, badd.identifier, badd.shower, badd.activator, null));
                        e.Clear(); v.Clear(); r.Clear();
                    }
                    else
                    {
                        u.SC.RemoveThisStatus(badd.identifier);
                        e.Clear(); v.Clear(); r.Clear();

                    }
                    break;
                case BuffsAndDebuffs.unrage:
                    if (u.SC.Contains(BuffsAndDebuffs.rage) > 0)
                    {
                        print("there is enough rage to continue");
                    }
                    else
                    {
                        e.Add(effects.stats_strength); v.Add(-badd.value * 4); r.Add(false);
                        e.Add(effects.stats_inteligence); v.Add(badd.value); r.Add(false);
                        e.Add(effects.stats_stamina); v.Add(-badd.value); r.Add(false);
                        e.Add(effects.boost_stamina); v.Add(badd.value); r.Add(false);
                        StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));
                        badd.value = 0;
                        print("unrage effects?");
                        u.SC.RemoveThisStatus(badd.identifier);
                        e.Clear(); v.Clear(); r.Clear();
                    }
                    break;/*
                    case BuffsAndDebuffs.boost_strength:
                    e.Add(effects.strength);v.Add((badd.value>0?1:-1));r.Add(false);
                    StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));                        
                    break;*/
                case BuffsAndDebuffs.boost_stamina:
                case BuffsAndDebuffs.boost_strength:
                case BuffsAndDebuffs.boost_dexterity:
                case BuffsAndDebuffs.boost_constitution:
                case BuffsAndDebuffs.boost_inteligence:
                case BuffsAndDebuffs.boost_wisdom:
                case BuffsAndDebuffs.boost_charisma:
                    if (badd.value == 0)
                    {
                        u.SC.RemoveThisStatus(badd.identifier);
                        e.Clear(); v.Clear(); r.Clear();
                    }
                    else
                    {
                        e.Add(GetBoosterType(badd.identifier));
                        v.Add((badd.value > 0 ? 1 : -1)); r.Add(false);
                        StartCoroutine(DmgDealer(u, false, e, v, r, null, u, false, null, null, false));
                        u.SC.AddStatus(new BuffsAndDebuffsData(false, -(badd.value > 0 ? 1 : -1), badd.identifier,
                         badd.shower, badd.activator, null));
                        e.Clear(); v.Clear(); r.Clear();
                    }
                    break;
                default:
                    print("something donts feels right, verify trigger in scriptableobject (stausinfo)");
                    break;
            }
            if (temp == 0) { temp = (l.Count > 4) ? MaxStatusAnimationDuration / l.Count : MinStatusAnimationDuration; }
            print("2/l=" + (temp) + " time:" + Time.time);
            StartCoroutine(u.Blink1(l[i].SR, temp));
            yield return new WaitForSeconds(temp);

        }
        u.SC.RemoveUsed();

    }
    public float MaxStatusAnimationDuration, MinStatusAnimationDuration;
    public effects GetBoosterType(BuffsAndDebuffs e) =>
        //effects badd = e;
        //badd.SetData(UC.M.DB.GetStatusInfo(t));
        e switch
        {
            BuffsAndDebuffs.boost_strength => effects.stats_strength,
            BuffsAndDebuffs.boost_dexterity => effects.stats_dexterity,
            BuffsAndDebuffs.boost_constitution => effects.stats_constitution,
            BuffsAndDebuffs.boost_inteligence => effects.stats_inteligence,
            BuffsAndDebuffs.boost_wisdom => effects.stats_wisdom,
            BuffsAndDebuffs.boost_stamina => effects.stats_stamina,
            BuffsAndDebuffs.boost_charisma => effects.stats_charisma,
            _ => effects.other_none,
        };
    public void HandDiscard()
    {

        for (int i = 0; Hand.Count > i; i++)
        {
            CardCode card = Hand[i];
            card.code.enabledDrag = false;
            if (card.code.CIS.info.efeitos.Contains(effects.card_ADHD))
            {
                //explode card
                consumed.Add(card);
                StartCoroutine(card.code.Move(card.transform.position, new Vector3(1920 / 2, 1080 / 2, 0), new Vector3(3000, 0, 0)));
            }
            else
            {
                Discard.Add(card);
                card.code.CardMovement(card.transform.position,
                PlayAreas[(int)Target.Discard].transform.position, new Vector3(3000, 0, 0));
                //Hand.Remove(card);//RemoveAt(i);
            }
        }
        Hand.Clear();
        updateDeckDraworDiscard();
        OrganizeHand();
    }
    public void SummonTurn()
    {
        ExecuteAllAtk(false);
        //StatusHandler(player,player.SC.GetList(Trigger.EndTurn));

        //EnemyTurn();
    }
    public void test()
    {
        string s = "";
        bool b = false;
        s = b + (b ? 0 : 1).ToString();
        print(s);
        b = true;
        s = b + (b ? 0 : 1).ToString();
        print(s);
    }
    public void EnemyTurn()
    {
        ExecuteAllAtk(true);
        //if (!BattleStop) { CheckBattleEnd(); }
        //if (BattleStop) { } else { StartTurn(); }
    }
    public void ExecuteAllAtk(bool cpu)
    {
        StartCoroutine(ExecuteAllAtkRoutine(cpu));
    }
    public IEnumerator ExecuteAllAtkRoutine(bool cpu)
    {
        List<UnitControl> allenemy = new List<UnitControl>(), targets = new List<UnitControl>();
        Target t1, t2;
        t1 = cpu ? Target.PlayerTeam : Target.EnemyTeam;
        t2 = cpu ? Target.EnemyTeam : Target.PlayerTeam;
        allenemy = GetUnitList(t2); targets = GetUnitList(t1);
        UnitControl unit;        //units
        yield return cpu ? StartCoroutine(TeamStartTurn(Target.EnemyTeam)) : null;
        for (int i = 0; i < allenemy.Count; i++)
        {            //UnitAtk(allenemy[i], GetTarget(targets, allenemy[i].unitTarget));
            if (BattleStop) { i = allenemy.Count + 1; }
            else
            {
                unit = allenemy[i]; if (unit == player) { }
                else
                {                    //yield return StartCoroutine(UnitStartTurn(unit));
                    if (unit.GS(status.health).value1(status.health, this, unit.SC) > 0)
                    {
                        StartCoroutine(unit.Shining(unit.intention, .15f));
                        yield return new WaitForSeconds(.2f);
                        yield return StartCoroutine(ExecuteAtkRoutine(unit, GetTarget(unit.atks[unit.IndexAtk].EnemyTarget
                        ? targets : allenemy, unit.unitTarget)));
                        //ExecuteAtk(unit, GetTarget(unit.atks[unit.IndexAtk].EnemyTarget? targets : allenemy, unit.unitTarget));// sada
                    }
                    yield return StartCoroutine(StatusHandler(unit, unit.SC.GetList(Trigger.PlayAtkCard)));
                    yield return StartCoroutine(UnitEndTurn(unit));
                }
            }    //to do: trigger any atk status dependent  
            if (!BattleStop) { CheckBattleEnd(); }
        }
        if (cpu)
        {
            if (!BattleStop) { CheckBattleEnd(); }
            if (BattleStop) { }
            else
            {
                StartCoroutine(StartTurn());
            }
        }
        else
        {
            EnemyTurn();
        }
    }
    public AnimationManager AM;
    public IEnumerator WaitForAnimation(UnitControl unit)
    {
        while (!(AM.Done))
        { yield return null; }
        AM.Done = false;
    }
    public IEnumerator UnitStartTurn(UnitControl unit)
    {
        if (unit.GS(status.shield).value1(status.shield, this, unit.SC) > 0)
        {
            yield return StartCoroutine(unit.Blink(status.shield, .4f));
            yield return StartCoroutine(unit.DealDamage1(9999, status.shield, .3f));
        }
        yield return StartCoroutine(StatusHandler(unit, unit.SC.GetList(Trigger.StartTurn)));
    }
    public IEnumerator UnitEndTurn(UnitControl unit)
    {
        yield return StartCoroutine(StatusHandler(unit, unit.SC.GetList(Trigger.EndTurn)));
    }
    public void ExecuteAtk(UnitControl unidade, UnitControl targeted)
    {

        /*
        atk atkTest = unidade.atks[unidade.IndexAtk];        // if (atkTest.EnemyTarget) { } else { targeted = unidade; }
        DmgDealer(targeted, false, atkTest.efeito, atkTest.value, atkTest.reverse, null, unidade, false,
        atkTest.minions, atkTest.buff, true);*/
    }
    public IEnumerator ExecuteAtkRoutine(UnitControl unidade, UnitControl targeted)
    {
        atk atkTest = unidade.atks[unidade.IndexAtk];        // if (atkTest.EnemyTarget) { } else { targeted = unidade; }
        AtkAnimationData a = atkTest.anime ?? DB.standard;
        //        print("target=" + targeted.animador.transform.lossyScale + " source=" + unidade.animador.transform.lossyScale + "target/source=" + unidade.DivideVector(targeted.animador.transform.lossyScale, unidade.animador.transform.lossyScale));
        yield return StartCoroutine(unidade.AnimeSolver(a.anime, targeted.transform.position,
        unidade.DivideVector(targeted.animador.transform.lossyScale, unidade.animador.transform.lossyScale)));
        //animated atks from "minion"
        yield return StartCoroutine(DmgDealer(targeted, false, atkTest.efeito, atkTest.value, atkTest.reverse, null, unidade, false,
        atkTest.minions, atkTest.buff, true));
        //yield return new WaitForSeconds(.5f);

    }


    public void UnitAtk(UnitControl unidade)
    {
        int atk = 0;
        if (unidade.GS(status.health).value1(status.health, this, unidade.SC) > 0)
        {
            switch (unidade.behaviour)
            {
                case AI.Random:
                    int r = UnityEngine.Random.Range(0, unidade.atks.Count);
                    unidade.IndexAtk = r;
                    unidade.SetIntention(intentions.random, "?");
                    break;
                case AI.Incremental:
                    atk = unidade.IndexAtk + 1;
                    if (atk < unidade.atks.Count) { } else { atk = 0; }
                    unidade.IndexAtk = atk;
                    unidade.SetIntention(unidade.atks[atk]);
                    break;
                case AI.Decremental:
                    atk = unidade.IndexAtk - 1;
                    if (atk < 0)
                    {
                        atk = unidade.atks.Count - 1;
                    }
                    unidade.IndexAtk = atk;
                    unidade.SetIntention(unidade.atks[atk]);

                    //atk atkTest = unidade.atks[r];
                    //DmgDealer(targeted, false, atkTest.efeito, atkTest.value, atkTest.reverse, null, unidade);

                    //DealDamage(atkTest.value,targeted,atkTest.efeito);
                    //print("deal damage to player?");
                    break;
                default: break;
            }
        }
        else
        {
            unidade.SetIntention(intentions.dead, "");
        }
    }
    public UnitControl GetTarget(List<UnitControl> AllUnits, AI_Target system)
    {
        int index = 0;
        List<UnitControl> AllUnits1 = new List<UnitControl>();
        UnitControl prota = player;
        for (int i = 0; i < AllUnits.Count; i++)
        {
            if (AllUnits[i] == enemy) { prota = enemy; }
            if (AllUnits[i] == player) { prota = player; }
            if (AllUnits[i].GS(status.health).value1(status.health, this, AllUnits[i].SC) > 0)
            { AllUnits1.Add(AllUnits[i]); }
        }
        switch (system)
        {
            case AI_Target.Random: index = UnityEngine.Random.Range(0, AllUnits1.Count); break;
            case AI_Target.Summon: AllUnits1.Remove(prota); index = UnityEngine.Random.Range(0, AllUnits1.Count); break;
            case AI_Target.Protagonist: return prota; //break;
            case AI_Target.Front: break;
            case AI_Target.Back: break;
            /*/case AI_Target.:index=UnityEngine.Random.Range(0, AllUnits.Count);;break;//*/
            default: index = 0; break;
        }
        if (AllUnits1.Count > 0)
        { return AllUnits1[index]; }
        else
        {
            return prota;
        }
    }
    public List<UnitControl> GetUnitList(Target alvo)
    {
        List<UnitControl> unidades = new List<UnitControl>();
        foreach (UnitControl unit in everybody)
        {
            if (unit.areaType.type == alvo)
            {
                unidades.Add(unit);
            }
        }
        return unidades;
    }
    public void EndBattle()
    {
        TriggerEffectList(Trigger.EndBattle);
        endTurnButton.SetInteractable(false);//to do: there is bug sometimes locking this button discover why
        SurrenderButton.SetInteractable(false);//to do: there is bug sometimes locking this button discover why
        BattleStop = true;
        /*for (int i = 0;i<Hand.Count;i++)
        {
            DiscardCard(Hand[i]);
        }*/
        //Deck=Deck+Hand;
        //ClearCards();
    }
    public List<CardCode> pool;
    public void ClearCards()
    {
        if (Deck.Count > 0) { foreach (CardCode c in Deck) { pool.Add(c);/*Destroy(c.gameObject);*/ } }
        if (Hand.Count > 0) { foreach (CardCode c in Hand) { pool.Add(c);/*Destroy(c.gameObject);*/ } print("hand explode?"); }
        if (consumed.Count > 0) { foreach (CardCode c in consumed) { pool.Add(c);/*Destroy(c.gameObject);*/ } }
        if (Discard.Count > 0) { foreach (CardCode c in Discard) { pool.Add(c);/*Destroy(c.gameObject);*/ } }
        //if (Full.Count > 0) { foreach (CardCode c in Full) { pool.Add(c);/*Destroy(c.gameObject);*/ } }
        Deck.Clear(); Hand.Clear(); consumed.Clear(); Discard.Clear();//Full.Clear();
        PoolCleaner();
    }
    public void PoolCleaner()
    {
        if (pool.Count > 0)
        {
            Vector3 v = pool[0].transform.position; v.x = -3000; for (int i = 0; i < pool.Count; i++)
            {
                pool[i].transform.position = v;
                //print("card[" + i + "]=" + v.x);
            }
        }
    }
    public const int density = -1;
    public const int left = -1, right = 1, one = 1, two = 2;
    public GameObject handPosition;
    public int divideroffset, xoffset, maximumHand;

    public void OrganizeHand()
    {
        if (Hand.Count == 0) { print("no hand to organize"); }
        else
        {
            int handSize = Hand.Count;
            Vector3 old = Vector3.zero;
            float height = handPosition.transform.position.y;
            float horizontalCenter = handPosition.transform.position.x;            //float resolutionMod=(2.5f/1920)*Screen.width;
            divideroffset = math.clamp((int)(handSize / 2.5f), 3, 999);//to do: magic numbers fix it            // Calculate the offset for each card                 0-10                        10-999
            int offset = 1920 / ((handSize > 10) ? handSize + divideroffset : maximumHand + divideroffset);
            float totalWidth = (handSize - 1) * offset;            // Start position for the first card to center the entire hand
            float startPosition = horizontalCenter - totalWidth / 2;
            for (int index = 0; index < handSize; index++)
            {                //                print("offset=" + offset);
                float horizontalOffset = startPosition + (offset * index) - xoffset;                // Adding density calculations back
                float densityAdjustment = ((float)index / Mathf.Clamp(handSize, 1, 23) - density);                //print((float)index +"/"+handSize + "-"+ "0.5f"+")" +"-"+ density+"="+densityAdjustment);                // Adjusts density based on card index

                Vector3 newPosition = new Vector3(horizontalOffset, height, densityAdjustment);                // Using density for z-coordinate
                old = Hand[index].transform.position;
                if (old.x < -2900) { old = PlayAreas[0].transform.position; }//to do: real magic number aprouch with caution
                Hand[index].code.CardMovement(old, newPosition);
            }
        }
    }
    public void Install(DeckData qtd)
    {
        UpdateAllCards();
        allCards.Clear();
        for (int i = 0; i < qtd.all.Count; i++)
        {
            CardCode card;
            card = CreateCardInBattle();
            allCards.Add(card);
            /*if (pool.Count == 0) { g = Instantiate(prefab); card = g.GetComponent<CardCode>(); allCards.Add(card); }
            else { card = pool[0]; pool.Remove(card); allCards.Add(card); }            */
            Deck.Add(card); card.code.CIS.info = qtd.all[i]; card.code.CIS.NewInfo();
        }
        FullDeckShower(qtd);
        updateDeckDraworDiscard();        //everybody = new List<UnitControl>();
        Vector3 v = unitPrefab.transform.position;
        if (player == null)
        {
            player = Instantiate(unitPrefab); player.SetAreaType(Target.PlayerTeam);
            player.gameObject.SetActive(true); player.SetTeamIcon();
            PlayAreas.Add(player.BC2d);
            player.transform.position = new Vector3(1920 - v.x, v.y, v.z);
            player.PlayerHealth = HBC.allbuttons[(int)buttonNames.health].textButton;
            player.SetData(DB.saves[saveSlot].player);
        }
        if (enemy == null)
        {
            enemy = Instantiate(unitPrefab); enemy.SetAreaType(Target.EnemyTeam);
            PlayAreas.Add(enemy.BC2d); enemy.gameObject.SetActive(true);
            enemy.SetTeamIcon();            // enemy.transform.position = new Vector3(1920 - v.x, v.y, v.z);            //player.PlayerHealth = HBC.allbuttons[(int)buttonNames.health].textButton;            //player.SetData(DB.saves[saveSlot].player);
        }
        MinionsToBattlefield();
        player.UpdateAtri();
        enemy.UpdateAtri();
        CreateList();
        SetUpRelic();
        PoolCleaner();
    }
    // public List<UnitData> AllMinions = new List<UnitData>();
    //public List<MinionData> AllMinions = new List<MinionData>();

    public void MinionsToBattlefield()
    {
        ResetMinion();
        List<MinionData> AllMinions = new List<MinionData>();
        int p = player.UD.minions.Count, e = enemy.UD.minions.Count;
        MinionData md = new MinionData(null, Target.PlayerTeam);
        UnitData ud;
        for (int i = 0; i < p || i < e; i++)
        {
            if (p > i) { ud = player.UD.minions[i]; md = new MinionData(ud, Target.PlayerTeam); AllMinions.Add(md); print("add player minon"); }
            if (e > i) { ud = enemy.UD.minions[i]; md = new MinionData(ud, Target.EnemyTeam); AllMinions.Add(md); print("add enemy minon"); }
        }
        for (int i = 0; everybody.Count < 20 && i < AllMinions.Count; i++)// to do: magic number solve 18=minion limit
        {
            StartCoroutine(SetMinions(AllMinions[i].team, AllMinions[i].ud));
        }

    }
    public int minionWidth = 1, minionHeigth = 1;// minionCounter;
    public Vector2 Offset;
    /*public void SetMinions()
    {
        int r = UnityEngine.Random.Range(2, 4);
        SetMinions((Target)r, player.UD);
    }*/
    public IEnumerator SetMinions(Target t, UnitData ud)
    {
        Vector3 v = enemy.transform.position, vScale;
        Vector2 position = new Vector2(0, 0);
        //if (minionCounter < 18)
        //{
        UnitControl g;
        if (minionPool.Count == 0)
        {
            g = Instantiate(unitPrefab); vScale = g.transform.localScale / two;//magic number 2=half;
            g.SC.SetWidthHeight(two);
        }
        else { g = minionPool[0]; minionPool.Remove(g); vScale = g.transform.localScale; }            //if( PlayAreas.Contains()){}
        g.gameObject.SetActive(true);
        //StartCoroutine(g.AnimeSolver(DB.standardSummon.anime,Vector3.zero,Vector3.one));
        PlayAreas.Add(g.BC2d);            //v=g.transform.position;
        g.transform.position = new Vector3(v.x + (position.x * minionWidth), v.y + (position.y * minionHeigth), v.z);
        g.transform.localScale = vScale;
        everybody.Add(g);
        g.areaType.type = t;
        g.SetData(ud);
        g.UpdateAtri();
        g.SetTeamIcon();            //minionCounter++;
        g.SC.ResetStatus();
        g.SetIntention();
        print(g.UD.name);

        //}
        int z = 0, d = 0;
        for (int i = 2; i < everybody.Count; i++)
        {
            z = i % 2 + 1; d = (i + 2) / 2 - 1; position = new Vector2(d, z);
            everybody[i].transform.position = new Vector3(v.x + Offset.x - (position.x * minionWidth),
             v.y - Offset.y + (position.y * minionHeigth), v.z);
        }
        yield return g.StartCoroutine(g.AnimeSolver(DB.standardSummon.anime, Vector3.zero, new Vector3(.6f, .6f, 2)));

    }
    public int saveSlot = 0;
    public float relicWidth = 50;
    public int relicPage = 0;
    public List<RelicShower> relics2;
    public TextMeshPro RelicTextPage;
    public void ChangeRelicPreviewPage(bool increment)
    {
        int i = increment ? (relicPage + 1) : (relicPage - 1);
        if ((i - 1) > relics.Count / 23)
        {
            i = 1;
        }
        if (i < 1)
        {
            i = (relics.Count / 23) + 1;
        }
        relicPage = i;
        RelicTextPage.text = relicPage + "/" + (1 + (relics.Count / 23));
        ShowAllRelics();
        /*if (i >= Enum.GetValues(typeof(shower)).Length)
        { i = 0; }
        actualdeck = (shower)i;
        page=1;*/

    }
    public void ShowAllRelics()
    {
        //todo ability to disable relic?
        Vector3 v = Vector3.zero;
        for (int i = 0; i < relics2.Count; i++)
        {
            if (i < (24 * (relicPage - 1)) || (i > (relicPage * 23)))
            {
                v = relics2[i].transform.position;
                v.x = -3000;
                relics2[i].transform.position = v;
            }
            else
            {
                v = relics2[i].transform.position;
                v.x = relicPrefab.transform.position.x + relicWidth * (i - ((relicPage - 1) * 24));
                relics2[i].transform.position = v;
            }
        }
    }

    public void AddReward(Rewards R)
    {
        print(R.type);
        switch (R.type)
        {
            case RewardType.noReward:  /*todo*/break;
            case RewardType.Money: AddGold(R.value); break;
            case RewardType.Relic: AddRelic(R.item); break;
            case RewardType.Card: AddCard(R.card); break;
            case RewardType.MultipleCards: /*/ShowOptions(R.pool, R.many);//PRS.ShowCards(R.pool,);*/break;
            case RewardType.Heal: StartCoroutine(DealDamage(PercentHealth(R.chance), player, status.cure)); break;
            case RewardType.MaxHP: StartCoroutine(DealDamage(R.value, player, status.maxHealth)); break;
            case RewardType.Minion: CreateMinion(R.enemy); break;
            case RewardType.Soul: AddSoul(R.soul); break;            //case RewardType.Soul: AddSoul(r.soul)               break;
            case RewardType.Scene: DS.SetScene(R.DD); DS.UpdateData(); SetDialogue(); break;
            case RewardType.DungeonReroll: MC.RedoDungeon(); break;
            case RewardType.UpgradeCard: ShowSelection(CardMode.Upgradable, 0); break;
            case RewardType.RemoveCard: ShowSelection(CardMode.Removable, 0); break;
            case RewardType.Battle:
                if (R.enemy != enemy.UD) { enemy.SetData(R.enemy); Reset(); }
                allButtons.SolveCast(classes.CloseAllTabs); HBC.button(buttonNames.battle).StartShining(true);
                HBC.button(buttonNames.reward).StartShining(false); break;
            case RewardType.DeckChange:
                ChangeDeck(R.deck);
                break;
            case RewardType.SaveChange:
                SaveChange(R.profession);
                break;
            default: Debug.Log("shit is happening on AddReward system"); break;
        }
    }
    public void SaveChange(SaveData s)
    {
        DB.saves[0] = s;
        InstanciateSave();
    }
    public void ShowSelection(CardMode cm, int money)
    {
        CardValueUpgradeOrRemove = money;
        cardMode = cm;
        actualdeck = shower.fullDeck;
        DisActive();
        ShowAllCards(false);
    }
    public RewardItem RI;
    public void SetItemDes(RewardItem item)
    {
        if (RI == null) { RI = item; }
    }
    public int PercentHealth(float f)
    {
        f = (f * player.GS(status.maxHealth).value1(status.maxHealth, this, player.SC));
        print(f);
        return (int)Math.Round(f / 100);
    }
    public void AddSoul(SoulData soul)
    {
        GetSave().souls.Add(soul);
    }
    public void AddCard(CardData c)
    {
        DB.saves[saveSlot].deck.all.Add(c);
    }
    public void ChangeDeck(DeckData DD)
    {
        DB.saves[saveSlot].deck.all.Clear();
        DB.saves[saveSlot].deck.all.AddRange(DD.all);
    }
    public void RemoveCard(CardData c)
    {
        DB.saves[saveSlot].deck.all.Remove(c);
    }
    public void RemoveRelic(int r)
    {
        DB.saves[saveSlot].items.RemoveAt(r);
    }
    public void ChangeRelic(int r, ItemData item)
    {
        if (item.soul == null)
        {
            ItemData i = ItemData.CreateInstance<ItemData>();
            i.Init(item);
            i.soul = SCI.GetActualSoul();
            DB.saves[saveSlot].items[r] = i;
            SCI.RemoveSoul();
            SetUpRelic();
        }
        else
        {
            print("already ocuppied");
        }
    }
    public void UpgradeCard(CardData New, CardData old)
    {
        if (New == null) { } else { DB.saves[saveSlot].deck.all.Add(New); }
        if (old == null) { } else { DB.saves[saveSlot].deck.all.Remove(old); }
    }
    public int upgradesRemaining=0;
    public void UpgradeHandInBattle(DragAndDrop code)
    {
        code.CIS.info=code.CIS.info.upgrade;
        code.CIS.NewInfo();
        upgradesRemaining--;
        if(Hand.Count(h=>h.code.CIS.info.upgrade!=null)>0){}
        else{upgradesRemaining=0;}
        if(upgradesRemaining<1)
        {
            foreach(CardCode c in Hand)
            {
                c.code.enabledDrag=true;
                c.code.upgradable=false;
                
            }
        }
        else
        {
            code.upgradable=(!(code.CIS.info.upgrade==null));
        }
        showUpgradeInBattle.SetActive(false);
        
        //code.upgradable = false;
    }
    public int gold;
    public TextMeshPro moneyText;
    public FakeButton moneySprite, allButtons;
    public void AddGold(int value)
    {
        gold += value;
        updateMoney();
    }
    public void updateMoney()
    {
        //StartCoroutine(Shining(moneyText.GetComponentInParent<SpriteRenderer>()));
        moneySprite.SetWork(true);
        moneyText.text = gold.ToString();
    }
    public void AddRelic(ItemData i)
    {
        DB.saves[saveSlot].items.Add(i);
        SetUpRelic();
    }
    public void SetUpRelic()
    {//todo clean relics?        
        relics = DB.saves[saveSlot].items;
        RelicShower r;
        GameObject g;
        Vector3 v;
        int mod = 1;
        Power p = new Power();
        for (int i = 0; relics.Count > relics2.Count; i++)
        {
            g = Instantiate(relicPrefab.gameObject);
            r = g.GetComponent<RelicShower>();
            relics2.Add(r);
        }
        for (int i = 0; i < relics2.Count; i++)
        {
            r = relics2[i]; //g.GetComponent<RelicShower>();
            v = relicPrefab.transform.position;
            mod = (i <= 23) ? 1 : -1;
            //            print("v.x=" + v.x + " + (relicWidth=" + relicWidth + " * i=" + i + "))*mod=" + mod);
            v.x = (v.x + (relicWidth * i)) * mod;
            //          print(v.x);
            //v.x = v.x * mod;
            r.transform.position = v;
            p = GetDataFromRelic(relics[i], r);
            triggers[(int)relics[i].activation].items.Add(p);
        }

    }
    public Power GetDataFromRelic(ItemData relic, RelicShower r)
    {
        Power p = new Power();
        p.efeito = new List<effects>(relic.efeito);
        p.value = new List<int>(relic.value);
        p.times = relic.times;
        p.font = source.item;
        p.fontcode = relic.cod;
        p.reverse = new List<bool>(relic.reverse);
        p.minions = new List<UnitData>(relic.minions);
        p.buff = new List<BuffsAndDebuffsData>(relic.buff);
        if (relic.soul != null)
        {
            p.efeito.AddRange(relic.soul.RelicEfeitos);
            p.value.AddRange(relic.soul.valueRelic);
            p.reverse.AddRange(relic.soul.reverseRelic);
            p.minions.AddRange(relic.soul.unidades);
            p.buff.AddRange(relic.soul.buff);
        }
        p.anime = relic.anime;
        p.focus = relic.target;
        p.Alvo = relic.alvo;
        p.shower = r;
        p.shower.item = relic;
        p.shower.SetArt();
        return p;
    }
    //public bool triggered=true;
    public void CreateList()
    {
        triggers.Clear();
        string[] s = Enum.GetNames(typeof(Trigger));
        //char[][] s=Enum.GetNames(typeof(Trigger)).CopyTo(s,);        //int b =triggers.Count;        //Array what=Enum.GetValues(typeof(Trigger));
        for (int b = 0; b < s.Length; b++)
        {
            triggers.Add(new AllLists(s[b], new List<Power>(), (Trigger)b));
        }
    }
    // public string[] enumshow;
    public TextMeshPro deckCount, HandCount, DiscardCount;
    public void updateDeckDraworDiscard()
    {
        deckCount.text = Deck.Count.ToString();
        HandCount.text = Hand.Count.ToString();
        DiscardCount.text = Discard.Count.ToString();
    }
    public IEnumerator DiscardCard(CardCode card)
    {
        //CardData c = card.code.CIS.info;//add soul list

        List<effects> efeitos = new List<effects>();
        efeitos.AddRange(card.code.CIS.info.efeitos);
        List<int> values = new List<int>();
        values.AddRange(card.code.CIS.info.values);
        List<bool> reverses = new List<bool>();
        reverses.AddRange(card.code.CIS.info.reverse);
        List<UnitData> minion = new(card.code.CIS.info.minions);
        List<BuffsAndDebuffsData> buff = new(card.code.CIS.info.buff);
        for (int i = 0; i < card.code.CIS.info.souls.Count; i++)
        {
            efeitos.AddRange(card.code.CIS.info.souls[i].CardEfeitos);
            values.AddRange(card.code.CIS.info.souls[i].valueCard);
            reverses.AddRange(card.code.CIS.info.souls[i].reverse);
            minion.AddRange(card.code.CIS.info.souls[i].unidades);
            buff.AddRange(card.code.CIS.info.souls[i].buff);
        }
        //card.transform.position = new Vector3(3000, 0, 0);
        yield return card.code.Move(card.transform.position, PlayAreas[(int)Target.Discard].transform.position, new Vector3(3000, 0, 0));

        if (efeitos.Contains(effects.triggers_discardTrigger))
        {
            List<int> v = new List<int>(); List<effects> e = new List<effects>(); List<bool> r = new List<bool>();
            for (int i = 0; i < efeitos.Count; i++)
            {
                if (efeitos[i] == effects.triggers_discardTrigger)
                {
                    e.Add(efeitos[i + 1]);
                    v.Add(values[i + 1]);
                    r.Add(reverses[i + 1]);
                }
            }
            //card discarded
            StartCoroutine(DmgDealer(player, true, e, v, r, card, player, true, minion, buff, true));
        }
        TriggerEffectList(Trigger.Discard);
        int index = Hand.IndexOf(card);
        Hand[index].code.enabledDrag = false;
        Discard.Add(Hand[index]);
        Hand.RemoveAt(index);
        updateDeckDraworDiscard();
        OrganizeHand();
    }
    public IEnumerator PlayCard(CardCode card, UnitControl unidade, bool repeatable, UnitControl source)
    {
        yield return StartCoroutine(NormalCard(card, unidade, repeatable, source));
        if (repeatable)
        {
            if (!BattleStop)
            {
                CheckBattleEnd();
            }
        }
    }
    public void triggedButton(int i)
    {
        TriggerEffectList((Trigger)i);
    }
    public void TriggerEffectList(Trigger activator)
    {
        StartCoroutine(TriggerEffectList1(activator));
        //CheckBattleEnd();
    }
    public IEnumerator TriggerEffectList1(Trigger activator)
    {
        List<Power> p = triggers[(int)activator].items;
        UnitControl uc;
        Vector3 division;
        for (int i = 0; i < p.Count; i++)
        {
            StartCoroutine(p[i].shower.Blink());
            uc = GetTarget(GetUnitList((p[i].focus)), p[i].Alvo);
            division = uc.DivideVector(uc.animador.transform.lossyScale, p[i].shower.animeSprite.transform.lossyScale);
            yield return StartCoroutine(p[i].shower.AnimeSolver((p[i].anime ?? DB.standard).anime, uc.transform.position,
             division, uc));
            print(" relic trigged target=" + uc.animador.transform.lossyScale + "/relic=" + p[i].shower.animeSprite.transform.lossyScale + " result=" + division);
            //p[i].shower.CallAnimation()
            //relic triggered
            StartCoroutine(DmgDealer(uc, false, p[i].efeito, p[i].value, p[i].reverse, null, player, false, p[i].minions, p[i].buff, false));
        }
    }
    public IEnumerator NormalCard(CardCode card, UnitControl unidade, bool repeatable, UnitControl source)
    {
        //bool reverse = false;
        int index = Hand.IndexOf(card);
        int cost = card.code.CIS.info.custo, actualmana = source.GS(status.mana).value1(status.mana, this, source.SC);
        if (cost <= actualmana || !repeatable)
        {
            if (repeatable)
            {
                StartCoroutine(source.DealDamage1(cost, status.mana, 1));
                switch (card.code.CIS.info.type)
                {
                    case cardtype.combat:
                        TriggerEffectList(Trigger.PlayAtkCard);
                        TriggerEffectList(Trigger.PlayCard);
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayAtkCard)));
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayCard)));
                        break;
                    case cardtype.defense:
                        //todo
                        TriggerEffectList(Trigger.PlayDefensiveCard);
                        TriggerEffectList(Trigger.PlayCard);
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayDefensiveCard)));
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayCard)));
                        //reverse = true;
                        break;
                    case cardtype.utility:
                        TriggerEffectList(Trigger.PlayUtilityCard);
                        TriggerEffectList(Trigger.PlayCard);
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayUtilityCard)));
                        yield return StartCoroutine(StatusHandler(player, player.SC.GetList(Trigger.PlayCard)));
                        break;
                    default:
                        break;
                }

            }
            List<effects> efeitos = new List<effects>();
            efeitos.AddRange(card.code.CIS.info.efeitos);
            List<int> values = new List<int>();
            values.AddRange(card.code.CIS.info.values);
            List<bool> reverses = new List<bool>();
            reverses.AddRange(card.code.CIS.info.reverse);
            List<UnitData> minion = card.code.CIS.info.minions;
            List<BuffsAndDebuffsData> buff = card.code.CIS.info.buff;
            for (int i = 0; i < card.code.CIS.info.souls.Count; i++)
            {
                efeitos.AddRange(card.code.CIS.info.souls[i].CardEfeitos);
                values.AddRange(card.code.CIS.info.souls[i].valueCard);
                reverses.AddRange(card.code.CIS.info.souls[i].reverse);
            }
            if (repeatable)
            {
                switch (card.code.CIS.info.reusability)
                {
                    case cardtype2.normal:
                        Discard.Add(Hand[index]);
                        yield return StartCoroutine(card.code.Move(card.transform.position, unidade.transform.position, new Vector3(3000, 0, 0)));
                        Hand.RemoveAt(index);
                        updateDeckDraworDiscard();
                        OrganizeHand();
                        break;
                    case cardtype2.exaust:
                        consumed.Add(Hand[index]);
                        yield return StartCoroutine(card.code.Move(card.transform.position, new Vector3(1920 / 2, 1080 / 2, 0), new Vector3(3000, 0, 0)));
                        Hand.RemoveAt(index);
                        updateDeckDraworDiscard();
                        OrganizeHand();
                        break;
                    case cardtype2.consum:
                        pool.Add(Hand[index]);
                        DB.saves[saveSlot].deck.all.Remove(Hand[index].code.CIS.info);
                        yield return StartCoroutine(card.code.Move(card.transform.position, new Vector3(1920 / 2, 1080 / 2, 0), new Vector3(3000, 0, 0)));
                        Hand.RemoveAt(index);
                        updateDeckDraworDiscard();
                        OrganizeHand();
                        break;
                    default:
                        print("pretty sure this should not happen....");
                        break;
                }
            }
            yield return StartCoroutine(source.AnimeSolver((card.code.CIS.info.anime ?? DB.standard).anime, unidade.transform.position,
            unidade.DivideVector(unidade.animador.transform.lossyScale, source.animador.transform.lossyScale)));
            //card play
            StartCoroutine(DmgDealer(unidade, repeatable, efeitos, values, reverses, card, source, false, minion, buff, true));

        }
        else
        {
            StartCoroutine(source.DealDamage1(0, status.mana, 1));
        }
    }
    public IEnumerator DmgDealer(UnitControl unidade, bool repeatable, List<effects> efeitos, List<int> values, List<bool> reverse,
     CardCode card, UnitControl source, bool discarded, List<UnitData> minion, List<BuffsAndDebuffsData> buffs,
     bool StatusModifier)
    {
        string test = "source= ";
        test = test + card + source;
        //print(source.name+"/"+card.name+"/");
        int max = efeitos.Count;
        int reversed = 0;
        int mod = 0;
        UnitControl modSource = source;
        for (int i = 0; i < max; i++)
        {
            if (StatusModifier)
            {
                mod = modSource.SC.Contains(DB.Mod(efeitos[i]));
            }
            reversed = reverse[i] ? -values[i] - mod : values[i] + mod;            //print("before?="+reversed);
                                                                                   //            print(test + efeitos[i] + " " + values[i] + "=" + reversed);

            switch (efeitos[i])
            {
                case effects.direct_dmg:
                    StartCoroutine(DealDamage(reversed, unidade, status.health));
                    break;
                case effects.direct_shield://reversed=reversed+source.SC.Contains(BuffsAndDebuffs.dexterity);
                    StartCoroutine(DealDamage(reversed, unidade, status.shield));
                    break;
                case effects.card_repeat:
                    if (repeatable)
                    {
                        for (int o = 0; o < (values[i] - 1); o++)
                        {
                            StartCoroutine(PlayCard(card, unidade, false, source));
                            ///modify?
                        }
                    }
                    else
                    {
                        //print("repeat is not working?");
                    }
                    break;
                case effects.direct_heal:
                    StartCoroutine(DealDamage(-reversed, unidade, status.cure));
                    break;
                case effects.triggers_bloodTrigger:
                    if (unidade.GS(status.health).value1(status.health, this, unidade.SC) >= reversed)
                    {
                        StartCoroutine(DealDamage(reversed, unidade, status.penetrate));
                    }
                    else
                    { i++; }
                    break;
                case effects.minion_summonMinion:
                    if (reversed > minion.Count) { reversed = minion.Count - 1; }
                    yield return StartCoroutine(SetMinions(unidade.areaType.type, minion[reversed]));
                    break;
                case effects.triggers_discardTrigger:
                    i++;                        //if(discarded)                        {}                        else                        {i++;}
                    break;
                case effects.triggers_deadTrigger:
                    if (unidade.GS(status.health).value1(status.health, this, unidade.SC) < 0) { print("stop he is already dead"); }
                    else { i++; print("spell failed because he is alive look health=" + unidade.GS(status.health).value1(status.health, this, unidade.SC)); }
                    break;
                case effects.minion_createMinion:
                    UnitData creation;//=new UnitData(card.code.CIS.info.minions[reversed]);
                    creation = UnitData.CreateInstance<UnitData>();
                    if (reversed > minion.Count) { reversed = minion.Count - 1; }
                    creation.initUnitData(minion[reversed]);//card.code.CIS.info.minions[reversed]);
                    if (i > 0) { if (efeitos[i - 1] == effects.triggers_deadTrigger) { creation.initUnitData(unidade.UD); creation.nome += "(Shadow Clone)"; } }
                    creation.atributes[(int)status.maxHealth].value = unidade.GS(status.health).value1(status.health, this, unidade.SC) * values[i - 1] / 100;
                    creation.atributes[(int)status.health].value = unidade.GS(status.health).value1(status.health, this, unidade.SC) * values[i - 1] / 100;                //creation.createMinion(creation.name,creation.description,creation.cod,creation.art,creation.atks,creation.aI                ,creation.target,creation.atributes,creation.level,creation.AllRewards,creation.minions);
                    //creation.nome+="(Skull)";                                    //print("minoon created");
                    CreateMinion(creation);
                    yield return StartCoroutine(SetMinions(source.areaType.type, creation));
                    break;
                case effects.other_buffOrDebuff://remove this but see dependecies first
                    //unidade.SC.AddStatus(buffs[reversed]);
                    break;
                case effects.stats_strength:
                case effects.stats_dexterity:
                case effects.stats_constitution:
                case effects.stats_inteligence:
                case effects.stats_wisdom:
                case effects.stats_charisma:
                case effects.stats_stamina:
                case effects.direct_poison:
                case effects.direct_bleed:
                case effects.condition_Rage:
                case effects.condition_Unrage:
                case effects.boost_strength:
                case effects.boost_dexterity:
                case effects.boost_constitution:
                case effects.boost_inteligence:
                case effects.boost_wisdom:
                case effects.boost_luck:
                case effects.boost_stamina:
                case effects.boost_charisma:
                    //always remeber to add status to DB                    
                    unidade.SC.AddStatus(unidade.SC.StatusFromEffect(efeitos[i], reversed, true));
                    //                    print(string.Join(',', efeitos) + " i=" + i);
                    break;
                case effects.card_cost:
                    int b = source.GS(status.mana).value1(status.mana, this, source.SC);
                    values[i + 1] = b;
                    //efeitos.Add(effects.repeat);values.Add(b);reverse.Add(false);
                    //max=efeitos.Count;
                    // _ = DealDamage(b, source, status.mana);
                    //print(source);
                    //if(max>16)             {max=0;print("infinite loop");}//anti infinite loop bullshit
                    StartCoroutine(source.DealDamage1(b, status.mana, .8f));
                    break;
                case effects.direct_penetrate:
                    StartCoroutine(DealDamage(reversed, unidade, status.penetrate));
                    break;
                case effects.card_RandomFromDeck:
                    int r, r1;
                    for (int i1 = 0; i1 < reversed; i1++)
                    {
                        r = UnityEngine.Random.Range(0, Full.Count);
                        r1 = UnityEngine.Random.Range(0, everybody.Count);
                        unidade = everybody[r1];
                        StartCoroutine(PlayCard(Full[r], unidade, false, source));
                    }
                    break;
                case effects.card_ADHD:
                    break;
                case effects.card_upgrade://todo later need design
                    int j=0;
                    //h=>h.code.CIS.info.upgrade!=null
                    j=Hand.Count(h=>h.code.CIS.info.upgrade!=null);///fix this j is getting the full list value
                    print("j=="+j);
                    upgradesRemaining=0;
                    foreach (CardCode c in Hand)
                    {
                        
                        if(c.code.CIS.info.upgrade==null)
                        {
                            //print("no fuking card to upgrade");
                        }
                        else{c.code.upgradable = true;}//add glow animation for cards? so easy identify whigh one can upgrade
                        if(j>0){
                        c.code.enabledDrag=false;
                        upgradesRemaining=reversed;
                    }
                    }
                    
                    print("upgrade triggered");
                    break;
                case effects.card_upgradeAtRandom:
                    print("upgrade at hand random triggered");
                    List<CardCode> targets = new();
                    foreach (CardCode c in Hand)
                    {
                        if (c.code.CIS.info.upgrade != null)
                        {
                            targets.Add(c);
                        }
                    }
                    for (int i3=0;i3 <reversed && targets.Count > 0; i3++)
                    {
                        
                            int rand = UnityEngine.Random.Range(0, targets.Count);
                            targets[rand].code.CIS.info = targets[rand].code.CIS.info.upgrade;
                            //if animation put it here
                            targets[rand].code.CIS.NewInfo();
                            targets.RemoveAt(rand);
                        
                    }
                    print(string.Join(",", Hand.Select(s => s.code.CIS.info.name)));
                    print(string.Join(",", targets.Select(s => s.code.CIS.info.name)));
                    break;

                case effects.card_create:
                    //   dasdas
                    //GameObject g; 
                    CardCode card1 = CreateCardInBattle();
                    allCards.Add(card1);
                    //Deck.Add(card);
                    Hand.Add(card1);
                    card1.code.CIS.info = DB.AllCards[reversed]; card1.code.CIS.NewInfo();
                    OrganizeHand();
                    break;
                default:
                    print("value with no info in card");
                    break;
            }
        }
        //        print(string.Join(',',efeitos));//maybe its working 
        if (card == null)
        { }
        else { print(card.code.CIS.info.name + "// hit :" + unidade.UD.nome); }
        yield return null;
    }
    public CardCode CreateCardInBattle()
    {
        GameObject g;
        CardCode card1;

        if (pool.Count == 0) { g = Instantiate(prefab); card1 = g.GetComponent<CardCode>(); }
        else { card1 = pool[0]; pool.Remove(card1); }
        card1.code.enabledDrag = true;
        //card1.dr
        return card1;
    }
    public void CreateMinion(UnitData ud)
    {
        player.UD.minions.Add(ud);
    }
    public IEnumerator DealDamage(int value, UnitControl unidade, status target)
    {
        StartCoroutine(unidade.DealDamage1(value, target, 1));
        yield return null;
    }
    public void ResolveEvents(eventEnum e)
    {
        HBC.button(buttonNames.map).StartShining(false);

        switch (e)
        {
            case eventEnum.none:
                print("eventless Call programmer");
                break;
            case eventEnum.random:
                int random = UnityEngine.Random.Range(1, Enum.GetValues(typeof(eventEnum)).Length);
                if (random == 5) { random = 2; }
                //            print(random);
                ResolveEvents((eventEnum)random);
                break;
            case eventEnum.monster:
                print(e);

                enemy.SetData(DB.GetUnit(enemyLevel.monster));
                Reset();
                allButtons.SolveCast(classes.CloseAllTabs);
                //DisActive();
                HBC.button(buttonNames.battle).StartShining(true);
                //enemy.SetUnit();
                break;
            case eventEnum.elite:
                print(e);
                enemy.SetData(DB.GetUnit(enemyLevel.elite));
                Reset(); allButtons.SolveCast(classes.CloseAllTabs);
                //DisActive();
                HBC.button(buttonNames.battle).StartShining(true);
                // enemy.SetUnit();
                break;
            case eventEnum.midBoss:
                print(e);
                enemy.SetData(DB.GetUnit(enemyLevel.midBoss));
                Reset();
                //DisActive();
                allButtons.SolveCast(classes.CloseAllTabs); HBC.button(buttonNames.battle).StartShining(true);
                //enemy.SetUnit();
                break;
            case eventEnum.boss:
                print(e);
                enemy.SetData(DB.GetUnit(enemyLevel.boss));
                //HBC.button(buttonNames.map).StartShining(false);
                Reset();// DisActive();
                allButtons.SolveCast(classes.CloseAllTabs); HBC.button(buttonNames.battle).StartShining(true);

                //enemy.SetUnit();
                break;
            case eventEnum.shop:
                MC.SetPathstrue();//todo rewards
                DisActive();
                ShopShower.PopulateShop();
                //HBC.button(buttonNames.shop).StartShining(true);
                ShopOn();
                print(e);
                break;
            case eventEnum.chest:
                //MapShower.SetPathstrue();//todo rewards
                PRS.CreateItem(DB.GetRandomReward());
                allButtons.SolveCast(classes.Rewards);
                HBC.button(buttonNames.reward).StartShining(true);
                print(e);
                break;
            case eventEnum.camp:
                const int camp = 0;
                PRS.CreateItem(DB.AllRewards[camp]);
                allButtons.SolveCast(classes.Rewards);
                HBC.button(buttonNames.reward).StartShining(true);
                //   MapShower.SetPathstrue();//todo rewards
                print(e);
                //todo
                break;
            case eventEnum.soulGetter:
                //MC.SetPathstrue();//todo rewards
                PRS.CreateItem(DB.GetSoul()); allButtons.SolveCast(classes.Rewards);
                HBC.button(buttonNames.reward).StartShining(true);
                //print(e);
                break;
            default:
                print("null event is possible?");
                break;
        }
    }
    public bool BattleStop = false, SurrenderNow = false;

    public void Surrender()
    {
        if (!BattleStop) { SurrenderNow = true; CheckBattleEnd(); }
    }
    public void CheckBattleEnd()
    {
        if (enemy.GS(status.health).value1(status.health, this, enemy.SC) <= 0)
        {

            DisActive();
            SetRewards(enemy.UD.AllRewards);
            if (RI == null) { } else { PRS.BreakForReal(RI); RI = null; }
            EndBattle();
            player.SC.ResetStatus();
            UpdateAllCards();
            HBC.button(buttonNames.battle).StartShining(false);
            HBC.button(buttonNames.map).StartShining(false);
            HBC.button(buttonNames.reward).StartShining(true); ClearCards();
        }
        else
        {
            if (player.GS(status.health).value1(status.health, this, player.SC) <= 0 || SurrenderNow)
            {
                SurrenderNow = false;
                if (enemy.UD.Defeat.Count > 0)
                {
                    DisActive();
                    SetRewards(enemy.UD.Defeat);
                    if (RI == null) { } else { PRS.BreakForReal(RI); RI = null; }
                    EndBattle();
                    player.SC.ResetStatus();
                    UpdateAllCards();
                    HBC.button(buttonNames.battle).StartShining(false);
                    HBC.button(buttonNames.map).StartShining(false);
                    HBC.button(buttonNames.reward).StartShining(true); ClearCards();

                }
                else { EndBattle(); EndRun(); }
            }
        }

    }
    public GameObject RewardShower;
    public void SetRewards()
    {
        RewardShower.SetActive(!RewardShower.activeInHierarchy);
    }
    public DialogSystem DS;
    public CompanionControl CC;

    public void SetDialogue()
    {
        DS.SetUp();
    }
    public PrizeRewardShower PRS;
    public void SetRewards(List<RewardData> RD)
    {
        //print(!RewardShower.activeInHierarchy);
        RewardShower.SetActive(!RewardShower.activeInHierarchy);
        foreach (RewardData RD1 in RD)
        {
            PRS.CreateItem(RD1);
        }
        // ClearCards();
    }
    public HeadBarControl HBC;
    public void EndRun()
    {
        saveSlot = 0;
        SceneManager.LoadScene(0);
    }
}

public enum Target { Deck, Discard, PlayerTeam, EnemyTeam, Hand, Random, Empty }
public enum Trigger
{
    None, StartBattle, StartTurn, EndBattle, PlayCard,
    PlayAtkCard, PlayDefensiveCard, PlayUtilityCard, EndTurn, Discard, Draw, Consume, RageEnds
}
[Serializable]
public class Power
{
    public List<effects> efeito;
    public List<int> value;
    public int times;
    public source font;
    public int fontcode;
    public List<bool> reverse;
    public Target focus;
    public AI_Target Alvo;
    public RelicShower shower;
    public List<UnitData> minions;
    public List<BuffsAndDebuffsData> buff;
    public AtkAnimationData anime;
}
public enum alvo { player, PlayerTeam, EnemyTeam, enemy, playerMinion, enemyMinion, hand, deck, Discard, consume }
public enum source { card, item }
[Serializable]
public class AllLists
{
    public string name;
    public Trigger activator;
    public List<Power> items;

    public AllLists(string s, List<Power> i, Trigger tt)
    {
        name = s; items = i; activator = tt;
    }
}
[Serializable]
public class MinionData
{
    public UnitData ud;
    public Target team;
    public MinionData(UnitData u, Target t)
    {
        ud = u; team = t;
    }
}
public enum shower { deck, hand, discard, consume, fullDeck }
public enum CardMode { none, Soulable, Addable, Removable, Upgradable, Buyable }
public enum ChoiceRequeriment
{
    none, RelicCount, CardCount, Relic, Card, CompanionCount
, Companion, SurrenderCount, VictoryCount, EnemyCount, KillEnemy
}