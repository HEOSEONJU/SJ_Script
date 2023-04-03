using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equip_Weapon : Equip_Slot
{

    
    //public Weapon_Item weapon;


    public override void  Setting_Slot(int i=0)
    {


        Slot_Item_Image = GetComponent<Image>();
        if (Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item == null)
        {
            Color Temp_Color = Color.white;
            Temp_Color.a = 0;
            Slot_Item_Image.color = Temp_Color;

        }
        else
        {
            Slot_Item_Image.color = Color.white;
            Slot_Item_Image.sprite = Game_Master.instance.PM.Data.Equip_Weapon_Item.Base_item.Item_Icon;
        }
        Slot_Position = transform.parent;
        rect = GetComponentInParent<RectTransform>();
        Origin = rect.position;


        Canvas_Group = gameObject.AddComponent<CanvasGroup>();

        
        
        

    }


    public void Init_Slot_item()
    {
        
        Slot_Item_Image.sprite = Base_Image;

        Image_Set();
        Game_Master.instance.PM._WeaponSlotManager.Change_Weapon_Prefab();
    }

}
