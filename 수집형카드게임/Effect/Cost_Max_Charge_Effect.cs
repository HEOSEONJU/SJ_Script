using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Cost_Max_Charge_Effect : Base_Effect
{
    [SerializeField]
    int Charge_Value;
    public override void Effect_Function(Manager _Manager)
    {

        
        if(_Manager.My_MAXCost == 10)
        {
            for (int i = 0; i < Charge_Value; i++)
            {
                if (_Manager.MYDeck.Drew())
                {
                    _Manager.CardAlignment();
                }
            }
            
        }
        else
        {


            _Manager.My_MAXCost += Charge_Value;
            if (_Manager.My_MAXCost > 10)
            {
                _Manager.My_MAXCost = 10;
            }
            



        }

        
        _Manager._Result = Card_Result.Success;

    }
    public override void RequireMent(Manager _Manager)
    {
        if (_Manager.My_MAXCost == 10 && _Manager.MYDeck.DeckCards.Count <= 0)//코스트최대 덱에카드가1장이상
        {
            _Manager._Result = Card_Result.CantDrew;
            Require = false;
            return;

        }
        Require = true;
    }
}