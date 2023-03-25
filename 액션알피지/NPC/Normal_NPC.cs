using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Normal_NPC : Base_NPC
{
    // Start is called before the first frame update
    public override void Connecting_NPC(Player_Manager MANAGER)
    {
        _manager = MANAGER;
    }
    public override void DisConnecting_NPC()
    {
        Close_Window();
        _manager = null;
    }
    public override void Get_ID()
    {
        Object_ID = gameObject.GetInstanceID();
    }
    public override void Open_Window()
    {
        UI.SetActive(true);
    }

    public override void Close_Window()
    {
        _manager._Connect_Object.Close_NPC();
        _manager.Check_UI();
        UI.SetActive(false);




    }
}
