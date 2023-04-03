using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Normal_NPC : Base_NPC
{
    public override int ID
    {
        get { return NPC_ID; }
    }

    [SerializeField]
    TextMeshProUGUI Text;
    [SerializeField]
    string Start_Text;

    [SerializeField]
    string Default_Text = "µü È÷ ÇÒ ¾ê±â´Â ¾ø´Â°É";

    [SerializeField]
    string Special;
    [SerializeField]
    TextMeshProUGUI Special_Text;
    public override void Init()
    {
        MY_Position = transform;
        

        
        Special_Text.text = Special;

    }

    public override bool Check_UI()
    {
        Debug.Log("Normal_UI" + UI.activeSelf);
        return UI.activeSelf;
    }

    public override void Connecting_NPC(Player_Manager MANAGER)
    {
        _manager = MANAGER;
    }
    public override void DisConnecting_NPC()
    {
        UI.SetActive(false);
        

    }

    public override void Open_Window()
    {
        if (Check_UI())
            return;
        UI.SetActive(true);
        Text.text = Start_Text;
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(false);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Equip_Window(false);
    }

    public override void Close_Window()
    {
        
        UI.SetActive(false);
        

    }

    public override void Close_ALL_Window()
    {
        Close_Window();
        if (_manager != null)
            _manager._Connect_Object.Make_IF_NULL();
        
    }
    public void Talk()
    {
        string s = Game_Master.Instance.PM.PQB.Call_Talk_Script(ID);
        if(s==null)
        {
            Text.text = Default_Text;
            return;
        }
        Text.text = s;
    }
}
