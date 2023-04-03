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
        
        
        Vender_UI.SetActive(true);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(true);
        Close_Window();
    }

    public void Close_Vender_Function()
    {
        Close_ALL_Window();
        



    }
    public override void Close_ALL_Window()
    {
        Vender_UI.SetActive(false);
        Function.Sell_Window.SetActive(false);
        Function.Buy_Window.SetActive(false);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(false);
        base.Close_ALL_Window();
    }

}
