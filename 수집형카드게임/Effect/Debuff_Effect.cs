using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff_Effect : Base_Effect
{
    [SerializeField]
    public List<Effect_Value> _Effect_Value;

    public override void Effect_Enemy_Function(Manager _Manager)
    {
        
        //GC.Active_Effect(_Manager.SelectedCard.Value_Char_Effect_Num);
        SpellEffect E = new SpellEffect();
        E.ClearValue();
        foreach (Effect_Value v in _Effect_Value)
        {
            E.Effect_Type_Value.Add(v);
        }
        if (_Manager.SelectedCard != null)
        {
            //_Manager.Enemy_Manager. .Active_Effect(_Manager.SelectedCard.Value_Char_Effect_Num);//디버프맞았을때 이펙트출력문추가하기
            E.Init(_Manager.SelectedCard.CardNumber);
            _Manager.Enemy_Manager.Spell_Effects_List.Add(E);
            
        }
        else
        {
            E.Init(_Manager.Char_Manager.Skill_NUM);
            _Manager.Enemy_Manager.Skill_Effects_List.Add(E);
        }

        
        
        
        _Manager._Result = Card_Result.Success;
    }
    public override void RequireMent(Manager _Manager)
    {

        if (_Manager.Enemy_Manager.Spell_Effects_List.Find(x => x.ID == _Manager.SelectedCard.CardNumber) != null)
        {
            _Manager._Result = Card_Result.Duplication;
            Require = false;
            return;
        }
        



        Require = true;
    }
}
