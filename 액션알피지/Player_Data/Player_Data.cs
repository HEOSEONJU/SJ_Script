using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Data")]
public class Player_Data : ScriptableObject
{


    public Item_Data Equip_Weapon_Item;
    public List<Item_Data> Equip_Armor_Item;
    
    public List <Item_Data> Items;

    public int Gold;

    public Vector3 Last_Position;


    public void Swap_Items_WtoI(int INDEX_Arrival)//Weapon->ETC
    {
        if (Items[INDEX_Arrival].Base_item == null)
        {
            Items[INDEX_Arrival].Base_item = Equip_Weapon_Item.Base_item;
            Items[INDEX_Arrival].Upgrade = Equip_Weapon_Item.Upgrade;
            Items[INDEX_Arrival].count = Equip_Weapon_Item.count;

            Equip_Weapon_Item.Base_item = null;
            Equip_Weapon_Item.Upgrade = 0;
            Equip_Weapon_Item.count = 0;
        }
        else
        {
            Item_Data temp = Items[INDEX_Arrival];
            Items[INDEX_Arrival] = Equip_Weapon_Item;
            Equip_Weapon_Item = temp;
        }
        
    }

    public void Swap_Items_ItoW(int INDEX_Arrival)//ETC-Weapon
    {
        
        if (Items[INDEX_Arrival].Base_item.Type == Type.Weapon)
        {
            
            if (Equip_Weapon_Item.Base_item == null)
            {
                
                Equip_Weapon_Item.Base_item = Items[INDEX_Arrival].Base_item;
                Equip_Weapon_Item.Upgrade = Items[INDEX_Arrival].Upgrade;
                Equip_Weapon_Item.count = Items[INDEX_Arrival].count;

                Items[INDEX_Arrival].Base_item = null;
                Items[INDEX_Arrival].Upgrade = 0;
                Items[INDEX_Arrival].count = 0;
            }
            else
            {
                Item_Data temp = Items[INDEX_Arrival];
                Items[INDEX_Arrival] = Equip_Weapon_Item;
                Equip_Weapon_Item = temp;
            }
        }


        
        

            

        



    }


    public void Swap_Items_AtoA(int INDEX_Arrival, int INDEX_Start)//Armor-Armor
    {
        
        if (Equip_Armor_Item[INDEX_Arrival].Base_item == null)
        {
            
            Equip_Armor_Item[INDEX_Arrival].Base_item = Equip_Armor_Item[INDEX_Start].Base_item;
            Equip_Armor_Item[INDEX_Arrival].Upgrade = Equip_Armor_Item[INDEX_Start].Upgrade;
            Equip_Armor_Item[INDEX_Arrival].count = Equip_Armor_Item[INDEX_Start].count;


            
            Equip_Armor_Item[INDEX_Start].Base_item = null;
            Equip_Armor_Item[INDEX_Start].Upgrade = 0;
            Equip_Armor_Item[INDEX_Start].count = 0;
        }
        else
        {

            Item_Data temp = Equip_Armor_Item[INDEX_Arrival];
            Equip_Armor_Item[INDEX_Arrival] = Equip_Armor_Item[INDEX_Start];
            Equip_Armor_Item[INDEX_Start] = temp;
        }
    }

    public void Swap_Items_ItoA(int INDEX_Arrival, int INDEX_Start)//ETC-Armor
    {
        
        if (Items[INDEX_Start].Base_item.Type == Type.Armor)
        {
            
            if (Equip_Armor_Item[INDEX_Arrival].Base_item == null)
            {
                
                Equip_Armor_Item[INDEX_Arrival].Base_item = Items[INDEX_Start].Base_item;
                Equip_Armor_Item[INDEX_Arrival].Upgrade = Items[INDEX_Start].Upgrade;
                Equip_Armor_Item[INDEX_Arrival].count = Items[INDEX_Start].count;

                Items[INDEX_Start].Base_item = null;
                Items[INDEX_Start].Upgrade = 0;
                Items[INDEX_Start].count = 0;
            }
            else
            {
                
                Item_Data temp = Equip_Armor_Item[INDEX_Arrival];
                Equip_Armor_Item[INDEX_Arrival] = Items[INDEX_Start];
                Items[INDEX_Start] = temp;
            }
        }

        


    }

    public void Swap_Items_AtoI(int INDEX_Arrival, int INDEX_Start)//Armor-ETC
    {

        if (Items[INDEX_Arrival].Base_item == null)
        {
            
            Items[INDEX_Arrival].Base_item = Equip_Armor_Item[INDEX_Start].Base_item;
            Items[INDEX_Arrival].Upgrade = Equip_Armor_Item[INDEX_Start].Upgrade;
            Items[INDEX_Arrival].count = Equip_Armor_Item[INDEX_Start].count;


            
            Equip_Armor_Item[INDEX_Start].Base_item = null;
            Equip_Armor_Item[INDEX_Start].Upgrade = 0;
            Equip_Armor_Item[INDEX_Start].count = 0;
        }
        else
        {
            
            Item_Data temp = Items[INDEX_Arrival];
            Items[INDEX_Arrival] = Equip_Armor_Item[INDEX_Start];
            Equip_Armor_Item[INDEX_Start] = temp;
        }

        
    }


    public void  Swap_Items_ItoI(int INDEX_Arrival,int INDEX_Start)//ETC->ETC
    {
        if (Items[INDEX_Arrival].Base_item == null)
        {
            Items[INDEX_Arrival].Base_item = Items[INDEX_Start].Base_item;
            Items[INDEX_Arrival].Upgrade = Items[INDEX_Start].Upgrade;
            Items[INDEX_Arrival].count = Items[INDEX_Start].count;


            
            Items[INDEX_Start].Base_item = null;
            Items[INDEX_Start].Upgrade = 0;
            Items[INDEX_Start].count = 0;
        }
        else
        {
            if ((Items[INDEX_Arrival].Base_item.Item_Index == Items[INDEX_Start].Base_item.Item_Index) && (Items[INDEX_Arrival].Base_item.Type == Type.Use))
            {
                Use_Item temp_Use = (Use_Item)Items[INDEX_Arrival].Base_item;
                if (temp_Use.Max_Count >= Items[INDEX_Arrival].count+ Items[INDEX_Start].count)                 
                {
                    Items[INDEX_Arrival].count += Items[INDEX_Start].count;
                    Items[INDEX_Start].Base_item = null;
                    Items[INDEX_Start].Upgrade = 0;
                    Items[INDEX_Start].count = 0;
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


