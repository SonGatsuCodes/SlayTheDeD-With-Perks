using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDebug : MonoBehaviour
{
   public Slider s;
   public void SetTimeScale()
   {
    Time.timeScale = 1/s.value;
   }/*
   public float GetValue(int s)
   {
    switch (s)
    {
        case 1: return .1f;
        case 2: return .25f;
        default: return s;
    }
   }*/
}
