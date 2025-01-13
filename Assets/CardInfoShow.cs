using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class CardInfoShow : MonoBehaviour
{
    public Manager M;
    public CardData info;
    public List<SoulData> soul;
    public TextMeshPro descText, costText, reuseText;
    public SpriteRenderer background, art, artBack, description, costSprite, reuseArt;
    public List<mods> Buffs;
    public void NewInfo()
    {
        descText.text = DescUpdate() /*s+ TranslateSoul(info.souls)*/ + " <color=#00FFC7ff>" + TranslateSoul(soul) + d;
        costText.text = info.efeitos.Contains(effects.card_cost)?"X":info.custo.ToString();
        art.sprite = info.art;
        artBack.sprite = info.backart;
        costSprite.sprite = info.costArt;
        reuseText.text = info.reusability.ToString();
        reuseArt.color = GetColorReuse(info.reusability);
        background.color= GetColorByType(info.type);
    }
    public Color GetColorReuse(cardtype2 ct)
    {
        float a = 0, b = 0, c = 0;
        switch (ct)
        {
            case cardtype2.normal:
                b = 0.825f;
                c=.825f;
                a=.825f;
                
                break;
            case cardtype2.exaust:
                a=.7f;
                b = 0.4f;
                break;
            case cardtype2.consum:
                a = 0.6f;
                break;
        }

        return new Color(a, b, c, 1);
    }
    public Color GetColorByType(cardtype ct)
    {
        float a = 0, b = 0, c = 0;
        switch (ct)
        {
            case cardtype.combat:
                a=.5f;
                break;
            case cardtype.defense:
                c=.5f;
                break;
            case cardtype.utility:
            b=.5f;
                break;
        }

        return new Color(a, b, c, 1f);
    }
    public string TranslateSoul(List<SoulData> ss)
    {
        string s = "";
        if (ss.Count > 0)
        {
            for (int i = 0; ss.Count > i; i++)
            {
                if (ss[i] == null)
                {

                }
                else
                {
                    if (ss[i].valueCard.Count > 0)
                    {
                        for (int j = 0; j < ss[i].valueCard.Count; j++)
                        {
                            s = s + " " + ss[i].CardEfeitos[j] + " " + ss[i].valueCard[j];
                        }
                    }
                }
            }
        }
        return s;
    }
    public const string y = "<color=#ffff00ff>", r = "<color=#ff0000ff>", g = "<color=#00ff00ff>", b = "<color=#ffffffff>", d = "</color>";
    public string DescUpdate()
    {
        //M.player.
        bool write = true;
        int mod = 0;
        string s = info.descricao, s1 = "", number = "", cor = b;

        // number=r;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '<') { s1 = s1 + y; i++; }
            if (s[i] == '>') { s1 = s1 + d; i++; }

            if (s[i] == '[')
            {
                write = false;
                //s1=s1+cor;
            }
            if (write)
            {
                s1 = s1 + s[i];
            }

            if (s[i] == ']')
            {
                //number=number+d;
                cor = GetColor(info.efeitos[int.Parse(number)]);
                mod = GetValue(info.efeitos[int.Parse(number)]);
                s1 = s1 + cor + (info.values[int.Parse(number)] + mod) + d;//+info.efeitos[int.Parse(number)]
                                                                           //                print(number);
                number = "";
                write = true;
            }

            if (!write && s[i] != '[')
            {
                number = number + s[i];
            }
        }
        s = "";
        for (int i = 0; i < info.souls.Count; i++)
        {
            for (int i1 = 0; i1 < info.souls[i].CardEfeitos.Count; i1++)
            {
                s = s + " " + info.souls[i].CardEfeitos[i1] + " " + GetColor(info.souls[i].CardEfeitos[i1])
                + (info.souls[i].valueCard[i1] + GetValue(info.souls[i].CardEfeitos[i1])) + "</color>";
            }
        }
        s1 = s1 + s;
        return s1;
    }
    public string GetColor(effects e)
    {
        int v = 0;
        if (M.player == null) { }
        else
        { v = M.player.SC.Contains(M.DB.Mod(e)); }
        //print("v="+v);
        if (v > 0)
        {
            return g;
        }
        if (v < 0)
        {
            return r;
        }

        return b;
    }
    public int GetValue(effects e)
    {
        int v = 0;
        if (M.player == null) { }
        else
        { v = M.player.SC.Contains(M.DB.Mod(e)); }//fdsfsd
        //        print("v1="+v);
        return v;
    }

}
