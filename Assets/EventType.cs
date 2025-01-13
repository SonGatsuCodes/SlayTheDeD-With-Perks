using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EventType : MonoBehaviour
{
    public MapCreator MC;
    public eventEnum eventEnum;
    public FakeButton button;
    public SpriteRenderer art, way1, way2, way3, way4, way5;
    // Start is called before the first frame update
    
    public void SetEnum()
    {
        int random = UnityEngine.Random.Range(1,Enum.GetValues(typeof(eventEnum)).Length);
        if (random == 5)
        {random=2;}
        eventEnum = (eventEnum)random;
    }
    public void SetArt()
    {
        art.sprite=MC.M.DB.eventArt[(int)eventEnum];
    }
    public void paths(int options)
    {
        Vector2Int a;
        switch (options)
        {
            case 1:
                a = new Vector2Int(0, 1);
                break;
            case 2:
                a = new Vector2Int(1, 0);
                break;
            case 3:
                a = new Vector2Int(1, 1);
                break;
            default:
                a = new Vector2Int(0, 0);
                break;
        }
        way1.color = new Color(1, 1, 1, a.x);
        way3.color = new Color(1, 1, 1, a.y);
    }
    public bool LifeFindsAWay(int pathIndex)
    {
        //List<SpriteRenderer> way =new List<SpriteRenderer>();
        SpriteRenderer[] way = new SpriteRenderer[3];
        way[0] = way1;
        way[1] = way2;
        way[2] = way3;
//        print("way" + pathIndex + "=" + (way[pathIndex].color.a == 1));
        if (way[pathIndex].color.a == 1)
        { return true; }
        return false;
    }
    // Update is called once per frame
    /*void Update()
    {

    }*/
    public void StartShining(bool b)
    {
        work = b;
        if (b) { StartCoroutine(Shining()); }
        else { art.color = Color.white; }
        button.SetInteractable(b);
    }
    public void TriggerEventor(GameObject g ,bool shine)
    {
       // StartShining(shine);
           MC.ShiningAll(false);
           MC.SetPaths(this);
    }
    public bool work = false;
    
    public IEnumerator Shining()
    {
        float f = Time.time, t;
        while (work)
        {
            t = Time.time - f;
            art.color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), t);
            yield return new WaitForSeconds(0);
            if (Time.time > (f + 1)) { f = Time.time; }
        }
    }
}
public enum eventEnum { none, random, monster, elite, midBoss, boss, shop, chest, camp, soulGetter }