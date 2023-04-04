using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest_Hunt : Quest_Basic
{
    
    public int Hunt_ID;
    [SerializeField]
    int Hunt_Count;

    

    public override float Progress_float(int point)
    {
        return ((point * 1f) / Hunt_Count)*100f;
    }
    public override  bool Complete_Check(int point=0)
    {
        //Debug.Log(Current_Hunt + "/" + Hunt_Count);
        if(point == Hunt_Count)
        {
            return true;
        }

        return false;
    }

    public override void  Hunt_Check(int Monster_ID,int Current_point, out int Result_point)
    {
        Result_point = Current_point;
        if (Complete_Check(Current_point))
        {
            
            return;
        }

        if(Monster_ID==Hunt_ID)
        {
            Result_point = Current_point + 1;
            
        }
    }
}
