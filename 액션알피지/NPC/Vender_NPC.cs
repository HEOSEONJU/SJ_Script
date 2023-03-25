using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Vender_NPC: Normal_NPC
{

    public Vender_Function Function;
    //상점 펑션추가
    public void Awake()
    {
        MY_Position = transform;
        Function.Init(this);
        Get_ID();

    }
    public override void Close_Window()
    {
        Function.Sell_Window.SetActive(false);
        Function.Buy_Window.SetActive(false);
        _manager._Connect_Object.Close_NPC();
        _manager.Check_UI();
        UI.SetActive(false);
    }

    public void Sell_Item()
    {

    }
    public void Purchase_Item()
    {


    }
}
