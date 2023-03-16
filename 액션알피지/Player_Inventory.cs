


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Inventory : MonoBehaviour
{
    [SerializeField]
    Player_Manager _Manager;
    public Equip_Weapon Main_Weapon;
    public List<Equip_Armor> Equip_Armors;
    public List<Slot> Item_List;

    int Max_Inventroy_Count = 20;
    int Inventroy_Count;
    

    //public int Gold = 0;

    

    public GameObject Inventory_Object;
    public GameObject Equip_Object;
    public Transform Slot_Parent;

    UI_Manager UI;
    //public List<Slot> Equip_Item_Image;
    //public List<Slot> Inventory_Item_Image;





    public bool Inventory_Open = false;
    public bool Equip_Open = false;

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
            Main_Weapon.Init_Slot_item(Data.Equip_Weapon_Item);   
        }

        for (i = 1; i < 4; i++)
        {
            Equip_Armors.Add(Equip_Object.transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Equip_Armor>());
        }
        for (i = 0; i < Equip_Armors.Count; i++)
        {
            Equip_Armors[i].Init_Slot_item(Data.Equip_Armor_Item[i],i);
        }
        for (i = 0; i < Max_Inventroy_Count; i++)
        {
            Item_List.Add(Slot_Parent.GetChild(i).GetComponentInChildren<Slot>());
        }
        for (i = 0; i < Data.Items.Count; i++)
        {
            Item_List[i].Insert_Slot_Item(Data.Items[i],i);
        }
        
        Open_Close_Equip_Window(false);
        Inventory_Object.SetActive(false);
        Inventory_Open = false;
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
        
        Debug.Log("2");
        switch (temp.Base_item.Type)
        {
            case Type.Use:
                for (int i = 0; i < Max_Inventroy_Count; i++)
                {
                    if (_Manager.Call_Data().Items[i].INDEX== temp.Base_item.Item_Index)
                    {
                        Use_Item Check = (Use_Item)_Manager.Call_Data().Items[i].Base_item;
                        if (Check.Max_Count >= _Manager.Call_Data().Items[i].count + temp.count)
                        {
                            _Manager.Call_Data().Items[i].count += temp.count;
                            Load_on_Data(_Manager.Call_Data());
                            return true;
                        }
                    }
                }
                break;
        }
        _Counter_Inventory();

        if (Inventroy_Count >= 20)
        {
            return false;
        }

        Debug.Log("3");
        for (int i = 0; i < Max_Inventroy_Count; i++)
        {
            if (_Manager.Call_Data().Items[i].Base_item == null)
            {
                Debug.Log("4");

                _Manager.Call_Data().Items[i].Insert_Data(temp);
                Load_on_Data(_Manager.Call_Data());
                return true;
            }
        }
        return false;



    }


    void _Counter_Inventory()
    {
        int temp = 0;
        for (int i = 0; i < Max_Inventroy_Count; i++)
        {
            if (_Manager.Call_Data().Items[i].Base_item != null)
                temp++;

        }
        Debug.Log("Counter_Inven: " + temp);
        Inventroy_Count = temp;
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
        
        Equip_Open = State;
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
                _Manager.Data_Save(true);
                break;
        }
        Inventory_Object.SetActive(State);
        Inventory_Open = State;
        //Renewal_Inventroy(_Manager.Call_Data());
    }



    public void Load_on_Data(Player_Data Data)
    {
        Main_Weapon.Init_Slot_item(Data.Equip_Weapon_Item);
        for (int i = 0; i < Equip_Armors.Count; i++)
        {
            Equip_Armors[i].Init_Slot_item(Data.Equip_Armor_Item[i],i);

        }

        for (int i = 0; i < Item_List.Count; i++)
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
