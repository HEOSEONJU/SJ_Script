using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Vender_NPC: Normal_NPC
{
    public Vender_Function Function;
    public GameObject Vender_UI;
    //상점 펑션추가
    public override void Init()
    {
        base.Init();
        Function.Init(this);
    }
    public override bool Check_UI()
    {
        return (UI.activeSelf || Vender_UI.activeSelf);
    }

    public void Open_Vender_Function()
    {
        Game_Master.Instance.UI.Close_All_UI();
        Game_Master.Instance.UI.Open_UI(Vender_UI);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(true);
    }
}
