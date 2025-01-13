using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.Rendering.UI;

public class DataManipulator : MonoBehaviour
{
    [Header("dependecies")]
    public TextAsset test;
    //public bool createCSV=false;
    public List<Special2> s;
    //public UnitData ud;
    ////public List<UnitData> test2; 
    //[JsonArray]
    [Serialize]
    public List<object> ss;
    //[JsonArray]
    // Start is called before the first frame update
    void OnValidate()
    {
        //Json s1;
        if (test != null)
        {
            s.Clear();
            List<string> ss=new List<string>();
            ss.AddRange(test.text.Split('\n'));
            foreach (string s1 in ss)
            {
                Special2 s2=new Special2();
                s2.n = s1.Split(',')[0];
                if(s1.Split(',').Count()>1){
                s2.done=(s1.Split(',')[1]=="done");}
                else
                {s2.done=false;}
                s.Add(s2);
            }
            //s.Clear();
            //ss= new List<string>();
            //s1=JsonUtility.ToJson(ud);
            //ss.AddRange(JsonUtility.ToJson(ud).Split(","));

        }
    }
}
[Serializable]
public class Special2
{
    [HideInInspector]
    public string n;
    public bool done;
}