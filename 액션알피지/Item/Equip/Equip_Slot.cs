using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Slot : Inventory_Slot
{
    


    public Equip_Slot_Type EST;

    protected void Image_Set()//이미지 true면 투명도해제 false면 투명하게
    {
        Color temp = Color.white;
        temp.a = 1;
        switch(EST)
        {
            case Equip_Slot_Type.Weapon:
                if (Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item == null)
                {
                    Slot_Item_Image.sprite = Base_Image;
                    Activation = false;
                }
                else
                {
                    Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item.Item_Icon;
                    Activation = true;
                }
                    break;
            default:
                if (Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX].Base_item == null)
                {
                    Slot_Item_Image.sprite = Base_Image;
                    Activation = false;
                }
                else
                {
                    Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Armor_Item[Slot_INDEX].Base_item.Item_Icon;
                    Activation = true;
                }
                break;
            
                
        }

        



        Slot_Item_Image.color = temp;


    }
    public void Init_Slot_item(int i)
    {
        Slot_INDEX = i;
        

        Image_Set();
    }

}
public enum Equip_Slot_Type
{
    Weapon,
    Helmet,
    Breastplate,
    Glove
}