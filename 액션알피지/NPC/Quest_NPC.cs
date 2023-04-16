using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Quest_NPC : Normal_NPC
{
    [SerializeField]
    const int Max_Quset = 6;
    public List<int> Have_Quest_ID;
    //public Vender_Function Function;
    public Quest_Function QF;
    public GameObject Quest_UI;
    public override void Init()
    {
        base.Init();
        QF.Init(this,Have_Quest_ID);
        
    }

    public override bool Check_UI()
    {
        Debug.Log("Quest_UI" + Quest_UI.activeSelf+"/ UI" + base.Check_UI());
        
        return (base.Check_UI() || Quest_UI.activeSelf);
    }

    public void Open_Quset()
    {
        Game_Master.instance.UI.Close_All_UI();
        Game_Master.instance.UI.Open_UI(Quest_UI);
        QF.Reset_Page();
    }



}
