using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Buff_Effect : Base_Effect
{
    [SerializeField]
    protected bool Multi;//True�� ����
    [SerializeField]
    public List<Effect_Value> _Effect_Value;    //��������
    List<int> Skip;//�̹̰��� ������ �޾� �����ʾƵ� �Ǵ´��
    public override void Effect_Solo_Function(Manager _Manager)
    {
        switch (Multi)
        {
            case true:
                for(int i=0;i< _Manager.Char_Manager.CombatChar.Count;i++)//�� ĳ���͵鿡�� ����
                {
                    if (Skip.Contains(i) || _Manager.Char_Manager.CombatChar[i].Live)//�׾��ų� ��ŵ����̸� ���
                        continue;
                    Function(_Manager, _Manager.Char_Manager.CombatChar[i]);//����
                }
                Skip.Clear();
                break;
            default:
                Function(_Manager,_Manager.Target_Solo);//����
                break;
        }
            _Manager._Result = Card_Result.Success;//�������
        
    }

    public void Function(Manager _Manager, GameCard GC)
    {
        SpellEffect E = new SpellEffect();
        E.ClearValue();
        foreach (Effect_Value v in _Effect_Value)
        {
            E.Effect_Type_Value.Add(v);
        }
        if (_Manager.SelectedCard != null)//����ī��
        {
            GC.Active_Effect(_Manager.SelectedCard.Value_Char_Effect_Num);
            E.Init(_Manager.SelectedCard.CardNumber);
            GC.Spell_Effects.Add(E);
        }
        else//ĳ�����нú� ��ų
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
                int L = 0;//������
                int D = 0;//�ߺ�

                for(int i=0;i<_Manager.Char_Manager.CombatChar.Count;i++)
                {
                    if (_Manager.Char_Manager.CombatChar[i].Live)//�������ִٸ�
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
                
                if(L==D)//�����ߺ��̰��ٴ°� ��� ������������������
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