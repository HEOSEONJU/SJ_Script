using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Quest_Hunt : Quest_Basic
{
    
    public int Hunt_ID;
    [SerializeField]
    int Hunt_Count;
    

    public override float Progress_float()
    {
        return ((Progress * 1f) / Hunt_Count)*100f;
    }
    protected override  void Complete_Check()
    {
        Debug.Log(Progress + "/" + Hunt_Count);
        if(Progress == Hunt_Count)
        {
            Complete = true;
            return;
        }

        Complete= false;
    }

    public void Hunt_Check(int Monster_ID)
    {
        if(Complete)
        {
            return;
        }

        if(Monster_ID==Hunt_ID)
        {
            Progress++;
            Complete_Check();
        }
    }
}
