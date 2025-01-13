using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Mono.Cecil.Cil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RelicShower : MonoBehaviour
{
    public Manager M;
    public SpriteRenderer art,animeSprite;
    public ItemData item;
    public SoulData soul;
    public FakeButton button;
    public Collider2D collider1;    //public GameObject gameObject;
    public TextMeshPro valueText, description;
    public GameObject valueContainer, descContainer;
    public SortingGroup SG;
    public RelicShower big;
    public IEnumerator AnimeSolver(Anime a,Vector3 targetUC,Vector3 scale,UnitControl uc)
    {
        print(scale);
        //if(a==null){a=test;}
        Frame actual;
        SpriteRenderer ani = animeSprite;
        Vector3 originalPosition = transform.position, originalScale = Vector3.one
        , v = Vector3.zero, v1 = Vector3.zero, s = Vector3.zero, s1 = Vector3.zero, old = originalPosition
        , oldScale = originalScale;
        Vector4 c = Vector4.one, c1 = Vector4.one;
        float limit, progress;

        print("scale="+scale+" originalsscale="+originalScale);
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
                s = actual.ScaleUseTarget?uc.MultiVector(originalScale,scale):originalScale; s1 = uc.MultiVector(originalScale, actual.scale);
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
                if (ani.transform.localScale != uc.MultiVector(originalScale, actual.scale)) { s = ani.transform.localScale; s1 = uc.MultiVector(originalScale, actual.scale);s1=actual.ScaleUseTarget?uc.MultiVector(s1,scale):s1; }
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
            }
            

            
                
        }
        ani.sprite = null;
        ani.transform.position = old;
        ani.transform.localScale = oldScale;
        ani.color = Color.white;
    
        //if(a==null){a=test;}
        /*
        Frame actual;
        SpriteRenderer ani = animeSprite;
        Vector3 originalPosition = ani.transform.position, originalScale = ani.transform.localScale
        , v = Vector3.zero, v1 = Vector3.zero, s = Vector3.zero, s1 = Vector3.zero,old=originalPosition
        ,oldScale=originalScale;
        Vector4 c = Vector4.one, c1 = Vector4.one;
        float limit, progress;

        originalScale=Vector3.Scale(scale,new Vector3(.5f,.5f,.5f));
        for (int i = 0; i < a.allFrames.Count; i++)
        {
            actual = a.allFrames[i];
            limit = Time.time + actual.time;
            originalPosition=(actual.UseTargetPosi?targetUC:old);
            if (i == 0)
            {
                ani.sprite = actual.art;
                v = ani.transform.position; v1 = originalPosition + actual.position;
                s = ani.transform.localScale; s1 = uc.MultiVector(originalScale, actual.scale);
                c = ani.color; c1 = actual.cor;
            }
            else
            {
                if (ani.sprite != actual.art) { ani.sprite = actual.art; }
                if (ani.transform.position != originalPosition + actual.position) { v = ani.transform.position; v1 = originalPosition + actual.position; }
                if (ani.transform.localScale != uc.MultiVector(originalScale, actual.scale)) { s = ani.transform.localScale; s1 = uc.MultiVector(originalScale, actual.scale); }
                if (ani.color != actual.cor) { c = ani.color; c1 = actual.cor; }
            }
            while (Time.time < limit)
            {
                //f = Time.time;
                progress = Mathf.Clamp01(1 - ((limit - Time.time) / actual.time));
                ani.transform.position = Vector3.Lerp(v, v1, progress);
                ani.transform.localScale = Vector3.Lerp(s, s1, progress);
                ani.color = Vector4.Lerp(c, c1, progress);
                yield return null;//new WaitForSeconds(a.allFrames[i].time);
            }

        }
        ani.sprite = null;
        ani.transform.position = old;
        ani.transform.localScale = oldScale;
        ani.color = Color.white;*/
    }
    public void SetArt()
    {
        art.sprite = item.art;
        art.color = Color.white;
        collider1.enabled = true;
    }
    public void SetValueVisible(bool visible)
    {
        valueContainer.SetActive(visible);
        valueText.text = gold.ToString();
    }
    public void showDesc(bool special)
    {
        if (descContainer == null || description == null) { }
        else
        {
            descContainer.SetActive(true);
            string s = "";
            bool soulExist = false,disableColors=!special;
            
            if (item.soul == null)
            {
//              print("souless item");  
              //disableColors=true;
            }
            else
            {
                soul=item.soul;
                disableColors=(true);
            }
            if (soul == null)
            {
                for (int i = 0; i < item.efeito.Count; i++)
                {
                    s = s + CalculateString(item.efeito[i], item.value[i], item.buff) + " " +(item.value[i] ).ToString();
                    if (i == item.value.Count - 1) { s = s + '.'; }
                    else { s = s + ','; }
                }
            }
            else
            {
                for (int i = 0; i < item.efeito.Count; i++)
                {
                    soulExist = soul.RelicEfeitos.Contains(item.efeito[i]);
                    s = s + (disableColors?"":(soulExist ? "<color=#00ff00ff>" : "<color=#ffffffff>"));
                    s = s + CalculateString(item.efeito[i], item.value[i], item.buff) + " " +
                    (item.value[i] + (soulExist ? soul.valueRelic[soul.RelicEfeitos.IndexOf(item.efeito[i])] : 0)).ToString();
                    
                    s = s + (disableColors?"":"</color>");
                    if (i == item.value.Count - 1) { s = s + '.'; }
                    else { s = s + ','; }
                }
                for (int i = 0; i < soul.RelicEfeitos.Count; i++)
                {
                    soulExist = item.efeito.Contains(soul.RelicEfeitos[i]);
                    if (!soulExist)
                    {
                        s = s + (disableColors?"":"<color=#00ff00ff>");
                        s = s + CalculateString(soul.RelicEfeitos[i], soul.valueRelic[i], soul.buff) + " " + soul.valueRelic[i];
                        //+(item.value[i]+(soulExist?soul.valueRelic[soul.RelicEfeitos.IndexOf(item.efeito[i])]:0)).ToString();
                        s = s + (disableColors?"":"</color>");
                        if (i == item.value.Count - 1) { description.text = description.text + '.'; }
                        else { description.text = description.text + ','; }
                    }
                }
                
            }
            description.text = s + " at " + item.activation + ' ' + 't' + 'o' + ' ' + item.target + ' ' + item.alvo;
        }
    }
    public string CalculateString(effects e, int v, List<BuffsAndDebuffsData> b)
    {
        string s = "";
        switch (e)
        {
            case effects.other_buffOrDebuff:
                s = " " + b[v].identifier.ToString();
                break;
            default:
                s = " " + e.ToString();
                break;

        }
        return s;
    }
    public void CloseDesc()
    {
        if (descContainer == null) { } else { descContainer.SetActive(false); }
    }
    public void setBigValue()
    {
        if (big == null) { }
        else
        {
            if (valueContainer.activeInHierarchy)
            {
                if (big.gameObject.activeInHierarchy)
                {

                    big.gold = gold;
                    big.SetValueVisible(true);
                }
            }
        }
    }
    public void OnMouseEnter()
    {
        if (big == null) { } else { big.gameObject.SetActive(true); big.item = item;big.soul=soul; big.SetArt(); big.showDesc(false); setBigValue(); }
    }
    public void OnMouseExit()
    {
        if (big == null) { }
        else
        {
            if (valueContainer.activeInHierarchy) { big.SetValueVisible(false); }
            big.gameObject.SetActive(false);
        }
    }
    public int gold;
    public void SetValue(bool buy)
    {
        if (buy) { gold = item.gold * 10; }//to do: magic number solve in future
        else { gold = item.gold; }
        if (gold == 0) { gold = 9999; }
        //gold=item.gold*10;
    }
    public void SetValue(int v)
    {

        gold = v;
    }
    public void CallAnimation()
    {
        StartCoroutine(Blink());
    }
    // Start is called before the first frame update
    public IEnumerator Blink()
    {
        const float animationTime = 0.25f;
        float startTime = Time.time, f;

        while (Time.time < (startTime + animationTime))
        {

            f = Mathf.Clamp((Time.time - startTime) * (1 / animationTime), 0, 1);
            //print("time="+f);
            art.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), f);
            //    transform.position = Vector3.Lerp(start, finish, f);

            yield return null;
        }
        //transform.position = Vector3.Lerp(start, finish, 1);
        art.color = Color.white;

        //yield return new WaitForSeconds(0);

    }
}
