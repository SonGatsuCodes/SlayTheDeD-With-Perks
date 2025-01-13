using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimationManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool Done = false;
    public Manager M;
    public Anime test;
    public UnitControl subject;
    public List<Chapter> movie;
  /*  public void TestAnime()
    {
        StartCoroutine(subject.AnimeSolver(test,Vector3.one,Vector3.one));
    }
*/    
    public IEnumerator Anima(Anime a,UnitControl uc,Vector3 target,Vector3 scale)
    {
        //if(a==null)        {a=M.DB.standard.anime;}
        yield return null; 
        //StartCoroutine(uc.AnimeSolver(a,target,scale));
        /*if(movie.Count>1)
        {
         //setnextchapter()
            //movie.Clear();   
        }
        else
        {
            //setnextChapter();
            //removePlayed();
        }*/
    }
    /*
    public Vector3 MultiVector(Vector3 a, Vector3 b)
    {
        Vector3 v = Vector3.zero;
        v.x = a.x * b.x;
        v.y = a.y * b.y;
        v.z = a.z * b.z;
        return v;
    }*/
    /*public IEnumerator AnimeSolver(Anime a, UnitControl sourceUC,Vector3 targetUC)
    {
        if(a==null){a=test;}
        Frame actual;
        SpriteRenderer ani = sourceUC.animador;
        Vector3 originalPosition = ani.transform.position, originalScale = ani.transform.localScale
        , v = Vector3.zero, v1 = Vector3.zero, s = Vector3.zero, s1 = Vector3.zero,old=originalPosition;
        Vector4 c = Vector4.one, c1 = Vector4.one;
        float limit, progress;
        for (int i = 0; i < a.allFrames.Count; i++)
        {
            actual = a.allFrames[i];
            limit = Time.time + actual.time;
            originalPosition=(actual.UseTargetPosi?targetUC:old);
            if (i == 0)
            {
                ani.sprite = actual.art;
                v = ani.transform.position; v1 = originalPosition + actual.position;
                s = ani.transform.localScale; s1 = MultiVector(originalScale, actual.scale);
                c = ani.color; c1 = actual.cor;
            }
            else
            {
                if (ani.sprite != actual.art) { ani.sprite = actual.art; }
                if (ani.transform.position != originalPosition + actual.position) { v = ani.transform.position; v1 = originalPosition + actual.position; }
                if (ani.transform.localScale != MultiVector(originalScale, actual.scale)) { s = ani.transform.localScale; s1 = MultiVector(originalScale, actual.scale); }
                if (ani.color != actual.cor) { c = ani.color; c1 = actual.cor; }
            }
            while (Time.time < limit)
            {
                //f = Time.time;
                progress = Mathf.Clamp01(1 - ((limit - Time.time) / actual.time));
                ani.transform.position = Vector3.Lerp(v, v1, progress);
                ani.transform.localScale = Vector3.Lerp(s, s1, progress);
                ani.color = Vector4.Lerp(c, c1, progress);
                yield return null;//new WaitForSeconds(a.allFrames[i].time);
            }

        }
        ani.sprite = null;
        ani.transform.position = old;
        ani.transform.localScale = originalScale;
        ani.color = Color.white;
    }*/
}

[Serializable]
public class Chapter
{
    public Anime anime;
    public UnitControl actor;
    public Vector3 target;
}
[Serializable]
public class Anime
{
    public List<Frame> allFrames;
}
[Serializable]
public class Frame
{
    public Sprite art = null;
    public float time = .1f;
    public Vector3 position = Vector3.zero, scale = Vector3.one;
    public Color cor = Color.white;
    public bool UseTargetPosi,ScaleUseTarget;
}
//public enum UsePosi { none, Self, Target }