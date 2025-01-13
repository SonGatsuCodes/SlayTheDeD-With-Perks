using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class UnitControl : MonoBehaviour
{
    public Manager M;
    public UnitData UD;
    public BoxCollider2D BC2d;
    public List<atributes> status1;
    public StatusControl SC;
    public SpriteRenderer model, intention, teamIcon, animador;
    public ModelControl modelComplex;
    public TextMeshPro intentionValue;
    public PlayAreaType areaType;
    [Serialize]
    public List<atk> atks;
    public AI behaviour;
    public AI_Target unitTarget;
    public List<atributes> buffModifier;
    public int IndexAtk;
    public SpriteRenderer BallonDialog;
    public TextMeshPro DialogText;
    public void SetIntention(intentions i, string s)
    {
        intention.sprite = M.DB.GetIntention(i);
        intentionValue.text = s;

    }
    public void SetAreaType(Target t)
    {
        areaType.type = t;
    }
    public void SetIntention(atk atk)
    {
        string s = "", v = "";
        int mod = 0;//SC.Contains(M.DB.Mod(atk.efeito[i]));
        for (int i = 0; i < atk.value.Count; i++)
        {
            mod = SC.Contains(M.DB.Mod(atk.efeito[i]));
            v = (atk.value[i] + mod).ToString();
            if (i == 0) { s = v; }
            else { s = s + ", " + v; }

        }
        intention.sprite = M.DB.GetIntention(atk.Intentions);
        intentionValue.text = s;
    }
    public void SetIntention()
    {
        SetIntention(atks[IndexAtk]);
    }


    public void UpdateAtri()
    {
        string S = "";
        atributes x;
        //foreach (atributes x in status1)
        for (int i = 0; i < status1.Count; i++)
        {
            x = status1[i];
            if (x.text != null)
            {
                if (x.nome == status.health)
                {
                    S = "/" + GS(status.maxHealth).value1(status.maxHealth, M, SC);
                    if (PlayerHealth != null)
                    {
                        PlayerHealth.text = x.value1(x.nome, M, SC).ToString() +
                     "/" + GS(status.maxHealth).value1(status.maxHealth, M, SC);
                    }
                }
                if (x.nome == status.mana) { S = "/" + GS(status.maxEnergy).value1(status.maxEnergy, M, SC); }
                x.text.text = x.value1(x.nome, M, SC).ToString() + S;
                S = "";
            }
        }
    }
    public void SetData(UnitData u)
    {
        UD = u;
        SetUnit();
    }
    public void SetTeamIcon()
    {
        teamIcon.color = M.DB.GetTeamColor(areaType.type);
        modelComplex.SetSide(areaType.type == Target.PlayerTeam);
    }
    public void SetUnit()
    {
        //print(UD.name);        //UD.description;        //UD.cod;
        model.sprite = UD.art;
        atks = UD.atks;
        behaviour = UD.aI;
        unitTarget = UD.target;
        SetTeamIcon();        //UD.atributes; todo
        for (int i = 0; i < UD.atributes.Count; i++)
        {
            GS(i).value = UD.atributes[i].value;
        }
        IndexAtk = 0;        //print("unit seted?");
        UpdateAtri();
    }
    public TextMeshPro PlayerHealth;
    public IEnumerator Blink(status s, float duration)
    {
        GS(s).StartShining(true);
        if (s == status.health && PlayerHealth != null)
        {
            StartCoroutine(Blink1(PlayerHealth.GetComponentInParent<SpriteRenderer>(), duration));
        }
        yield return StartCoroutine(GS(s).Shining(duration));
    }
    public IEnumerator Blink1(SpriteRenderer s, float duration)
    {
        yield return StartCoroutine(Shining(s, duration));
    }
    public atributes GS(status s)
    {
        return GS((int)s);
    }
    public atributes GS(int i)
    {
        return status1[i];
    }
    public IEnumerator DealDamage1(int dmg, status s, float duration)
    {
        status trigged;
        int index = (int)s; const int shield = (int)status.shield;
        //        print("before switch explanation");
        switch (s)
        {
            case status.penetrate:
                GS(status.health).value = GS(status.health).value - dmg;
                trigged = status.health;
                //yield return StartCoroutine(Blink(status.health));
                break;
            case status.health:
                if (GS(shield).value > 0)
                {
                    int sum = dmg - GS(shield).value;
                    if (sum > 0)
                    {
                        GS(shield).value = 0;
                        yield return StartCoroutine(DealDamage1(sum, status.health, duration));
                        trigged = status.health;
                        //yield return StartCoroutine(Blink(status.health));
                    }
                    else
                    {
                        GS(shield).value = GS(shield).value - Math.Clamp(dmg, 0, int.MaxValue);
                        trigged = status.shield;
                        //yield return StartCoroutine(Blink(status.shield));
                    }
                }
                else
                {
                    GS(index).value = GS(index).value - dmg;
                    trigged = status.health;
                    //yield return StartCoroutine(Blink(status.health));                    
                }
                break;
            case status.shield:
                GS(index).value = math.clamp(GS(index).value - dmg, 0, 9999);
                trigged = status.shield;
                //yield return StartCoroutine(Blink(status.shield));
                break;
            case status.cure:
                //GS(status.health].StartShining(true);
                GS(status.health).value = Mathf.Clamp((GS(status.health).value + dmg),
                 -999, GS(status.maxHealth).value);
                trigged = status.health;
                //yield return StartCoroutine(Blink(status.health));
                break;
            case status.mana:
                GS(index).value = Mathf.Clamp((GS(index).value - dmg)
                , 0, GS(status.maxEnergy).value1(status.maxEnergy, M, SC));
                trigged = status.mana;
                //yield return StartCoroutine(Blink(status.mana));
                break;
            //case status.maxHealth: break;
            //case status.maxEnergy:                break;
            default:
                GS(index).value = GS(index).value - dmg;
                trigged = (status)index;
                //yield return StartCoroutine(Blink((status)index));
                break;
        }
        //        print("after triggers");
        //yield return true;
        UpdateAtri();
        yield return StartCoroutine(Blink(trigged, duration));
    }
    public IEnumerator Shining(SpriteRenderer s, float duration)
    {
        //print("time1="+Time.time);
        bool work = true;
        Color old = s.color; old.a = 1;
        float f = Time.time, t;
        while (work)
        {
            t = (Time.time - f) / duration;
            s.color = Color.Lerp(new Color(old.r, old.g, old.b, 0), new Color(old.r, old.g, old.b, 1), t);
            yield return null;
            if (Time.time > (f + 1)) { f = Time.time; work = false; }
        }
        s.color = old;
        //print("time2="+Time.time);
        //M.AM.Done=true;
    }
    public Vector3 MultiVector(Vector3 a, Vector3 b)
    {
        Vector3 v = Vector3.zero;
        v.x = a.x * b.x;
        v.y = a.y * b.y;
        v.z = a.z * b.z;
        return v;
    }
    public Vector3 DivideVector(Vector3 a, Vector3 b)
    {
        Vector3 v = Vector3.zero;
        v.x = a.x / b.x;
        //        print(a.x+"/"+b.x+"="+v.x);
        v.y = a.y / b.y;
        v.z = a.z / b.z;
        return v;
    }

    public IEnumerator AnimeSolver(Anime a, Vector3 targetUC, Vector3 scale)
    {
        //if(a==null){a=test;}
        Frame actual;
        SpriteRenderer ani = animador;
        Vector3 originalPosition = transform.position, originalScale = new(.6f, .6f, .6f)
        , v = Vector3.zero, v1 = Vector3.zero, s = Vector3.zero, s1 = Vector3.zero, old = originalPosition
        , oldScale = originalScale;
        Vector4 c = Vector4.one, c1 = Vector4.one;
        float limit, progress;

        //print("scale=" + scale + " originalsscale=" + originalScale);
        //originalScale = MultiVector(scale, originalScale);
        for (int i = 0; i < a.allFrames.Count; i++)
        {
            actual = a.allFrames[i];
            limit = Time.time + actual.time;
            originalPosition = (actual.UseTargetPosi ? targetUC : old);
            if (i == 0)
            {
                ani.sprite = actual.art;
                v = ani.transform.position; v1 = originalPosition + actual.position;
                s = actual.ScaleUseTarget ? MultiVector(originalScale, scale) : originalScale; s1 = MultiVector(originalScale, actual.scale);
                c = ani.color; c1 = actual.cor;

                //f = Time.time;
                progress = 1;
                ani.transform.position = Vector3.Lerp(v, v1, progress);
                ani.transform.localScale = Vector3.Lerp(s, s1, progress);
                ani.color = Vector4.Lerp(c, c1, progress);
                yield return null;//new WaitForSeconds(a.allFrames[i].time);

            }
            else
            {
                if (ani.sprite != actual.art) { ani.sprite = actual.art; }
                if (ani.transform.position != originalPosition + actual.position) { v = ani.transform.position; v1 = originalPosition + actual.position; }
                if (ani.transform.localScale != MultiVector(originalScale, actual.scale)) { s = ani.transform.localScale; s1 = MultiVector(originalScale, actual.scale); s1 = actual.ScaleUseTarget ? MultiVector(s1, scale) : s1; }
                if (ani.color != actual.cor) { c = ani.color; c1 = actual.cor; }
                while (Time.time < limit)
                {
                    //f = Time.time;
                    progress = Mathf.Clamp01(1 - ((limit - Time.time) / actual.time));
                    ani.transform.position = Vector3.Lerp(v, v1, progress);
                    ani.transform.localScale = Vector3.Lerp(s, s1, progress);
                    ani.color = Vector4.Lerp(c, c1, progress);
                    yield return null;//new WaitForSeconds(a.allFrames[i].time);
                }
                if (i == a.allFrames.Count - 1)
                {
                    progress = Mathf.Clamp01(1 - ((limit - Time.time) / actual.time));
                    ani.transform.position = Vector3.Lerp(v, v1, progress);
                    ani.transform.localScale = Vector3.Lerp(s, s1, progress);
                    ani.color = Vector4.Lerp(c, c1, progress);
                    yield return null;//new WaitForSeconds(a.allFrames[i].time);

                }
            }
        }
        ani.sprite = null;
        ani.transform.position = old;
        ani.transform.localScale = oldScale;
        ani.color = Color.white;
    }
}
public enum status { health, mana, shield, maxHealth, maxEnergy, penetrate, cure }
[Serializable]
public class atributes
{
    public status nome;
    public SpriteRenderer sprite;
    public TextMeshPro text;
    public int value;
    public SpriteMask mask;
    public bool work;
    public void StartShining(bool b)
    {
        work = b;
        if (b) { }
        else { Color old = sprite.color; old.a = 1; sprite.color = old; }
    }
    public int value1(status s, Manager M, StatusControl SC)
    {
        int mod = SC.Contains(M.DB.Mod(s));
        mod = (int)(mod * M.DB.ModWeigth(s));
        return value + mod;
    }
    public int value1(BuffsAndDebuffs s, Manager M, StatusControl SC)
    {
        int mod = SC.Contains(s);
        mod = (int)(mod * M.DB.Weight(s));
        return value + mod;
    }

    public atributes(status nome1, SpriteRenderer sprite1, TextMeshPro text1, int value1, SpriteMask mask1, bool work1)
    {

        nome = nome1;
        sprite = sprite1;
        text = text1;
        value = value1;
        mask = mask1;
        work = work1;
    }

    public IEnumerator Shining(float duration)
    {
        Color old = Color.white;
        if (sprite == null) { }
        else { old = sprite.color; old.a = 1; }
        float f = Time.time, t;
        while (work)
        {
            t = Time.time - f;
            if (sprite == null) { }
            else { sprite.color = Color.Lerp(new Color(old.r, old.g, old.b, 0), new Color(old.r, old.g, old.b, 1), t); }
            yield return null;
            if (Time.time > (f + duration)) { f = Time.time; work = false; }

        }
        if (sprite == null) { }else { sprite.color = old; }//a bug about max heelth in status1 not having a sprite
    }
}
public enum AI { Random, Incremental, Decremental }
public enum AI_Target { Random, Summon, Protagonist, Front, Back }
[Serializable]
public class atk
{
    public int cost;
    public List<int> value;
    [Serialize]
    public List<effects> efeito;
    public intentions Intentions;
    public List<bool> reverse;
    public bool EnemyTarget = true;
    public List<UnitData> minions;
    public List<BuffsAndDebuffsData> buff;
    public atk upgrade, downgrade;
    public AtkAnimationData anime;
}