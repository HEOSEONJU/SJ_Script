using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Effect : Base_Effect
{
    public int Heal_Value;
    [SerializeField]
    protected bool Multi;//True¸é ±¤¿ª
    public override void Effect_Solo_Function(Manager _Manager)
    {
        

        switch (Multi)
        {
            case true:
                foreach (GameCard GC in _Manager.Char_Manager.CombatChar)
                {
                    if (GC.Live)
                    {
                        GC.Heal(Heal_Value);
                    }
                }
                break;
            default:
                    _Manager.Target_Solo.Heal(Heal_Value);
                    _Manager._Result = Card_Result.Success;
                    break;
                
                

        }
    }
    public override void RequireMent(Manager _Manager)
    {
        switch (Multi)
        {
            case true:
                foreach (GameCard GC in _Manager.Char_Manager.CombatChar)
                {
                    if (GC.Live)
                    {
                        if( GC.Current_HP != GC.Current_MAXHP)
                        {
                            Require = false;
                            return;
                        }
                    }
                }
                break;
                
                
            default:
                if (_Manager.Target_Solo.Current_MAXHP != _Manager.Target_Solo.Current_HP)
                    Require = false;
                    return;

                
        }


                Require = true;
    }
}
