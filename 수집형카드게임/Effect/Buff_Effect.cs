using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Buff_Effect : Base_Effect
{
    [SerializeField]
    protected bool Multi;//True면 광역
    [SerializeField]
    public List<Effect_Value> _Effect_Value;    //버프종류
    List<int> Skip;//이미같은 버프를 받아 받지않아도 되는대상
    public override void Effect_Solo_Function(Manager _Manager)
    {
        switch (Multi)
        {
            case true:
                for(int i=0;i< _Manager.Char_Manager.CombatChar.Count;i++)//각 캐릭터들에게 적용
                {
                    if (Skip.Contains(i) || _Manager.Char_Manager.CombatChar[i].Live)//죽었거나 스킵대상이면 통과
                        continue;
                    Function(_Manager, _Manager.Char_Manager.CombatChar[i]);//적용
                }
                Skip.Clear();
                break;
            default:
                Function(_Manager,_Manager.Target_Solo);//적용
                break;
        }
            _Manager._Result = Card_Result.Success;//결과성공
        
    }

    public void Function(Manager _Manager, GameCard GC)
    {
        SpellEffect E = new SpellEffect();
        E.ClearValue();
        foreach (Effect_Value v in _Effect_Value)
        {
            E.Effect_Type_Value.Add(v);
        }
        if (_Manager.SelectedCard != null)//스펠카드
        {
            GC.Active_Effect(_Manager.SelectedCard.Value_Char_Effect_Num);
            E.Init(_Manager.SelectedCard.CardNumber);
            GC.Spell_Effects.Add(E);
        }
        else//캐릭터패시브 스킬
        {
            E.Init(_Manager.Char_Manager.Skill_NUM);
            GC.Skill_Effects.Add(E);
        }
    }

    public override void RequireMent(Manager _Manager)
    {
        Skip=new List<int>();
        switch (Multi)
        {
            case true:
                int L = 0;//생존수
                int D = 0;//중복

                for(int i=0;i<_Manager.Char_Manager.CombatChar.Count;i++)
                {
                    if (_Manager.Char_Manager.CombatChar[i].Live)//생존해있다면
                    {
                        L++;
                        if(_Manager.Char_Manager.CombatChar[i].Spell_Effects.Find(x => x.ID == _Manager.SelectedCard.CardNumber)!=null)
                        {
                            D++;
                            Skip.Add(i);
                            break;
                        }
                        
                    }
                }
                
                if(L==D)//생존중복이같다는건 모두 같은버프를갖고있음
                {
                    _Manager._Result = Card_Result.Duplication;
                    Require = false;
                }
                break;
            default:

                if(_Manager.Target_Solo.Spell_Effects.Find(x=>x.ID==_Manager.SelectedCard.CardNumber)!=null)
                {
                    _Manager._Result = Card_Result.Duplication;
                    Require = false;
                }
                
                break;
        }
        Require = true;
    }
}