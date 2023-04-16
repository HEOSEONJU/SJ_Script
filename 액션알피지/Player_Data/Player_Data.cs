using Quests;
using Item_Enum;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Data")]
public class Player_Data : ScriptableObject
{


    public Item_Data Equip_Weapon_Item;
    public List<Item_Data> Equip_Armor_Item;
    public List<Item_Data> Items;

    public int Gold;
    public int Current_Gold
    {
        get { return Gold; }
        set { Gold = value; }
    }


    public Vector3 Last_Position;


    public List<Quest_Info> Accepted_Quest;
    public List<int> Complted_Quest;




    public void Swap_Items_WtoI(int INDEX_Arrival)//Weapon->>ETC
    {
        if (Items[INDEX_Arrival].Base_item == null)
        {
            Items[INDEX_Arrival].Base_item = Equip_Weapon_Item.Base_item;
            Items[INDEX_Arrival].Upgrade = Equip_Weapon_Item.Upgrade;
            Items[INDEX_Arrival].count = Equip_Weapon_Item.count;

            Equip_Weapon_Item.Delete_Data();

        }
        else
        {
            
            if (Items[INDEX_Arrival].Base_item.Type!=EItem_Slot_Type.Weapon)
            {
                return;
            }

            Item_Data temp = Items[INDEX_Arrival];
            Items[INDEX_Arrival] = Equip_Weapon_Item;
            Equip_Weapon_Item = temp;
        }


        Game_Master.instance.PM._weaponSlotManager.Change_Weapon_Prefab();
    }

    public void Swap_Items_ItoW(int INDEX_Arrival)//ETC->Weapon
    {


        if (Equip_Weapon_Item.Base_item == null)
        {

            Equip_Weapon_Item.Base_item = Items[INDEX_Arrival].Base_item;
            Equip_Weapon_Item.Upgrade = Items[INDEX_Arrival].Upgrade;
            Equip_Weapon_Item.count = Items[INDEX_Arrival].count;

            Items[INDEX_Arrival].Delete_Data();

        }
        else
        {
            if (Items[INDEX_Arrival].Base_item.Type!= EItem_Slot_Type.Weapon)
            {
                return;
            }

            Item_Data temp = Items[INDEX_Arrival];
            Items[INDEX_Arrival] = Equip_Weapon_Item;
            Equip_Weapon_Item = temp;
        }











        Game_Master.instance.PM._weaponSlotManager.Change_Weapon_Prefab();
    }


    public void Swap_Items_AtoA(int INDEX_Arrival, int INDEX_Start)//Armor->Armor
    {

        if (Equip_Armor_Item[INDEX_Arrival].Base_item == null)
        {

            Equip_Armor_Item[INDEX_Arrival].Base_item = Equip_Armor_Item[INDEX_Start].Base_item;
            Equip_Armor_Item[INDEX_Arrival].Upgrade = Equip_Armor_Item[INDEX_Start].Upgrade;
            Equip_Armor_Item[INDEX_Arrival].count = Equip_Armor_Item[INDEX_Start].count;



            Equip_Armor_Item[INDEX_Start].Delete_Data();
        }
        else
        {

            Item_Data temp = Equip_Armor_Item[INDEX_Arrival];
            Equip_Armor_Item[INDEX_Arrival] = Equip_Armor_Item[INDEX_Start];
            Equip_Armor_Item[INDEX_Start] = temp;
        }
    }

    public void Swap_Items_ItoA(int INDEX_Arrival, int INDEX_Start)//ETC->Armor
    {

        if (Equip_Armor_Item[INDEX_Arrival].Base_item == null)
        {

            Equip_Armor_Item[INDEX_Arrival].Base_item = Items[INDEX_Start].Base_item;
            Equip_Armor_Item[INDEX_Arrival].Upgrade = Items[INDEX_Start].Upgrade;
            Equip_Armor_Item[INDEX_Arrival].count = Items[INDEX_Start].count;

            Items[INDEX_Start].Delete_Data();
        }
        else
        {
            if (Items[INDEX_Arrival].Base_item.Type != Items[INDEX_Start].Base_item.Type)
            {
                return;
            }
            Item_Data temp = Equip_Armor_Item[INDEX_Arrival];
            Equip_Armor_Item[INDEX_Arrival] = Items[INDEX_Start];
            Items[INDEX_Start] = temp;
        }






    }

    public void Swap_Items_AtoI(int INDEX_Arrival, int INDEX_Start)//Armor->ETC
    {

        if (Items[INDEX_Arrival].Base_item == null)
        {

            Items[INDEX_Arrival].Base_item = Equip_Armor_Item[INDEX_Start].Base_item;
            Items[INDEX_Arrival].Upgrade = Equip_Armor_Item[INDEX_Start].Upgrade;
            Items[INDEX_Arrival].count = Equip_Armor_Item[INDEX_Start].count;



            Equip_Armor_Item[INDEX_Start].Delete_Data();
        }
        else
        {
            if (Items[INDEX_Arrival].Base_item.Type != Equip_Armor_Item[INDEX_Start].Base_item.Type)
            {
                return;
            }
            Item_Data temp = Items[INDEX_Arrival];
            Items[INDEX_Arrival] = Equip_Armor_Item[INDEX_Start];
            Equip_Armor_Item[INDEX_Start] = temp;
        }


    }


    public void Swap_Items_ItoI(int INDEX_Arrival, int INDEX_Start)//ETC->ETC
    {
        if (Items[INDEX_Arrival].Base_item == null)
        {
            Items[INDEX_Arrival].Base_item = Items[INDEX_Start].Base_item;
            Items[INDEX_Arrival].Upgrade = Items[INDEX_Start].Upgrade;
            Items[INDEX_Arrival].count = Items[INDEX_Start].count;



            Items[INDEX_Start].Delete_Data();
        }
        else
        {
            if ((Items[INDEX_Arrival].Base_item.Item_Index == Items[INDEX_Start].Base_item.Item_Index) && Items[INDEX_Arrival].Base_item is Use_Item)
            {
                Use_Item temp_Use = Items[INDEX_Arrival].Base_item as Use_Item;
                if (temp_Use.Max_Count >= Items[INDEX_Arrival].count + Items[INDEX_Start].count)
                {
                    Items[INDEX_Arrival].count += Items[INDEX_Start].count;
                    Items[INDEX_Start].Base_item = null;
                    Items[INDEX_Start].Upgrade = 0;
                    Items[INDEX_Start].count = 0;
                }
            }
            if ((Items[INDEX_Arrival].Base_item.Item_Index == Items[INDEX_Start].Base_item.Item_Index) && (Items[INDEX_Arrival].Base_item.Type == EItem_Slot_Type.ETC))
            {
                Use_Item temp_Use = (Use_Item)Items[INDEX_Arrival].Base_item;
                if (temp_Use.Max_Count >= Items[INDEX_Arrival].count + Items[INDEX_Start].count)
                {
                    Items[INDEX_Arrival].count += Items[INDEX_Start].count;
                    Items[INDEX_Start].Delete_Data();
                }
            }
            else
            {
                Item_Data temp = Items[INDEX_Arrival];
                Items[INDEX_Arrival] = Items[INDEX_Start];
                Items[INDEX_Start] = temp;
            }
        }
        //한쪽이 널일 경우 상실

    }

}
