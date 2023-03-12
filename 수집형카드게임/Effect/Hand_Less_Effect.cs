using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.XR;

public class Hand_Less_Effect : Base_Effect
{
    public int Less_Count;//0이면 전부버리기

    public override void Effect_Function(Manager _Manager)
    {
        if(Less_Count== 0)
        {
            
            while(_Manager.Hand.CurrentHand>0)
            {
                _Manager.Hand.ThrowCard(_Manager.Hand.Cards[0]);
            }
            _Manager._Result = Card_Result.Success;
            return;
        }
        
        if (Less_Count >= _Manager.Hand.CurrentHand)
        {
            Less_Count = _Manager.Hand.CurrentHand;
        }

        for (int i = 0; i < Less_Count; i++)
        {
            int Ran = Random.Range(0, _Manager.Hand.CurrentHand);
            _Manager.Hand.ThrowCard(_Manager.Hand.Cards[Ran]);

        }


        _Manager._Result = Card_Result.Success;
    }
    public override void RequireMent(Manager _Manager)
    {
        if (Less_Count >= _Manager.Hand.CurrentHand)
        {
            Require = false;
            return;
        }
        Require = true;
    }
}
