using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class Quest_Basic :MonoBehaviour
{
    
    public Quest_Type QUEST_TYPE;
    
    public string Quest_Description;//이번 단계의설명





    

    public virtual bool Complete_Check(int point = 0)
    {
        return false;
    }
    public virtual float Progress_float(int point)
    {
        return point;
    }
   public virtual void Hunt_Check(int Monster_ID, int Current_point, out int Result_point)
    {
        Result_point = Current_point;
        return;
    }
    public virtual bool Talk_Check(int NPC_ID)
    {
        return false;
    }
    public virtual bool Give_Check(int Item_ID)
    {
        return false;
    }
    public virtual string Return_Value()
    {

        return "";
    }

}

public enum Quest_Type//퀘스트 타입 
{
    Hunt,//사냥퀘스트
    Talk,//대화형퀘스트
    Give    //상납형퀘스트
}
