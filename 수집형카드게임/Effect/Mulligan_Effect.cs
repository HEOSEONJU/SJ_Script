using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mulligan_Effect : Base_Effect
{
    // Start is called before the first frame update
    public override void Effect_Function(Manager _Manager)
    {

        int T = _Manager.Hand.CurrentHand;
        while (_Manager.Hand.CurrentHand > 0)
        {
            _Manager.Hand.ThrowCard(_Manager.Hand.Cards[0]);
        }
        
        

        for (int i = 0; i < T; i++)
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
