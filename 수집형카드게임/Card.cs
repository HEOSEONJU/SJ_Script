using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Card : MonoBehaviour
{
    public string Name;
    public int CardNumber;
    //public GameObject CardImage;
    public float percent;
    public PRS originPosi;
    
    public void InitCard()
    {
        //var e =Instantiate(Cardimage);
        //e.transform.parent = transform;
        //e.transform.localPosition = Vector3.zero;
        //e.transform.localScale = Vector3.one;
        //e.transform.localRotation= Quaternion.identity;
        

    }
    

    public void MoveTransForm(PRS prs,bool useDotween,float dotweenTime=0)
    {
        if(useDotween)
        {
            

            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }
}
