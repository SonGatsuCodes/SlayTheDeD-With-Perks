using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New dialog",menuName="things/Dialog")]
public class DialogData : ScriptableObject
{
    public Sprite art,background;
    [TextArea(5,20)]
    public string desc;    
    public List<Choice> choices;
}
