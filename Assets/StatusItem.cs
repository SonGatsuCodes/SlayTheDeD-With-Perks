using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StatusItem : MonoBehaviour
{
    public StatusControl SC;
    public SpriteRenderer SR;
    public TextMeshPro text,desc;
    public BuffsAndDebuffsData buffs;
    public Collider2D BC2D;
    public void SetData(BuffsAndDebuffsData badd){
       buffs.SetData( badd);
       
    }
    public void updateInfo()
    {
        //int mod=(buffs.positive)?1:-1;
        SR.sprite=buffs.originalInfo.art;//SC.UC.M.DB.GetSprite(buffs.identifier);//db.getimage;
        text.text=(buffs.value).ToString();
        buffs.shower=this;
        if(desc==null){}
        else
        {desc.text=buffs.value.ToString()+' '+buffs.identifier.ToString();}
    }
    public StatusItem show;
    public void OnMouseEnter()
    {
        if(show==null){show=SC.UC.M.ShowStatus;}
        if(show==this){}else{if(show.buffs==buffs){}else{show.SetData(buffs);show.updateInfo();}
        show.gameObject.SetActive(true);}
    }
    public void OnMouseExit ()
    {
        if(show==null){show=SC.UC.M.ShowStatus;}
        show.gameObject.SetActive(false);}
    }

