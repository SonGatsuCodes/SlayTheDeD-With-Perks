using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraAdapt : MonoBehaviour
{
    public Camera C;
    public TextMeshProUGUI t;
    // Start is called before the first frame update
    public void setCamera()
    {
        Vector3 v=new Vector3(0,0,0);
        v.x=960;//Screen.width/2;
        v.y=540;//Screen.height/2;
        Info(v);// v.x=1920/2;
        //v.y=1080/2;
        C.orthographicSize=v.y;
        v.z=-500;
        transform.position=v;
    }
    

    // Update is called once per frame
    void Start()
    {

//        setCamera();
    }
    public void Info(Vector3 v)
    {
        t.text=v.ToString();
    
    }
    public void Move(int dir)
    {
        Vector2[] value =new Vector2[4];
    value[0]=new Vector2(-1,-1);
    value[1]=new Vector2(-1,1);
    value[2]=new Vector2(1,-1);
    value[3]=new Vector2(1,1);
    
    Vector3 v=transform.position;
    v.x=v.x+value[dir].x;
    v.y=v.y+value[dir].y;
    Info(v);       
    transform.position=v;    
    }
}
