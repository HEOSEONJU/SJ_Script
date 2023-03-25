


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player_Inventory : MonoBehaviour
{
    [SerializeField]
    Player_Manager _Manager;
    public Equip_Weapon Main_Weapon;
    public List<Equip_Armor> Equip_Armors;
    public List<Inventory_Slot> Item_List;

    int Max_Inventroy_Count = 20;
    public int Inventroy_Count 
    { 
        get { return _Counter_Inventory(); } 
    }

    //public int Gold = 0;



    public GameObject Inventory_Object;
    public GameObject Equip_Object;
    public Transform Slot_Parent;

    public UI_Manager UI;
    //public List<Slot> Equip_Item_Image;
    //public List<Slot> Inventory_Item_Image;





    //public bool Inventory_Open = false;
    //public bool Equip_Open = false;

    public void Init(Player_Manager player_Manager, Player_Data Data)
    {
        _Manager = player_Manager;
        UI = player_Manager._UI;
        //Gold = Data.Gold;
        int i;
        Inventory_Object= UI.transform.GetChild(0).gameObject;
        Equip_Object = UI.transform.GetChild(1).gameObject;
        Slot_Parent = Inventory_Object.transform.GetChild(4);
        
        Main_Weapon = Equip_Object.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Equip_Weapon>();
        if(Data.Equip_Weapon_Item.Base_item!=null)
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
        for (i = 0; i < Data.Items.Count; i++)
        {
            Item_List[i].Insert_Slot_Item(i);
        }
        
        Open_Close_Equip_Window(false);
        Inventory_Object.SetActive(false);
        //Inventory_Open = false;
        UI.Active_Windows(false);
        
        

    }


    public int Current_Gold()
    {
        return _Manager.Call_Data().Gold;
    }


    public void Get_Money(int temp)
    {
        _Manager.Call_Data().Gold+=temp;        
        
    }


    public bool Use_Money(int temp)
    {
        
        if (temp <= _Manager.Call_Data().Gold)
        {
            _Manager.Call_Data().Gold -= temp;
            return true;
        }

        return false;
    }

    public bool Get_Item(Item_Data temp)
    {
        
        
        switch (temp.Base_item.Type)
        {
            case Type.Use:
                foreach(Item_Data ID in _Manager.Call_Data().Items)
                {
                    if (ID.INDEX == temp.Base_item.Item_Index)
                    {
                        Use_Item Check = (Use_Item)ID.Base_item;
                        if (Check.Max_Count >= ID.count + temp.count)
                        {
                            ID.count += temp.count;//이미 가진 소비아이템 합칠 수 있으므로 갯수 증가
                            Load_on_Data(_Manager.Call_Data());//인벤토리 갱신
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
        foreach (Item_Data ID in _Manager.Call_Data().Items)
        {
            if (ID.Base_item == null)
            {
                ID.Insert_Data(temp);//Items에 새로운 데이터 삽입
                Load_on_Data(_Manager.Call_Data());//인벤토리 갱신
                //_Manager.Save_On_FireBase();//얻은내역 업로드
                return true;
            }
        }
        return false;



    }

    int _Counter_Inventory()
    {
        int temp = 0;
        foreach(Item_Data ID in _Manager.Call_Data().Items)
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
                UI.Reseting_Status();
                break;
            default:
                UI.Active_Windows(false);
                break;
        }
        
        //Equip_Open = State;
        Equip_Object.SetActive(State);
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
                Load_on_Data(_Manager.Call_Data());
                UI.Renewal_Gold_Text();
                break;
            default:
                UI.Active_Windows(State);
                
                break;
        }
        Inventory_Object.SetActive(State);
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
    /*
    public void Renewal_Inventroy(Player_Data Data)
    {
        Main_Weapon.Init_Slot_item(Data.Equip_Weapon_Item);
        for(int i=0;i<Equip_Armors.Count;i++)
        {
            Equip_Armors[i].Init_Slot_item(Data.Equip_Armor_Item[i],i);

        }

        for(int i=0;i<Item_List.Count;i++)
        {
            if (Data.Items[i].Base_item == null)
            {
                Item_List[i].Delete_Slot_Item();
            }
            else
            {
                Item_List[i].Insert_Slot_Item(Data.Items[i],i);
            }
        }
    }
    */
}
