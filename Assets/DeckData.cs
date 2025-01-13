using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="/Asset/Deck/New Deck",menuName="things/Decks")]

public class DeckData : ScriptableObject
{
    public List<CardData> all;
}
