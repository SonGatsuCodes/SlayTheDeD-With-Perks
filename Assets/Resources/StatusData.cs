using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "00-status", menuName = "things/StatusInfo")]
public class StatusData : ScriptableObject
{
    public BuffsAndDebuffs indentifier;
    public Trigger activator;
    public bool buff;
    public Sprite art;

}
