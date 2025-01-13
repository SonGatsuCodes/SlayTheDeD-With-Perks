using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ListPositioner : MonoBehaviour
{
    public List<Object> relics;
    public Object prefab;
    public Transform origin;
    /*void Start()
    {
        print(gameObject.name);
    }
    void Update()
    {
        print(this.name);
    }*/
    public List<Object> teste;
    public void test()
    {
        TestShop(teste);
    }
       public void TestShop(List<Object> list)
    {
        GameObject a;
        RelicShower card;
        Vector3 v= origin.position;
        v=new Vector3(v.x+(20-((20/cardperline)*cardperline))*width,v.y-(20/cardperline)*height);// to do: freaking magic numbers         //CardRewardShower.gameObject.SetActive(true);        //int r=0;        //List<int> RandomList=M.PRS.RandomNumberNoRepeat(many,0,list.Count);
        for (int i = 0;         //list.Count 
        relics.Count< list.Count; i++)
        {
            a = Instantiate(prefab.GameObject());
            card = a.GetComponent<RelicShower>();
            relics.Add(card);
        }
        Init(list,v);
    }
    public void Init(List<Object> list,Vector3 v)
    {
        RelicShower card;
        for (int i = 0; i < relics.Count && i<(4*Relicperline); i++)//to do:magic number solve in future
        {
            card = relics[i].GetComponent<RelicShower>();            /*if (i >= many)            {                card.transform.position = new Vector3(-3000, 0, 0);///todo: magic number fix it ....pretty please?            }*/            //else
            {               //r=RandomList[i];
               card.item=list[i].GetComponent<ItemData>();                 //card.code.CIS.info = list[i];
                card.SetArt();                //card.code.CIS.NewInfo();
                card.SG.sortingOrder = 180;
                card.transform.position =new Vector3(v.x+(i-((i/Relicperline)*Relicperline))*widthRelic,v.y-(i/Relicperline)*heightRelic);                 //new Vector3(1920 / 2 + i * 220, 1080 / 2, 0);//todo: magic number fix it 
                card.transform.SetParent(origin, true);
                card.button.classe = classes.BuyRelic;
                card.SetValue(true);
                card.SetValueVisible(true);                //card.code.ri = ri;
            }
        }
    }
    public int width, height,cardperline;
    public int widthRelic, heightRelic,Relicperline;

}
