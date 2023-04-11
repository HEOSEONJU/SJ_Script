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
        Game_Master.Instance.UI.Close_All_UI();
        Game_Master.Instance.UI.Open_UI(Upgrade_UI);
        Game_Master.Instance.PM._Manager_Inventory.Open_Close_Inventory_Window(true);
        
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
