using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sacrifice_Effect : Base_Effect
{
    [SerializeField]
    bool Ran;
    [SerializeField]
    int Damage_Value;
    public override void Effect_Function(Manager _Manager)
    {
        if(Ran)
        {
            List<int> ints= new List<int>();
            for(int i=0;i< _Manager.Char_Manager.CombatChar.Count;i++)
            {
                if(_Manager.Char_Manager.CombatChar[i].Live) ints.Add(i);
            }
            //�������� 1������ ��ɸ�������
            _Manager.Char_Manager.CombatChar[0].Spell_Damaged(Damage_Value);
        }
        else
        {
            Debug.Log("���ϵ�����");
            _Manager.Target_Solo.Spell_Damaged(Damage_Value,_Manager.SelectedCard.Value_Char_Effect_Num);
        }


    }
    public override void RequireMent(Manager _Manager)
    {
        Debug.Log("/");
        switch (Ran)
        {
            case true:

                break;

            default:
                if (!_Manager.Target_Solo.Live)
                {
                    Debug.Log(_Manager.Target_Solo.Live+"����");
                    _Manager._Result = Card_Result.Char_Die;
                    Require = false;
                    return;
                }

                break;

        }


        Require = true;
    }

}
