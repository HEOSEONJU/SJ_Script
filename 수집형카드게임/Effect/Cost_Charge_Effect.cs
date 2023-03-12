using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cost_Charge_Effect : Base_Effect
{
    [SerializeField]
    int Charge_Value;
    public override void Effect_Function(Manager _Manager)
    {
        

        _Manager.My_Cost += Charge_Value;
        if (_Manager.My_Cost >= _Manager.My_MAXCost)
        {
            _Manager.My_Cost = _Manager.My_MAXCost;
        }
        Debug.Log("코스트회복");
        _Manager._Result = Card_Result.Success;

    }
    public override void RequireMent(Manager _Manager)
    {
        if ((_Manager.My_Cost == _Manager.My_MAXCost))//코스트회복못하는상황
        {
            _Manager._Result = Card_Result.CantCharge_Cost;
            Require = false;
            return;

        }
        Require = true;
    }
}
