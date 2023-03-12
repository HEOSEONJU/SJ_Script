using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_NPC : Normal_NPC
{
    [SerializeField]
    Upgrade_Slot_Vender Upgrade_Function;

    
    public void Start()
    {
        MY_Position = transform;
        Get_ID();
    }
    
    
    public override void Close_Window()
    {
        _manager._Connect_NPC.Close_NPC();
        _manager.Check_UI();
        Upgrade_Function.Remove_Slot_Item();
        UI.SetActive(false);




    }

    public void Upgrade_Item()
    {

        
        if(_manager.Call_Data().Items[Upgrade_Function.Current_INDEX].Upgrade <Upgrade_Function.MAX_Upgrade)
        {
            
            if(_manager._Manager_Inventory.Use_Money(Upgrade_Function.Need_Gold_Point))
            {
                
                _manager.Call_Data().Items[Upgrade_Function.Current_INDEX].Upgrade_Value();
                Upgrade_Function.Renewal_Image();
                
                _manager._UI.Renewal_Gold_Text();
            }
            else
            {
                //������    
            }




        }
        //��ȭġ�ִ�
    }
}
