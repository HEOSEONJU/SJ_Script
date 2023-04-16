
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    
    public Equip_Weapon Main_Weapon;
    public List<Equip_Armor> Equip_Armors;
    public List<Inventory_Slot> Item_List;

    int Max_Inventroy_Count = 20;
    public int Inventroy_Count 
    { 
        get { return _Counter_Inventory(); } 
    }

    



    public GameObject Inventory_Object;
    public GameObject Equip_Object;
    public Transform Slot_Parent;

    
    public void Init()
    {
        
        int i;
        Inventory_Object= Game_Master.instance.UI.transform.GetChild(0).gameObject;
        Equip_Object = Game_Master.instance.UI.transform.GetChild(1).gameObject;
        Slot_Parent = Inventory_Object.transform.GetChild(4);
        
        Main_Weapon = Equip_Object.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Equip_Weapon>();
        if(Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item!=null)
        {
            Main_Weapon.Init_Slot_item();   
        }

        for (i = 1; i < 4; i++)
        {
            Equip_Armors.Add(Equip_Object.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Equip_Armor>());
        }
        for (i = 0; i < Equip_Armors.Count; i++)
        {
            Equip_Armors[i].Init_Slot_item(i);
        }
        for (i = 0; i < Max_Inventroy_Count; i++)
        {
            Item_List.Add(Slot_Parent.GetChild(i).GetComponentInChildren<Inventory_Slot>());
        }
        for (i = 0; i < Game_Master.instance.PM.Data.Items.Count; i++)
        {
            Item_List[i].Insert_Slot_Item(i);
        }
        
        Open_Close_Equip_Window(false);
        Game_Master.instance.UI.Close_UI(Inventory_Object);
        //Inventory_Open = false;
        Game_Master.instance.UI.Close_Small_Windows();
        
        

    }




    


    public bool Use_Money(int temp)
    {
        
        if (temp <= Game_Master.instance.PM.Data.Current_Gold)
        {
            Game_Master.instance.PM.Data.Current_Gold -= temp;
            return true;
        }

        return false;
    }

    public bool Get_Item(Item_Data temp)
    {
        
        if(temp==null)
        {
            return false;
        }
        


        switch (temp.Base_item.Type)
        {
            case Item_Enum.EItem_Slot_Type.Use:
                foreach(Item_Data ID in Game_Master.instance.PM.Data.Items)
                {
                    if (ID.INDEX == temp.Base_item.Item_Index)
                    {
                        if ((ID.Base_item as Use_Item).Max_Count >= ID.count + temp.count)
                        {
                            ID.count += temp.count;//이미 가진 소비아이템 합칠 수 있으므로 갯수 증가
                            
                            Load_on_Data(Game_Master.instance.PM.Data);//인벤토리 갱신
                            //_Manager.Save_On_FireBase();//얻은내역 업로드
                            return true;
                        }
                    }
                }
                
                break;
        }
        

        if (Inventroy_Count >= 20)
        {
            return false;
        }
        foreach (Item_Data ID in Game_Master.instance.PM.Data.Items)
        {
            if (ID.Base_item == null)
            {
                ID.Insert_Data(temp);//Items에 새로운 데이터 삽입
                Load_on_Data(Game_Master.instance.PM.Data);//인벤토리 갱신
                //_Manager.Save_On_FireBase();//얻은내역 업로드
                return true;
            }
        }
        return false;



    }

    int _Counter_Inventory()
    {
        int temp = 0;
        foreach(Item_Data ID in Game_Master.instance.PM.Data.Items)
        {
            if (ID.Base_item != null)   temp++;
        }
        
        return  temp;
    }
    



    public void Open_Close_Equip_Window(bool State)
    {
        if (Equip_Object == null)
        {
            return;
        }
        switch(State)
        {
            case true:
                Game_Master.instance.UI.Reseting_Status();
                Game_Master.instance.UI.Open_UI(Equip_Object);
                break;
            default:
                Game_Master.instance.UI.Close_Small_Windows();
                Game_Master.instance.UI.Close_UI(Equip_Object);
                break;
        }

        
    }

    public void Open_Close_Inventory_Window(bool State)
    {
        if (Inventory_Object == null)
        {
            return;
        }

        switch (State)
        {
            case true:
                Load_on_Data(Game_Master.instance.PM.Data);
                Game_Master.instance.UI.Renewal_Gold_Text();
                Game_Master.instance.UI.Open_UI(Inventory_Object);
                break;
            default:

                Game_Master.instance.UI.Close_Small_Windows();
                Game_Master.instance.UI.Close_UI(Inventory_Object);
                break;
        }
        
        //Inventory_Open = State;
        //Renewal_Inventroy(_Manager.Call_Data());
    }



    public void Load_on_Data(Player_Data Data)
    {
        
        Main_Weapon.Init_Slot_item();
        for (int i = 0; i < Equip_Armors.Count; i++)
        {
            Equip_Armors[i].Init_Slot_item(i);

        }

        for (int i = 0; i < Item_List.Count; i++)
        {
            
            Item_List[i].Insert_Slot_Item(i);
            
        }
    }

}
