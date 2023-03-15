using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Slot : Slot
{
    
    [SerializeField]
    protected Player_Inventory _Inventory;



    protected void Image_Set()//이미지 true면 투명도해제 false면 투명하게
    {
        Color temp = Color.white;
        temp.a = 1;
        if (Slot_IN_Item.Base_item == null)
        {
            Slot_Item_Image.sprite = Base_Image;
            Activation = false;
        }
        else
        Slot_Item_Image.sprite = Slot_IN_Item.Base_item.Item_Icon;
        Activation=true;



        Slot_Item_Image.color = temp;


    }
    public void Init_Slot_item(Item_Data Slot,int i)
    {
        Slot_INDEX = i;
        Slot_IN_Item = Slot;
        

        Image_Set();
    }

}
