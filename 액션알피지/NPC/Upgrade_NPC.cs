using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Upgrade_NPC : Normal_NPC
{
    [SerializeField]
    Upgrade_Slot_Vender Upgrade_Function;
    public GameObject Upgrade_UI;
    public override void Init()
    {
        base.Init();
        UI.SetActive(false);
        Upgrade_UI.SetActive(false);
    }
    public override bool Check_UI()
    {
        return (UI.activeSelf || Upgrade_UI.activeSelf);
    }

    

    public void Open_Upgrade_Function()
    {


        Upgrade_UI.SetActive(true);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(true);
        Close_Window();
    }
    
    public void Close_Upgrade_Function()
    {
        Close_ALL_Window();
        


    }
    public override void Close_ALL_Window()
    {
        Upgrade_UI.SetActive(false);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(false);
        base.Close_ALL_Window();
    }
    public void Upgrade_Item()
    {

        
        if(Game_Master.instance.PM.Data.Items[Upgrade_Function.Current_INDEX].Upgrade <Upgrade_Function.MAX_Upgrade)
        {
            
            if(Game_Master.instance.PM._Manager_Inventory.Use_Money(Upgrade_Function.Need_Gold_Point))
            {

                Game_Master.instance.PM.Data.Items[Upgrade_Function.Current_INDEX].Upgrade_Value();
                Upgrade_Function.Renewal_Image();

                Game_Master.instance.UI.Renewal_Gold_Text();
                Game_Master.instance.PM.Data_Save(true);
            }
            else
            {
                //돈부족    
            }




        }
        //강화치최대
    }
}
