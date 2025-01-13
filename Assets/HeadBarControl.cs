using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBarControl : MonoBehaviour
{
    public List<FakeButton> allbuttons;
    // Start is called before the first frame update
    public FakeButton button(buttonNames i)
    {
        return allbuttons[(int)i];
    }
    public void SetOneShining(buttonNames i)
    {
        foreach(FakeButton b in allbuttons)
        {
            b.StartShining(false);
        }
        allbuttons[(int)i].StartShining(true);
    }
}
public enum buttonNames{reward,map,deck,battle,shop,health,money,dialogue,soul,team}
