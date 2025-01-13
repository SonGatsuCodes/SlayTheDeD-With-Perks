using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapCreator : MonoBehaviour
{
    public Manager M;
    public GameObject mapEvent;
    public Vector2Int size = new Vector2Int(6, 6);
    public Vector2 cell = new Vector2(50, 20);
    public Vector2 intial = new Vector2(0, 0);
    public List<EventType> AllEvents;
    public EventType Boss;
    public Sprite t1, t2, t3;
    public SortingGroup SG;
    // Start is called before the first frame update
    public void PopulateMap()
    {
        if (AllEvents.Count == 0)
        {
            GameObject g;
            Vector3 v = intial;
            float mod = 0;
            //int[] m = virtualMap();
            int a = 0;
            EventType ev;
            for (int j = 0; j < size.x; j++)
            {
                if (j % 2 == 0)
                { mod = (cell.y / 2); a = 1; }
                else { mod = 0; a = 0; }
                for (int i = 0; i < size.y - a; i++)
                {
                    g = Instantiate(mapEvent, this.transform);
                    g.transform.position = new Vector3(cell.x * j + v.x,
                    cell.y * i                     // * UnityEngine.Random.Range(0,(int)size.y)
                     + v.y + mod, 1);
                    ev = g.GetComponent<EventType>();
                    AllEvents.Add(ev);
                    ev.StartShining(false);                    //ev.art.sprite = a == 1 ? t1 : t2;
                    ev.paths(DropRate());
                    if (a == 0 && i == 0) { ev.paths(2); }
                    if (a == 0 && (i == (size.y - 1))) { ev.paths(1); }
                    if (j == (size.x - 1)) { ev.paths(0); }                    //if (j == 0) { ev.StartShining(true); }
                    ev.SetEnum();
                    ev.SetArt();
                }
            }
        }
        else
        {
            SG.sortingOrder = (SG.sortingOrder == -160) ? 160 : -160;// ?SG.sortingOrder=;
            //bool a = (SG.sortingOrder == -160) ? M.EnableCollider() : M.DisableColliders();
            transform.position = !(SG.sortingOrder == -160) ? Vector3.zero : outside;
            //M.
        }
    }
    public void RedoDungeon()
    {

        //GameObject g;
        Vector3 v = intial;
        float mod = 0;
        //int[] m = virtualMap();
        int a = 0;
        EventType ev;

        /*
        for (int i = 0; i < AllEvents.Count; i++)
        {if ((i/5) % 2 == 0)
        { mod = (cell.y / 2); a = 1; }
        else { mod = 0; a = 0; }

            AllEvents[i].StartShining(false);                    //ev.art.sprite = a == 1 ? t1 : t2;
            AllEvents[i].paths(DropRate());
            if (a == 0 && i == 0) { AllEvents[i].paths(2); }
            if (a == 0 && (i == (size.y - 1))) { AllEvents[i].paths(1); }
            if ((i / 5) == (size.x - 1)) { AllEvents[i].paths(0); }                    //if (j == 0) { ev.StartShining(true); }
            AllEvents[i].SetEnum();
            AllEvents[i].SetArt();
        }*/
        int x=0;
        for (int j = 0; j < size.x; j++)
        {
            if (j % 2 == 0) { mod = (cell.y / 2); a = 1; } else { mod = 0; a = 0; }
            for (int i = 0; i < size.y - a; i++)
            {
                //g = Instantiate(mapEvent, this.transform);
                
                //g.transform.position = new Vector3(cell.x * j + v.x, cell.y * i + v.y + mod, 1);
        //        AllEvents.Add(g);

                ev =AllEvents[x];// g.GetComponent<EventType>();
                ev.StartShining(false);
                ev.paths(DropRate());
                if (a == 0 && i == 0) { ev.paths(2); }
                if (a == 0 && (i == (size.y - 1))) { ev.paths(1); }
                if (j == (size.x - 1)) { ev.paths(0); }                    //if (j == 0) { ev.StartShining(true); }
                ev.SetEnum();
                ev.SetArt();
                x++;
            }
        }

    }
    public Vector3 outside = new Vector3(-9000, 0, 0);
    public void ShiningAll(bool shine)
    {
        foreach (EventType ev in AllEvents)
        {
            ev.StartShining(shine);
        }
        Boss.StartShining(shine);
        M.HBC.button(buttonNames.map).StartShining(shine);

    }
    public void SetPaths(EventType e)
    {
        M.HBC.button(buttonNames.map).StartShining(true);
        M.HBC.button(buttonNames.shop).SetInteractable(false);
        M.HBC.button(buttonNames.shop).StartShining(false);
        optionstrigger = e;
        print("caminho");
    }
    public void SetPathstrue()
    {
        print("true caminhos?");
        //M.HBC.button(buttonNames.shop).SetInteractable(false);
        EventType e = optionstrigger;
        int a = 0;
        a = AllEvents.IndexOf(e);
        //print(a);
        if (a == -1) { for (int i = 0; i < 5; i++) { SetShining(i); } }
        else
        {
            EventType even = AllEvents[a].GetComponent<EventType>();

            if ((a + 6) > AllEvents.Count) { Boss.StartShining(true);/*M.HBC.button(buttonNames.map).StartShining(true);*/ }
            if (even.LifeFindsAWay(2)) { SetShining(a + 5); }
            if (even.LifeFindsAWay(0)) { SetShining(a + 6); }

        }
    }
    public EventType optionstrigger;
    public void SetShining(int a)
    {
        AllEvents[a].GetComponent<EventType>().StartShining(true);
        print("map button shining?");
        M.HBC.button(buttonNames.map).StartShining(true);
    }

    public int DropRate()
    {
        int r = UnityEngine.Random.Range(0, 100);
        if (r < 20) { return 1; }
        if (r < 40) { return 2; }
        else { return 3; }
        //return a;
    }
    public void Disabler()
    {
        SG.sortingOrder = -160;// ?SG.sortingOrder=;
        //gameObject.SetActive(SG.sortingOrder==160);
        //bool a=(SG.sortingOrder==-160)?M.EnableCollider():M.DisableColliders();
    }
    public int[] virtualMap()
    {
        int[] array = new int[(int)size.x];
        bool[,] map = new bool[size.x, size.y];

        /*
        o x x x x x x x 
        x x x x x x x x 
        o x x x x x x x 
        x x x x x x x x 
        o x x x x x x x 
        x x x x x x x x 
        */
        //        print(map);
        for (int i = 0; i < size.x; i++)
        {
            array[i] = UnityEngine.Random.Range(3, (int)size.y);
        }
        return array;
    }
    /*void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }*/
}
