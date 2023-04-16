using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Quest_Talk : Quest_Basic
{
    
    public int Talk_Quest_Object_ID;

    public string Talk_Value_List;

    public override bool Complete_Check(int point=0)
    {
        if(point==1)
        return true;
        else
        {
            return false;
        }

    }
    public override float Progress_float(int point)
    {

        
            return point;

    }
    public override string Return_Value()
    {

        return Talk_Value_List;
    }

    public override bool Talk_Check(int NPC_ID)
    {


        if (NPC_ID == Talk_Quest_Object_ID)
        {
            
            return true;
        }
        return false;
    }
}
