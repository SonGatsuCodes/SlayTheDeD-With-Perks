using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CardCode : MonoBehaviour
{
    public DragAndDrop code;
    //[SerializeField]
    public SortingGroup sortingGroup;
    public void SetVisible(bool visible)
    {
        sortingGroup.sortingOrder=visible?200:0;
    }
}
