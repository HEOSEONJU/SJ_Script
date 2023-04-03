using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction_NPC : Interaction_Function
{
    [SerializeField]
    Base_NPC _NPC;

    public override int Get_ID()
    {
        return _NPC.ID;
    }


    public override bool Setting_IF(bool v)
    {
        
        return _NPC.Check_UI();
        
    }
    public override void Connect_Object(Player_Manager _Manager)
    {
        _NPC.Connecting_NPC(_Manager);
    }
    public override void DisConnect_Object()
    {
        _NPC.DisConnecting_NPC();
    }
    public override void Function(bool State)
    {


        if(!State)
        {
            Close_Window();
            return;
        }


        Debug.Log(_NPC.Check_UI());
        if (!_NPC.Check_UI())
        {
            Open_Window();
        }
        else
        {
            Close_Window();
        }
    }
    public override void Open_Window()
    {
        
        _NPC.Open_Window();
    }
    public override void Close_Window()
    {
        
        _NPC.Close_ALL_Window();
    }
}
