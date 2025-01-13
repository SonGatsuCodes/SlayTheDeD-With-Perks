using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class DragAndDrop : MonoBehaviour
{
    public bool dragging = false, enabledDrag = true, addable = false, buyable = false, removable = false,
    upgradable = false, Soulable = false;
    public Manager M;
    public CardInfoShow CIS;
    public Collider2D collider1;
    public int value = 999;
    public TextMeshPro valueText;
    public void Begin()
    {
        dragging = true;
    }
    public GameObject valueContainer;
    public void SetValueVisible(bool visible)
    {

        valueContainer.SetActive(visible);

        valueText.text = value.ToString();
    }

    public void SetValue(int v)
    {

        value = v;
    }
    public void SetValue(bool buy)
    {
        if (buy) { value = CIS.info.buy * 10; }//to do: magic number solve in future
        else { value = CIS.info.buy; }
        if (value == 0) { value = 9999; }
    }
    public CardCode code;
    public IEnumerator FinishDrag()
    {
        //    M.HitWho();
        dragging = false;
        PlayAreaType areainfo = M.HitWhoTarget();
        Target alvo;
        //int actualmana = M.player.GS(status.mana).value1(status.mana, M, M.player.SC);
        if (areainfo == null) { alvo = Target.Empty; } else { alvo = areainfo.type; }
        if(alvo==Target.PlayerTeam || alvo==Target.EnemyTeam) 
        {if (CIS.info.custo > M.player.GS(status.mana).value1(status.mana, M, M.player.SC)) { alvo=Target.Empty; }}
        //int index = Hand.IndexOf(card);
        yield return StartCoroutine(AnimeLine(lineRenderer, .15f, alvo != Target.Empty));//magic number .25f=duration
        /* lineRenderer.SetPosition(0, Vector3.zero);lineRenderer.SetPosition(1, Vector3.zero);lineRenderer.SetPosition(2, Vector3.zero);        */
        M.L2.SetPosition(0, Vector3.zero);
        M.L2.SetPosition(1, Vector3.zero);
        switch (alvo)
        {
            case Target.Deck:
                M.ShowAllCards();
                print("deck");
                break;
            case Target.Discard:
                print("discard");
                StartCoroutine(M.DiscardCard(code));
                break;
            case Target.PlayerTeam:
                StartCoroutine(M.PlayCard(code, areainfo.unidade, true, M.player));
                print("player");
                break;
            case Target.EnemyTeam:
                StartCoroutine(M.PlayCard(code, areainfo.unidade, true, M.player));
                print("enemy");
                break;
            case Target.Hand:
            case Target.Random:
            case Target.Empty:
            default:
                print("the mouse hit nothing");
                break;
        }

    }
    public IEnumerator AnimeLine(LineRenderer t, float duration, bool foward)
    {
        float progress = 0, start = Time.time;
        Vector3 v, v1, l1 = t.GetPosition(0), l2 = t.GetPosition(1), l3 = t.GetPosition(2);
        v1 = foward ? l3 : l1;
        while (progress < 1)
        {
            progress = Mathf.Clamp01((Time.time - start) / duration);
            v = Vector3.Lerp(l1, v1, progress);
            t.SetPosition(0, v);
            v = Vector3.Lerp(l2, v1, progress);
            t.SetPosition(1, v);
            v = Vector3.Lerp(l3, v1, progress);
            t.SetPosition(2, v);
            yield return null;
        }
        t.SetPosition(0, Vector3.zero);
        t.SetPosition(1, Vector3.zero);
        t.SetPosition(2, Vector3.zero);
        yield return true;
    }
    Vector3 v;
    public void DragOn()
    {
        if (enabledDrag)
        {
            v = Input.mousePosition; Drawline();
            //transform.position=v;
            if (Input.GetMouseButtonUp(0))
            {
                StartCoroutine(FinishDrag());
            }
        }
    }
    public void OnMouseEnter()
    {
        //print("mouse entered");
        //Vector3 v=transform.position;
        //v.z=v.z-.1f;
        if (enabledDrag)
        {
            CardInfoShow cisshow = M.show.GetComponent<DragAndDrop>().CIS;
            cisshow.info = CIS.info;
            cisshow.Buffs = CIS.Buffs;
            cisshow.NewInfo();
            M.show.SetActive(true);
            if (cisshow.info.upgrade==null)
            {M.showUpgradeInBattle.SetActive(false);

            }
            else{
                M.showUpgradeInBattle.GetComponent<DragAndDrop>().CIS.info=cisshow.info.upgrade;
                M.showUpgradeInBattle.GetComponent<DragAndDrop>().CIS.NewInfo();
                M.showUpgradeInBattle.SetActive(true);
                
            Vector3 v = cisshow.transform.position;
            v.x += (v.x > 960) ? -500 : 420;
            v.y += (v.y > 540) ? -250 : 250;
                M.showUpgradeInBattle.transform.parent.position=v;
                }
            CIS.NewInfo();//update the card info? 
            //CIS.NewInfo();
            if (upgradable)
            {
                cisshow = M.showUpgrade.code.CIS;
                cisshow.info = CIS.info.upgrade;
                cisshow.Buffs = CIS.Buffs;
                cisshow.NewInfo();
                M.showUpgrade.gameObject.SetActive(true);

            }
        }
        
    }
    public void OnMouseOver()
    {
        if (upgradable)
        {
            Vector3 v = Input.mousePosition;
            v.x += (v.x > 960) ? -140 : 120;
            v.y += (v.y > 540) ? -150 : 150;
            M.showUpgrade.transform.position = v;
        }
        if(!enabledDrag)
        {
            if(upgradable)
            {
                M.showUpgradeInBattle.SetActive(true);
                M.showUpgradeInBattle.GetComponent<DragAndDrop>().CIS.info=CIS.info.upgrade;
                M.showUpgradeInBattle.GetComponent<DragAndDrop>().CIS.NewInfo();
                Vector3 v = Input.mousePosition;
            v.x += (v.x > 960) ? -140 : 120;
            v.y += (v.y > 540) ? -150 : 150;
            M.showUpgradeInBattle.transform.parent.position=v;
            }
        }
    }
    public void OnMouseExit()
    {
        if (enabledDrag)
        {
            M.show.SetActive(false);
            M.showUpgradeInBattle.SetActive(false);
            if (upgradable)
            {
                M.showUpgrade.gameObject.SetActive(false);
                
            }
        }
        else
        {
            if(upgradable)
            {
            //M.showUpgradeInBattle.transform.parent.position=Vector3.zero;
                M.showUpgradeInBattle.SetActive(false);
            
            }
        }
    }
    public RewardItem ri;
    public void OnMouseDown()
    {
        CardData old;
        if (enabledDrag) { Begin(); }
        if (addable) { M.AddCard(CIS.info); M.PRS.CardSelected(); M.PRS.BreakForReal(ri); }
        if (buyable) { M.ShopShower.Buy(CIS.info, value, this); }
        if (removable)
        {
            M.ShopShower.Remove(CIS.info, value, this);
            if (M.RI == null) { print("null reward item?"); }
            else { M.PRS.BreakForReal(M.RI); }
        }//magic number solve this 50= money cost
        if (upgradable)
        {
            M.ShopShower.Upgrade(CIS.info.upgrade, value, this, CIS.info);
            if (M.RI == null) { print("null reward item?"); } else { M.PRS.BreakForReal(M.RI); }
            if (M.BattleStop){}else{M.UpgradeHandInBattle(this);}
            M.showUpgrade.gameObject.SetActive(false);
        }//magic number solve this 50 is money cost
        if (Soulable)
        {
            old = CIS.info;
            M.RemoveCard(CIS.info);
            CIS.info = CardData.CreateInstance<CardData>();
            CIS.info.Init(old);
            CIS.info.souls.AddRange(CIS.soul);
            M.AddCard(CIS.info);
            M.SCI.RemoveSoul();
            M.CleanTempSouls();
            M.DisActive();
            M.ShowSoul();
        }
    }
    public LineRenderer lineRenderer;
    public Material lineMaterial;
    public void Start()
    {
        test();
    }
    public float widthS, widthF;
    public void test()
    {
        lineRenderer.enabled = true;
        lineRenderer.startWidth = widthS;
        lineRenderer.alignment = LineAlignment.View;

        lineRenderer.endWidth = widthF;
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = 3; // Set two positions for the arrow
        test(M.L2);

    }

    public void test(LineRenderer l2)
    {
        l2.enabled = true;
        l2.startWidth = widthS * 2;
        l2.alignment = LineAlignment.View;

        l2.endWidth = widthF;
        l2.material = lineMaterial;
        l2.positionCount = 2; // Set two positions for the arrow

    }
    public TextMeshProUGUI origin, end;
    public int offsetDrawline = -2;
    public int distance = 20;
    public void Drawline()
    {
        Vector3 v3 = transform.position, h = M.Inside();
        v3.z = offsetDrawline;
        Ray ray = Camera.main.ScreenPointToRay(v3);

        lineRenderer.SetPosition(0, v3);
        //origin.text = "origin: " + ray.origin.ToString();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ray.origin = new Vector3(ray.origin.x, ray.origin.y, offsetDrawline);
        lineRenderer.SetPosition(1, ray.origin);
        //end.text = "end: " + ray.origin.ToString();
        if (h == Vector3.zero)
        {
            lineRenderer.SetPosition(2, ray.origin);
            //end.text = "end: " + ray.origin.ToString();
            M.L2.SetPosition(0, Vector3.zero);
            M.L2.SetPosition(1, Vector3.zero);
        }
        else
        {
            h.z = offsetDrawline;
            lineRenderer.SetPosition(2, ray.origin);
            //end.text = "end: " + ray.origin.ToString();
            if (distance < Vector3.Distance(ray.origin, h))
            {
                M.L2.SetPosition(0, ray.origin);
                M.L2.SetPosition(1, h);
            }
            //end.text = "end: " + h.ToString();
        }
    }
    public void CardMovement(Vector3 start, Vector3 finish)
    {
        StartCoroutine(Move(start, finish));
    }
    public void CardMovement(Vector3 start, Vector3 finish, Vector3 next)
    {
        StartCoroutine(Move(start, finish, next));
    }
    public IEnumerator Move(Vector3 start, Vector3 finish)
    {
        //enabledDrag = false;
        //print("move trigged");
        const float animationTime = 0.25f;
        float startTime = Time.time, f;
        while (Time.time < (startTime + animationTime))
        {
            f = Mathf.Clamp((Time.time - startTime) * (1 / animationTime), 0, 1);
            transform.position = Vector3.Lerp(start, finish, f);
            yield return new WaitForSeconds(.0f);
        }
        if (M.BattleStop && !M.Full.Contains(this.code)) { finish.x = -3000; }
        transform.position = Vector3.Lerp(start, finish, 1);
    }
    public IEnumerator Move(Vector3 start, Vector3 finish, Vector3 next)
    {
        //enabledDrag = false;
        //print("move trigged");
        const float animationTime = 0.25f;
        float startTime = Time.time, f;

        while (Time.time < (startTime + animationTime))
        {

            f = Mathf.Clamp((Time.time - startTime) * (1 / animationTime), 0, 1);            //print("time="+f);
            transform.position = Vector3.Lerp(start, finish, f);//asdadr
            yield return new WaitForSeconds(.0f);
        }
        if (M.BattleStop && !M.Full.Contains(this.code)) { next.x = -3000; }
        transform.position = next;//Vector3.Lerp(start,finish,1);
        //enabledDrag = true;
    }
    public void Update()
    {
        if (dragging)
        {
            DragOn();
        }
    }
}

