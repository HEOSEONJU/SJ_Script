using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Drew_Effect : Base_Effect
{
    public int Drew_Count;

    public override void Effect_Function(Manager _Manager)
    {
        
        
        

        for (int i = 0; i < Drew_Count; i++)
        {
            if (_Manager.MYDeck.Drew())
            {
                _Manager.CardAlignment();
            }
        }
        
        _Manager._Result = Card_Result.Success;
    }
    public override void RequireMent(Manager _Manager)
    {
        if (_Manager.MYDeck.DeckCards.Count <= 0)
        {
            _Manager._Result = Card_Result.CantDrew;
            Require = false;
            return;

        }
        Require = true;
    }
}
