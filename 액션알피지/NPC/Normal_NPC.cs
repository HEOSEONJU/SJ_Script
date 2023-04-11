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

    
    public TextMeshProUGUI Text;
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
        //Debug.Log("Normal_UI" + UI.activeSelf);
        return UI.activeSelf;
    }

    public override void Connecting_NPC(Player_Manager MANAGER)
    {
        _manager = MANAGER;
    }
    public override void DisConnecting_NPC()
    {
        Game_Master.instance.UI.Close_All_UI();
    }

    public override void Open_Window()
    {
        if (Check_UI())
            return;
        Game_Master.instance.UI.Close_All_UI();
        Game_Master.instance.UI.Open_UI(UI);
        Text.text = Start_Text;
        
    }

    public override void Close_Window()
    {
        Game_Master.instance.PM._Connect_Object.DisConnect_Object();
    }

    public void Talk()//¹öÅ÷À¸·Î±¸Çö
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
