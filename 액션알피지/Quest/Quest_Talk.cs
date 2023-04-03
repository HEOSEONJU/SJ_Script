using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class Quest_Talk : Quest_Basic
{
    
    public List<int> Talk_Quest_List;

    public List<string> Talk_Value_List;


    protected override void Complete_Check()
    {

        if(Progress==Talk_Quest_List.Count-1)
        {
            Complete= true;
            return;
        }

        Complete = false;
    }
    public override float Progress_float()
    {
        return ((Progress*1f) / Talk_Quest_List.Count)*100f;
    }
    public string Return_Value()
    {
        Complete_Check();
        return Talk_Value_List[Progress++];
    }

    public bool Talk_Check(int NPC_ID)
    {
        if (Progress == Talk_Quest_List.Count)
        {
            return false;
        }

        if (NPC_ID == Talk_Quest_List[Progress])
        {
            return true;
        }
        return false;
    }
}
